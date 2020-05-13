using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.Validation.Services;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IWriterBase<TModel, TId>
        where TModel : ModelBase<TId>
        {
            Task<ValidationResponse<TId>> DeleteAsync (TId id);
        }

    public abstract class WriterBase<TModel, TId> : IWriterBase<TModel, TId>
        where TModel : ModelBase<TId>
        {
            private readonly IDatabaseHelper<TId> databaseHelper;
            private readonly IReaderBase<TModel, TId> entityReader;

            public WriterBase (
                IDatabaseHelper<TId> databaseHelper,
                IReaderBase<TModel, TId> entityReader)
            {
                this.databaseHelper = databaseHelper;
                this.entityReader = entityReader;
            }

            public async Task<ValidationResponse<TId>> DeleteAsync (TId id)
            {
                var record = await entityReader.GetByIdAsync (id, true);
                var noData = record == null;
                if (noData)
                {
                    throw new Exception ($"Could not find record with id '{id}'");
                }
                record.Deleted = true;
                await databaseHelper.SaveAsync (record);
                return new ValidationResponse<TId>
                {
                    Entity = id,
                };
            }

            protected virtual async Task<ValidationResponse<TId>> WriteAsync (TModel model, bool isUpdate)
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
                    model = await GetUpdatedModel (model);
                    await databaseHelper.SaveAsync (model);
                    return new ValidationResponse<TId>
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
                    return ModelUpdater.Update<TModel,TId>(currentVersion, model, null);
                }
            }

            private async Task<ValidationResponse<TId>> ValidateAsync (TModel model)
            {
                using (var modelValidator = new ModelValidator<TModel,TId> ())
                {
                    return new ValidationResponse<TId>
                    {
                    ValidationErrors = await modelValidator.ValidateAsync (model),
                    };
                }
            }
        }
}