using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.FormValidationHelper.Attributes;

namespace Rocket.Libraries.DatabaseIntegrator
{
    internal class ModelValidator<TModel,TIdentifier> : FormValidationBase<TModel> 
        where TModel : ModelBase<TIdentifier>
    {
        public override async Task<ImmutableList<ValidationError>> ValidateAsync(TModel unValidatedObject)
        {
            return await ValidateProxyAsync(
                unValidatedObject,
                OnNonNullValidationsAsync
            );
        }

        protected override async Task<ImmutableList<ValidationError>> OnNonNullValidationsAsync(TModel unValidatedObject, ImmutableList<ValidationError> validationErrors)
        {
            await Task.Run(() => { });
            return validationErrors;
        }
    }
}