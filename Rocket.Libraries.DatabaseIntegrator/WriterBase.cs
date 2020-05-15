using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.Validation.Services;

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
            private readonly IReaderBase<TModel, TIdentifier> entityReader;

            public WriterBase (
                IDatabaseHelper<TIdentifier> databaseHelper,
                IReaderBase<TModel, TIdentifier> entityReader)
            {
                this.databaseHelper = databaseHelper;
                this.entityReader = entityReader;
            }

            public async Task<ValidationResponse<TIdentifier>> DeleteAsync (TIdentifier id)
            {
                var record = await entityReader.GetByIdAsync (id, true);
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

            public async Task<ValidationResponse<TIdentifier>> InsertAsync (TModel model)
            {
                return await WriteAsync (model, false);
            }

            public async Task<ValidationResponse<TIdentifier>> UpdateAsync (TModel model)
            {
                return await WriteAsync (model, true);
            }

            private async Task<ValidationResponse<TIdentifier>> WriteAsync (TModel model, bool isUpdate)
            {

                var validateResponse = await ValidateAsync (model);
                using (var validator = new DataValidator ())
                {
                    validator
                        .AddFailureCondition (model == null, "No data was supplied for saving.", true)
                        .AddFailureCondition (model.IsNew && isUpdate, "No Id was specified for the record to be updated.", false)
                        .AddFailureCondition (isUpdate == false && model.IsNew == false, "An Id was specified during a create operation. New records should not be submitted with Ids", false)
                        .ThrowExceptionOnInvalidRules ();
                }
                if (validateResponse.HasErrors)
                {
                    return validateResponse;
                }
                else
                {
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
            }

            private async Task<TModel> GetUpdatedModel (TModel model)
            {
                if (model.IsNew)
                {
                    return model;
                }
                else
                {
                    var currentVersion = await entityReader.GetByIdAsync (model.Id, true);
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