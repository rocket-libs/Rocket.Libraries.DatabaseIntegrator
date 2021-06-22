using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.FormValidationHelper;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IWriterBase<TModel, TIdentifier>
        where TModel : ModelBase<TIdentifier>
        {
            Task<ValidationResponse<TIdentifier>> DeleteAsync (TIdentifier id);

            Task<ValidationResponse<TIdentifier>> InsertAsync (TModel model);

            Task<ValidationResponse<TIdentifier>> UpdateAsync (TModel model);
        }

    public abstract class WriterBase<TModel, TIdentifier> : IWriterBase<TModel, TIdentifier>
        where TModel : ModelBase<TIdentifier>
        {
            private readonly IDatabaseHelper<TIdentifier> databaseHelper;

        private IReaderBase<TModel, TIdentifier> Reader { get; }

            public WriterBase (
                IDatabaseHelper<TIdentifier> databaseHelper,
                IReaderBase<TModel, TIdentifier> reader)
            {
                this.databaseHelper = databaseHelper;
                Reader = reader;
        }

            public virtual async Task<ValidationResponse<TIdentifier>> DeleteAsync (TIdentifier id)
            {
                var record = await Reader.GetByIdAsync (id, true);
                var noData = record == null;
                if (noData)
                {
                    throw new Exception ($"Could not find record with id '{id}'");
                }
                record.Deleted = true;
                await databaseHelper.SaveAsync (record);
                return new ValidationResponse<TIdentifier>
                {
                    Entity = id,
                };
            }

            public virtual async Task<ValidationResponse<TIdentifier>> InsertAsync (TModel model)
            {
                return await WriteAsync (model, false);
            }

            public virtual async Task<ValidationResponse<TIdentifier>> UpdateAsync (TModel model)
            {
                return await WriteAsync (model, true);
            }

            private async Task<ValidationResponse<TIdentifier>> WriteAsync (TModel model, bool isUpdate)
            {
                var validateResponse = await ValidateAsync (model);
                if (validateResponse.HasErrors)
                {
                    return validateResponse;
                }
                else
                {
                    return await WriteValidatedAsync (model, isUpdate);
                }
            }

            private async Task<ValidationResponse<TIdentifier>> WriteValidatedAsync (TModel model, bool isUpdate)
            {
                if (model == null)
                {
                    throw new DatabaseIntegratorException ("No data was supplied for saving.");
                }
                else if (model.IsNew && isUpdate)
                {
                    throw new DatabaseIntegratorException ("No Id was specified for the record to be updated.");
                }
                if (isUpdate)
                {
                    model = await GetUpdatedModel (model);
                }
                await databaseHelper.SaveAsync (model);
                return new ValidationResponse<TIdentifier>
                {
                    Entity = model.Id,
                    ValidationErrors = ImmutableList<ValidationError>.Empty,
                };
            }

            private async Task<TModel> GetUpdatedModel (TModel model)
            {
                if (model.IsNew)
                {
                    return model;
                }
                else
                {
                    var currentVersion = await Reader.GetByIdAsync (model.Id, true);
                    return ModelUpdater.Update<TModel, TIdentifier> (currentVersion, model, null);
                }
            }

            private async Task<ValidationResponse<TIdentifier>> ValidateAsync (TModel model)
            {
                using (var modelValidator = new ModelValidator<TModel, TIdentifier> ())
                {
                    return new ValidationResponse<TIdentifier>
                    {
                    ValidationErrors = await modelValidator.ValidateAsync (model),
                    };
                }
            }
        }
}