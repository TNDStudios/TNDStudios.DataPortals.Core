﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Mapping to CommonObject object to transform the properties
    /// for rendering to Json etc.
    /// </summary>
    public class CommonObjectModel
    {
        /// <summary>
        /// The Id for this object
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public Nullable<Guid> Id { get; set; }

        /// <summary>
        /// The name of the definition
        /// </summary>
        [JsonProperty]
        public String Name { get; set; }

        /// <summary>
        /// The description of the definition
        /// </summary>
        [JsonProperty]
        public String Description { get; set; }

        /// <summary>
        /// The last time the object was updated
        /// </summary>
        [JsonProperty]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The property bag for the connection object
        /// </summary>
        [JsonProperty]
        public List<PropertyBagItemModel> PropertyBag { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CommonObjectModel()
        {
            Id = Guid.Empty; // No Id by default but not null too
            LastUpdated = DateTime.Now; // Default date
            Name = String.Empty; // No name by default
            Description = String.Empty; // No description by default
            PropertyBag = new List<PropertyBagItemModel>(); // No properties by default
        }

    }
}
