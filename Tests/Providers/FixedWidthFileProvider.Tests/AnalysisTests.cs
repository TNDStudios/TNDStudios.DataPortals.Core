﻿using System;
using Xunit;
using System.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Data;
using System.IO;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    public class AnalysisTests
    {
        /// <summary>
        /// Test of 5000 sales records to make sure the analysis 
        /// is done in a timely manner
        /// </summary>
        [Fact]
        public void Analyse_BigData_ColumnCount()
        {
            // Arrange
            String file = (new TestHelper()).GetResourceString(
                TestHelper.TestFile_GenericFixedWidth);
            FixedWidthFileProvider provider = new FixedWidthFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(
                new AnalyseRequest<object>
                {
                    Data = file
                }
                );

            // Assert
            Assert.Equal(12, definition.ItemProperties.Count);
        }

        [Fact]
        public void Analyse_BigData_RowCount()
        {
            // Arrange
            Stream file = (new TestHelper()).GetResourceStream(
                TestHelper.TestFile_GenericFixedWidth);
            FixedWidthFileProvider provider = new FixedWidthFileProvider();

            // Act
            DataItemDefinition definition = provider.Analyse(
                new AnalyseRequest<Object>()
                {
                    Data = file
                });
            provider.Connect(definition, file);
            DataTable data = provider.Read("");

            // Assert
            Assert.Equal(530, data.Rows.Count);
        }
    }
}
