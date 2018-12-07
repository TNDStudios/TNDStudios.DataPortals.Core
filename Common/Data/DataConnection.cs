﻿using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Link between a data provider and a connection string that feeds it
    /// </summary>
    public class DataConnection : CommonObject
    {
        /// <summary>
        /// The type of provider for this data connection
        /// </summary>
        public DataProviderType ProviderType { get; set; }

        /// <summary>
        /// The connection string for this connection
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// Which credentials pack to use for this connection
        /// items in the credentials pack can be transposed in to connection strings
        /// </summary>
        public Guid Credentials { get; set; }

        /// <summary>
        /// Adhoc configuration items for different providers
        /// </summary>
        public List<PropertyBagItem> PropertyBag { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataConnection()
        {
            Credentials = Guid.Empty; // No credentials by default
            ProviderType = DataProviderType.Unknown; // No provider type by default
            ConnectionString = String.Empty; // No connection string by default
            PropertyBag = new List<PropertyBagItem>(); // No properties by default
        }
    }
}
