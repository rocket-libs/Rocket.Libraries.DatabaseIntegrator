namespace Rocket.Libraries.DatabaseIntegrator
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Dapper.Contrib.Extensions;

    public abstract class ModelBase<TId>
    {
        /// <summary>
        /// Gets or sets a value that uniquely identifies this record.
        /// </summary>
        public virtual TId Id { get; set; }

        /// <summary>
        /// Gets a value indicating whether the data in this record has been saved to the database or not.
        /// </summary>
        [JsonIgnore]
        [Computed]
        public bool IsNew => EqualityComparer<TId>.Default.Equals (Id, default);

        /// <summary>
        /// Gets or sets a value indicating whether this record is 'soft-deleted' or not
        /// </summary>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date when the record was first created.
        /// </summary>
        /// <remarks>This value is not available in legacy models.</remarks>
        [JsonIgnore]
        public virtual DateTime Created { get; set; }
    }
}