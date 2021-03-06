﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals
{
    /// <summary>
    /// An enumeration to define the different object types within the system
    /// so it could be used for factory classes or just general classification
    /// Replicated in javascript - tndStudios.models.common (common.js)
    /// </summary>
    public enum ObjectTypes
    {
        [Description("Api Definitions")]
        ApiDefinitions = 1,

        [Description("Data Definitions")]
        DataDefinitions = 2,

        [Description("Connections")]
        Connections = 3,

        [Description("Credentials")]
        Credentials = 4,

        [Description("Transformations")]
        Transformations = 5

    }

    /// <summary>
    /// Common object properties for items that may be saved
    /// out to a system or need descriptions etc.
    /// </summary>
    public class CommonObject
    {
        /// <summary>
        /// The parent package object (not always assigned but 
        /// if it is needed for external references to get data)
        /// </summary>
        [JsonIgnore]
        public Package ParentPackage { get; set; }

        /// <summary>
        /// The identifier of this object
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of this object
        /// </summary>
        [JsonProperty]
        public String Name { get; set; }

        /// <summary>
        /// The description of this object
        /// </summary>
        [JsonProperty]
        public String Description { get; set; }

        /// <summary>
        /// The last time that this object was updated
        /// </summary>
        [JsonProperty]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Adhoc configuration items for different providers
        /// </summary>
        public List<PropertyBagItem> PropertyBag { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CommonObject()
        {
            Id = Guid.NewGuid(); // A new identifier by default
            Name = String.Empty; // The name is empty by default
            Description = String.Empty; // No description by default
            LastUpdated = DateTime.Now; // Default value (Mainly used for refreshing caches)
            PropertyBag = new List<PropertyBagItem>(); // New empty property bag by default
        }
    }
}
