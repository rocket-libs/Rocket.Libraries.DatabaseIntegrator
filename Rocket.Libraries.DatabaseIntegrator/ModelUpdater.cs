namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Rocket.Libraries.Validation.Services;

    internal static class ModelUpdater
    {
        public static TModel Update<TModel,TId>(TModel oldModel, TModel newModel, ImmutableList<string> notForUpdate)
            where TModel : ModelBase<TId>
        {
            notForUpdate = notForUpdate ?? ImmutableList<string>.Empty;
            notForUpdate = notForUpdate.Add(nameof(ModelBase<TId>.Created))
                .Add(nameof(ModelBase<TId>.Id));

            using (var validator = new DataValidator())
            {
                validator
                    .AddFailureCondition(oldModel == null, "Old data to be updated was not supplied", true)
                    .AddFailureCondition(newModel == null, "New data to use for updating was not supplied", true)
                    .ThrowExceptionOnInvalidRules();
            }

            var manyPropertiesToUpdate = typeof(TModel)
                .GetProperties()
                .Where(prop =>
                    prop.CanRead &&
                    prop.CanWrite &&
                    !notForUpdate.Any(excludedProperty => excludedProperty == prop.Name))
                .ToImmutableList();

            foreach (var singlePropertyToUpdate in manyPropertiesToUpdate)
            {
                if (singlePropertyToUpdate.PropertyType != typeof(Guid))
                {
                    var newValue = singlePropertyToUpdate.GetValue(newModel);
                    singlePropertyToUpdate.SetValue(oldModel, newValue);
                }
            }

            return oldModel;
        }
    }
}