﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Tests.DelimitedFile
{
    /// <summary>
    /// Support methods to load data from streams etc.
    /// </summary>
    public class TestHelper : EmbeddedTestHelperBase
    {
        // Constants for the test files (used so we can get the resouce stream
        // but also so we can abstract the creation of the data definition)
        public const String TestFile_DataTypes = "TestFiles.DataTypesTest.txt";
        public const String TestFile_Headers = "TestFiles.HeadersTest.txt";
        public const String TestFile_ISODates = "TestFiles.Dates.ISODates.txt";
        public const String TestFile_CustomDates = "TestFiles.Dates.CustomDates.txt";
        public const String TestFile_WriteTests = "TestFiles.WriteTest.txt";
        public const String TestFile_PKMergeFrom = "TestFiles.PrimaryKey.MergeFrom.txt";
        public const String TestFile_PKMergeTo = "TestFiles.PrimaryKey.MergeTo.txt";
        public const String TestFile_ExpressionTests = "TestFiles.ExpressionTest.txt";
        public const String TestFile_BigFileSalesRecords = "TestFiles.BigFiles.SalesRecords5000.csv";

        /// <summary>
        /// Get a test connection for use with the readers
        /// </summary>
        /// <returns>A test connection</returns>
        public DataConnection TestConnection()
        => new DataConnection()
            {
                ProviderType = DataProviderType.DelimitedFileProvider,
                LastUpdated = DateTime.Now,
                Id = Guid.NewGuid(),
                PropertyBag =
                    (new DelimitedFileProvider())
                    .PropertyBagTypes()
                    .Select(type =>
                        new PropertyBagItem()
                        {
                            Value = type.DefaultValue,
                            ItemType = type
                        })
                    .ToList()
            };

        /// <summary>
        /// Generate the data set for the testing of different different types
        /// </summary>
        /// <param name="testDefinition">Which test file to load</param>
        /// <returns>The prepared data table</returns>
        public DataTable PopulateDataTable(String testDefinition)
            => PopulateDataTable(testDefinition, TestConnection());
        public DataTable PopulateDataTable(String testDefinition, DataConnection connection)
        {
            // Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testDefinition);

            // Get the test definition (The columns, data types etc. for this file)
            DataItemDefinition definition = TestDefinition(testDefinition);

            // Create a new flat file provider
            IDataProvider provider = new DelimitedFileProvider()
            {
                TestMode = true // The provider should be marked as being in test mode
            };
            provider.Connect(definition, connection, resourceStream); // Connect to the location of the data

            // Read the data from the provider
            DataTable data = provider.Read(""); // Get the data

            // Return the data table
            return data;
        }

        /// <summary>
        /// Generate a test definition that is common between the different test files
        /// </summary>
        /// <returns>The common data definition</returns>
        public DataItemDefinition TestDefinition(String testDefinition)
        {
            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = new DataItemDefinition();

            switch (testDefinition)
            {
                case TestFile_DataTypes:

                    // Definition for different data types and the data defined by ordinal position
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OrdinalPosition = 0 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "CreatedDate", DataType = typeof(DateTime), OrdinalPosition = 1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Size", DataType = typeof(Double), OrdinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OrdinalPosition = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Active", DataType = typeof(Boolean), OrdinalPosition = 4 });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = false,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = false
                                            }); // There is a header record

                    break;

                case TestFile_Headers:

                    // Definition for getting the data by the name of the header
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OrdinalPosition = -1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description Header", DataType = typeof(String), OrdinalPosition = -1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Value", DataType = typeof(String), OrdinalPosition = -1 });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    break;

                case TestFile_ISODates:

                    // Definition for supplying a list of ISO (and bad) dates to test
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Date", DataType = typeof(DateTime), OrdinalPosition = 0 });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    definition.Culture = CultureInfo.InvariantCulture;

                    break;

                case TestFile_CustomDates:

                    // Definition for supplying a list of custom (and bad) dates to test
                    // where the format is defined as dd MMM yyyy
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Date", DataType = typeof(DateTime), OrdinalPosition = 0, Pattern = "dd MMM yyyy" });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_WriteTests:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "StringValue", DataType = typeof(String), OrdinalPosition = 0 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "DateValue", DataType = typeof(DateTime), OrdinalPosition = 1, Pattern = "dd MMM yyyy" });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "BooleanValue", DataType = typeof(Boolean), OrdinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "NumericValue", DataType = typeof(Double), OrdinalPosition = 3 });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_PKMergeFrom:
                case TestFile_PKMergeTo:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Primary Key Part 1", DataType = typeof(String), OrdinalPosition = 0, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Primary Key Part 2", DataType = typeof(String), OrdinalPosition = 1, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Data", DataType = typeof(String), OrdinalPosition = 2 });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_ExpressionTests:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OrdinalPosition = 0, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Value", DataType = typeof(int), OrdinalPosition = 1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Multiplier", DataType = typeof(Double), OrdinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Result", DataType = typeof(Double), OrdinalPosition = 3, Calculation = "(Value * Multiplier)", PropertyType = DataItemPropertyType.Calculated });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag.Add(
                                            new PropertyBagItem()
                                            {
                                                ItemType = new PropertyBagItemType()
                                                {
                                                    DataType = typeof(Boolean),
                                                    DefaultValue = true,
                                                    PropertyType = PropertyBagItemTypeEnum.HasHeaderRecord
                                                },
                                                Value = true
                                            }); // There is a header record

                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_BigFileSalesRecords:

                    break;
            }

            // Return the definition
            return definition;
        }
    }
}
