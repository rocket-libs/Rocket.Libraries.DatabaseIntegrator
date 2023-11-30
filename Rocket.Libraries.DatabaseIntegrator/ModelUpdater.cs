namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    internal static class ModelUpdater
    {
        public static TModel Update<TModel, TIdentifier>(TModel oldModel, TModel newModel, ImmutableList<string> notForUpdate)
            where TModel : ModelBase<TIdentifier>
        {
            notForUpdate = notForUpdate ?? ImmutableList<string>.Empty;
            notForUpdate = notForUpdate.Add(nameof(ModelBase<TIdentifier>.Created))
                .Add(nameof(ModelBase<TIdentifier>.Id));

            if (oldModel == null)
            {
                throw new DatabaseIntegratorException("Old data to be updated was not supplied");
            }
            else if (newModel == null)
            {
                throw new DatabaseIntegratorException("New data to use for updating was not supplied");
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
                bool shouldUpdate = true;
                if (Initialization.PreserveGuidValuesDuringUpdate)
                {
                    shouldUpdate = singlePropertyToUpdate.PropertyType != typeof(Guid);
                }
                if (shouldUpdate)
                {
                    var newValue = singlePropertyToUpdate.GetValue(newModel);
                    singlePropertyToUpdate.SetValue(oldModel, newValue);
                }
            }

            return oldModel;
        }
    }
}