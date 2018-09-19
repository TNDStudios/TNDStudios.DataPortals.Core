﻿using System;
using Xunit;
using System.Reflection;
using System.IO;
using TNDStudios.DataPortals.Data;
using System.Data;
using System.Globalization;

namespace TNDStudios.DataPortals.Tests.FlatFile
{
    public class KeyAndMergeTests
    {
        [Fact]
        public void Merge_Files_2_Part_Primary_Key()
        {
            // Arrange
            DataItemDefinition definition = TestHelper.TestDefinition(TestHelper.TestFile_PKMergeFrom); // Get the test definition of what to merge from (but also to)

            DataTable baseData = TestHelper.PopulateDataTable(TestHelper.TestFile_PKMergeTo); // Get the data
            DataTable mergeData = TestHelper.PopulateDataTable(TestHelper.TestFile_PKMergeFrom); // Get the data

            Stream testStream = new MemoryStream(); // A blank stream to write data to
            IDataProvider provider = new FlatFileProvider(); // A flat file provider to use to write the data

            // Act
            provider.Connect(definition, testStream); // Connect to the blank stream
            provider.Write(baseData, ""); // Write the data to the empty stream
            provider.Write(mergeData, ""); // Write some more records with some updates and some adds
            DataTable mergedData = provider.Read(""); // Get the new data set back

            // Assert
            Assert.True(mergedData.Rows.Count == 6); // Expect of the total of 8 rows, 2 should merge
        }
    }
}
