﻿var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.apiDefinitions.page()
    },
    computed: {

        // Searchable list of the data definitions
        filteredApiDefinitions() {
            return this.page.apiDefinitions.filter(function (item) {
                return item.name.toLowerCase().indexOf(app.page.searchCriteria.toLowerCase()) > -1
            });
        },

        // Is the editor item saved?
        isSavedVisible() {

            if (this.page.editor.id != null &&
                this.page.editor.id != "")
                return "visible";
            else
                return "hidden";
        }
    },
    methods: {

        // Add a new item to the definition list
        newApiDefinition: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the api definition editor from the selected object
            app.page.editItem = editItem; // Reference to the origional item being edited
            this.loadApiDefinition(editItem.id); // Load the data from the server
        },

        // Delete the current data definition
        previewApiDefinition: function (credentialsLink) {

            // Get the editor id field
            var apiName = "";
            if (app.page.editor && app.page.editor.name) {
                apiName = app.page.editor.name;
            }

            // Make sure this is a real api definition
            if (apiName != "") {

                // Call the preview function
                tndStudios.models.apiDefinitions.preview(
                    app.page.packageId,
                    apiName,
                    credentialsLink.credentials.key);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Preview An Item That Is Not Saved Yet');

        },

        // Delete the current data definition
        deleteApiDefinition: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real api definition just in case
            // someone clicked the delete before it was saved
            if (idString != "") {

                // Call the delete function
                tndStudios.models.apiDefinitions.delete(
                    app.page.packageId,
                    idString,
                    app.deleteCallback);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Delete callback
        deleteCallback: function (success, data) {

            if (success) {

                // Clear the editor (as it was showing the api definition when it was deleted)
                app.page.editor.clear();

                // Remove the item from the api definitions list
                app.page.apiDefinitions = app.page.apiDefinitions.filter(
                    function (apiDefinition) {
                        return apiDefinition.id !== app.page.editItem.id;
                    });

                app.page.editItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Api Definition Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definition Deletion Failed");
        },

        // Save the contents of the editor object 
        saveApiDefinition: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // Call the Save function
                tndStudios.models.apiDefinitions.save(
                    app.page.packageId,
                    app.page.editor.toObject(),
                    app.saveCallback);
            }

        },

        // Save callback, assign the appropriate items
        saveCallback: function (success, data) {

            if (success) {

                // Some data came back?
                if (data.data) {

                    // Update the editor itself (possibily with new links or id if a new item)
                    app.page.editor.fromObject(data.data);

                    // Were we editing an existing item?
                    if (app.page.editItem != null) {

                        // Update the existing item
                        app.page.editItem.fromObject(data.data);
                    }
                    else {

                        // Add the new item to the list
                        app.page.editItem = new tndStudios.models.common.commonObject(data.data);
                        app.page.apiDefinitions.push(app.page.editItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Api Definition Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definition Could Not Be Saved");
        },

        // Start the load process
        load: function () {

            // Has an id been specified to load straight away?
            tndStudios.models.common.loadAtStart(this.loadApiDefinition);

            // Start loading the api definitions list
            app.loadApiDefinitions();

            // Start loading the connections list
            app.loadConnections();

            // Start loading the credentials list
            app.loadCredentials();

            // Start loading the data definitions list
            app.loadDataDefinitions();
        },

        // load api definitions from the server
        loadApiDefinitions: function () {

            // Call the Save endpoint
            tndStudios.models.apiDefinitions.list(
                app.page.packageId,
                null,
                app.loadApiDefinitionsCallback);
        },

        // Load was successful, assign the data
        loadApiDefinitionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.apiDefinitions = []; // clear the api definitions array

                    // Add the api definition objects back in with wrapper for additional functions
                    data.data.forEach(function (apiDefinition) {
                        app.page.apiDefinitions.push(new tndStudios.models.common.commonObject(apiDefinition)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definitions Could Not Be Loaded");
        },

        // load singular api definition from the server (as the list is only the headers)
        loadApiDefinition: function (id) {

            // Start the api call to load the connections
            tndStudios.models.apiDefinitions.get(
                app.page.packageId,
                id,
                app.loadApiDefinitionCallback);
        },

        // Load was successful, assign the data to the editor
        loadApiDefinitionCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.editor.fromObject(data.data);
                }
            }
        },

        // Load a list of connections for this package that can be used
        loadConnections: function () {

            // Load the list of available connections
            tndStudios.models.dataConnections.list(
                app.page.packageId,
                null,
                app.loadConnectionsCallback);
        },

        // Load callback, assign the data
        loadConnectionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.dataConnections = []; // clear the connections array

                    // Add the connection objects back in with wrapper for additional functions
                    data.data.forEach(function (connection) {
                        app.page.dataConnections.push(new tndStudios.models.common.commonObject(connection)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Connections Could Not Be Loaded");
        },

        // Load a list of data definitions for this package that can be used
        loadDataDefinitions: function () {

            // Load the list of available connections
            tndStudios.models.dataDefinitions.list(
                app.page.packageId,
                null,
                app.loadDataDefinitionsCallback);
        },

        // Load callback, assign the data
        loadDataDefinitionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.dataDefinitions = []; // clear the data definitions array

                    // Add the data definitions objects back in with wrapper for additional functions
                    data.data.forEach(function (dataDefinition) {
                        app.page.dataDefinitions.push(new tndStudios.models.common.commonObject(dataDefinition)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Data Definitions Could Not Be Loaded");
        },


        // Load a list of credentials for this package that can be used
        loadCredentials: function () {

            // Load the list of available credentials
            tndStudios.models.credentials.list(
                app.page.packageId,
                null,
                app.loadCredentialsCallback);
        },

        // Load callback, assign the data
        loadCredentialsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.credentialsStore = []; // clear the credentials array

                    // Add the credentials objects back in with wrapper for additional functions
                    data.data.forEach(function (credentials) {
                        app.page.credentialsStore.push(new tndStudios.models.credentials.credentials(credentials)); // Assign the Json package to the credentials listing
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Credentials List Could Not Be Loaded");
        },

        // Remove a set of credentials
        removeCredentials: function (item) {

            // Delete the item
            app.page.editor.credentialsLinks =
                app.page.editor.credentialsLinks.filter(function (listItem) {
                    return (item != listItem);
                });
        },

        // Add a new set of credentials to this api definition (assuming a set is selected)
        addCredentials: function () {

            // Something selected?
            if (app.page.selectedCredentials != undefined &&
                app.page.selectedCredentials != null &&
                app.page.selectedCredentials != '00000000-0000-0000-0000-000000000000') {

                // Check that the item has not already been assigned
                if (app.page.editor.credentialsLinks.filter(function (item) {
                    return (item.credentials.key == app.page.selectedCredentials);
                }).length == 0) {

                    // Add a new link to the array
                    var credentialsLink = new tndStudios.models.credentials.credentialsLink();

                    // Assign the values
                    credentialsLink.credentials.key = app.page.selectedCredentials; // Assign the key
                    credentialsLink.permissions.canRead = true; // Allow read access by default

                    // Loop the credentials array to pick up the name
                    var credentialCheck = app.page.credentialsStore.filter(function (item) {
                        return (item.id == app.page.selectedCredentials);
                    });

                    if (credentialCheck.length != 0)
                        credentialsLink.credentials.value = credentialCheck[0].name;

                    // Add the new link to the array in the editor
                    app.page.editor.credentialsLinks.push(credentialsLink);
                }
                else
                    tndStudios.utils.ui.notify(0, 'Cannot Add The Same Credentials More Than Once');
            }
        },
    }
});

// Create the validation rules for the main editor form
var validator = $("#editorForm").validate(
    {
        rules:
        {
            name: { required: true },
            dataConnection: { required: true },
            dataDefinition: { required: true }
        },
        messages:
        {
            name: "Name Of Api Definition Required",
            dataConnection: "Please specify the connection to use",
            dataDefinition: "Please specify the data definition to use"
        }
    });

// Start the load process to initialise the form
app.load();