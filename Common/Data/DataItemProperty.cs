﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Definition of a property member of a data item
    /// </summary>
    [JsonObject]
    public class DataItemProperty : DataItemPropertyBase
    {
        /// <summary>
        /// The data type for the property
        /// </summary>
        [JsonProperty]
        public Type DataType { get; set; }
        
        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemProperty()
        {
            base.Initialise();
            DataType = typeof(String); // String by default
        }
    }
}
