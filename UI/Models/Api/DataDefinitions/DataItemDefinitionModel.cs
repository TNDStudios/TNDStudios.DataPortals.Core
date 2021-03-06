﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using System.Text;
using System.Globalization;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Web friendly model to represent the Data Item Definition class
    /// where the "Culture" and various other data types are not suited
    /// for sending back over the API calls
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class DataItemDefinitionModel : CommonObjectModel
    {
        /// <summary>
        /// The list of properties that define the data item
        /// </summary>
        [JsonProperty]
        public List<DataItemPropertyModel> ItemProperties { get; set; }
        
        /// <summary>
        /// The specific culture information for this definition
        /// </summary>
        [JsonProperty]
        public String Culture { get; set; }

        /// <summary>
        /// The encoding format for the definition
        /// </summary>
        [JsonProperty]
        public String EncodingFormat { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataItemDefinitionModel() : base()
        {
            Name = "";
            Description = "";
            ItemProperties = new List<DataItemPropertyModel>();
            EncodingFormat = Encoding.Default.WebName;
            Culture = CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Check to see if the data definition is valid
        /// and passes a certain amount of checks
        /// </summary>
        /// <returns>If the data definition appears to be valid</returns>
        public Boolean IsValid
        {
            get =>
                (EncodingFormat != String.Empty &&
                    Culture != String.Empty &&
                    ItemProperties?.Count != 0);
        }
    }
}
