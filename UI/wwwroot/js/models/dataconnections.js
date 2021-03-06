﻿var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataConnections =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.connections = []; // The list of connections for this package
            this.searchCriteria = ""; // The filter for the connections list

            this.editor = new tndStudios.models.dataConnections.dataConnection(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it
            this.providerTypes = []; // The list of available provider types
            this.credentialsStore = []; // The list of credentials for this package

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Data Connection Model
        dataConnection: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';
            this.providerType = 0;
            this.providerData = new tndStudios.models.dataProviders.dataProvider();
            this.connectionString = '';
            this.credentials = new tndStudios.models.common.keyValuePair();
            this.credentials.key = '00000000-0000-0000-0000-000000000000';
            this.propertyBag = []; // Property bag for this connection
            this.objectName = ""; // The name of the object to connect to in this connection
            this.objectList = []; // The list of objects from the provider

            // Copy the content of this connection from another connection
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.providerType = fromObject.providerType;
                this.providerData = fromObject.providerData;
                this.connectionString = fromObject.connectionString;
                this.credentials = fromObject.credentials;
                this.propertyBag = fromObject.propertyBag;
                this.objectName = fromObject.objectName;
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    ProviderType: this.providerType,
                    ConnectionString: this.connectionString,
                    Credentials: this.credentials,
                    PropertyBag: this.propertyBag,
                    ObjectName: this.objectName
                };

                return result;
            }

            // Clear this connection object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
                this.providerType = 0;
                this.providerData = new tndStudios.models.dataProviders.dataProvider();
                this.connectionString = '';
                this.credentials = new tndStudios.models.common.keyValuePair();
                this.credentials.key = '00000000-0000-0000-0000-000000000000';
                this.propertyBag = [];
                this.objectName = "";
                this.objectList = [];
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Load the list of available connections
        list: function (packageId, filter, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection',
                'GET',
                null,
                callback);
        },

        // List the objects in this connection
        listObjects: function (packageId, saveObject, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/queryobjects',
                'POST',
                saveObject,
                callback);
        },
        
        // Get the full version of the connection
        get: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + id,
                'GET',
                null,
                callback);
        },

        // Delete an existing connection
        delete: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + id,
                'DELETE',
                null,
                callback
            );
        },

        // The the api call to save the connection
        save: function (packageId, saveObject, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection',
                'POST',
                saveObject,
                callback
            );
        },

        // Test the connection provided
        test: function (packageId, saveObject, callback) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/test',
                'POST',
                saveObject,
                callback
            );
        },

        // The the api call to sample the connection with this definition
        sample: function (
            packageId,
            connectionId,
            sampleObject,
            callback) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + connectionId + '/sample',
                'POST',
                sampleObject,
                callback
            );
        },

        // The the api call to save the data definition
        analyse: function (
            packageId,
            connectionId,
            callback) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + connectionId + '/analyse',
                'GET',
                null,
                callback
            );
        },

        // The the api call to load the provider types
        providers: function (packageId, callback) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/providers',
                'GET',
                null,
                callback
            );
        },

    };