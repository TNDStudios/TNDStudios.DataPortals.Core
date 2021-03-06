﻿using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.Api
{
    /// <summary>
    /// Define how an API Endpoint 
    /// </summary>
    public class ApiDefinition : CommonObject
    {
        /// <summary>
        /// Pointer to the definition to use
        /// </summary>
        public Guid DataDefinition { get; set; }

        /// <summary>
        /// Pointer to the data connection to use
        /// </summary>
        public Guid DataConnection { get; set; }

        /// <summary>
        /// Links to credentials and what each set of credentials will
        /// allow the credentials to do
        /// </summary>
        public List<CredentialsLink> CredentialsLinks { get; set; }

        /// <summary>
        /// List of property aliases (as the raw definition might want to be overridden)
        /// </summary>
        public List<KeyValuePair<String, String>> Aliases { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiDefinition() : base()
        {
            DataDefinition = Guid.Empty; // No Data Definition by default
            DataConnection = Guid.Empty; // No Connection by default
            CredentialsLinks = new List<CredentialsLink>(); // Create an empty list of credential links
            PropertyBag = new List<PropertyBagItem>(); // No properties by default
            Aliases = new List<KeyValuePair<String, String>>(); // New empty list of aliases
        }
    }
}
