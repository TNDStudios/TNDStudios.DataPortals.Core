using System;
using Xunit;
using AutoMapper;
using TNDStudios.DataPortals.UI;
using TNDStudios.DataPortals.UI.Models;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class AutoMapperTestsFixture : IDisposable
    {
        public IMapper TestMapper; // Mapper to use for the test fixture

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public AutoMapperTestsFixture()
        {
            // Initialise the mapper with the correct namespace
            AutoMapperProfile.Initialise();

            // Create the test mapper
            TestMapper = new Mapper(Mapper.Configuration);
        }

        /// <summary>
        /// Dispose of the mapper and the global mappings
        /// </summary>
        public void Dispose()
        {
            TestMapper = null;
            Mapper.Reset();
        }
    }

    public class AutoMapperTests : IClassFixture<AutoMapperTestsFixture>
    {
        private AutoMapperTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public AutoMapperTests(AutoMapperTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Passthrough to automappers stability check to see if the mappings
        /// pass their own internal sanity checks
        /// </summary>
        [Fact]
        public void Verify_Mappings()
        {
            // Arrange
            Boolean result = true;

            // Act
            try
            {
                // Ask Automapper to raise an exception if the configuration is not
                // deemed to be valid
                fixture.TestMapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test the linking of Guid key's from one object to the other 
        /// with a name value being brought along for population
        /// (Used when foreign keys to other objects are needed without
        /// problems caused by infinite loops with embedded byref objects)
        /// </summary>
        [Fact]
        public void Guid_To_GuidKeyValuePair()
        {
            // Arrange
            List<Guid> keys = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            // Act
            List<KeyValuePair<Guid, String>> keyValuePairs =
                fixture.TestMapper.Map<List<KeyValuePair<Guid, String>>>(keys);

            // Assert
            Assert.Equal(keys.Count, keyValuePairs.Count);
            for (Int32 itemId = 0; itemId < keys.Count; itemId++)
                Assert.Equal(keys[itemId], keyValuePairs[itemId].Key);
        }

        /// <summary>
        /// Test the linking of Guid key's from one object to the other 
        /// with a name value being brought along for population
        /// (Used when foreign keys to other objects are needed without
        /// problems caused by infinite loops with embedded byref objects)
        /// </summary>
        [Fact]
        public void GuidKeyValuePair_To_Guid()
        {
            // Arrange
            List<KeyValuePair<Guid, String>> keyValuePairs = new List<KeyValuePair<Guid, String>>()
            {
                new KeyValuePair<Guid, String>(Guid.NewGuid(), ""),
                new KeyValuePair<Guid, String>(Guid.NewGuid(), ""),
                new KeyValuePair<Guid, String>(Guid.NewGuid(), "")
            };

            // Act
            List<Guid> keys =
                fixture.TestMapper.Map<List<Guid>>(keyValuePairs);

            // Assert
            Assert.Equal(keys.Count, keyValuePairs.Count);
            for (Int32 itemId = 0; itemId < keys.Count; itemId++)
                Assert.Equal(keys[itemId], keyValuePairs[itemId].Key);
        }

        /// <summary>
        /// Test the Data Connection class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataConnection_To_Model()
        {
            // Arrange
            DataConnection connection = new DataConnection()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ConnectionString = "Connection String Content",
                ProviderType = DataProviderType.SQLProvider
            };

            // Act
            DataConnectionModel model =
                fixture.TestMapper.Map<DataConnectionModel>(connection);

            // Assert
            Assert.Equal(connection.Id, model.Id);
            Assert.Equal(connection.Name, model.Name);
            Assert.Equal(connection.Description, model.Description);
            Assert.Equal(connection.ConnectionString, model.ConnectionString);
            Assert.Equal(connection.ProviderType, (DataProviderType)model.ProviderType);
        }

        /// <summary>
        /// Test the Data Connection Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataConnectionModel_To_Domain()
        {
            // Arrange
            DataConnectionModel model = new DataConnectionModel()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ConnectionString = "Connection String Content",
                ProviderType = (Int32)DataProviderType.SQLProvider
            };

            // Act
            DataConnection connection =
                fixture.TestMapper.Map<DataConnection>(model);

            // Assert
            Assert.Equal(connection.Id, model.Id);
            Assert.Equal(connection.Name, model.Name);
            Assert.Equal(connection.Description, model.Description);
            Assert.Equal(connection.ConnectionString, model.ConnectionString);
            Assert.Equal(connection.ProviderType, (DataProviderType)model.ProviderType);
        }

        /// <summary>
        /// Test the API Definition class transformed to the API Model
        /// </summary>
        [Fact]
        public void ApiDefinition_To_Model()
        {
            // Arrange
            ApiDefinition definition = new ApiDefinition()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                DataConnection = Guid.NewGuid(),
                DataDefinition = Guid.NewGuid()
            };

            // Act
            ApiDefinitionModel model =
                fixture.TestMapper.Map<ApiDefinitionModel>(definition);

            // Assert
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.DataConnection, model.DataConnection.Key);
            Assert.Equal(definition.DataDefinition, model.DataDefinition.Key);

        }

        /// <summary>
        /// Test the API Definition Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void ApiDefinitionModel_To_Domain()
        {
            // Arrange
            ApiDefinitionModel model = new ApiDefinitionModel()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                DataConnection = new KeyValuePair<Guid, String>(Guid.NewGuid(), "Connection"),
                DataDefinition = new KeyValuePair<Guid, String>(Guid.NewGuid(), "Definition"),
                LastUpdated = new DateTime(2018, 12, 23),
                CredentialsLinks = new List<CredentialsLinkModel>()
                {
                    new CredentialsLinkModel(){ Credentials = new KeyValuePair<Guid, String>(Guid.NewGuid(), "Test Credentials Link") }
                },
                PropertyBag = new List<PropertyBagItemModel>()
                {
                    new PropertyBagItemModel()
                    {
                        ItemType = new PropertyBagItemTypeModel()
                        {
                            DataType = "System.Int64",
                            DefaultValue = (Int64)5,
                            PropertyType = 
                                new KeyValuePair<int, String>(
                                    (Int32)PropertyBagItemTypeEnum.RowsToSkip, 
                                    PropertyBagItemTypeEnum.RowsToSkip.GetEnumDescription()
                                    )
                        }
                    }
                },
                Aliases = new List<KeyValuePair<String, String>>()
                {
                    new KeyValuePair<String, String>("Column 1", "Alias Column 1")
                }
            };

            // Act
            ApiDefinition definition =
                fixture.TestMapper.Map<ApiDefinition>(model);

            // Assert
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
            Assert.Equal(definition.DataConnection, model.DataConnection.Key);
            Assert.Equal(definition.DataDefinition, model.DataDefinition.Key);
            Assert.Equal(definition.LastUpdated, model.LastUpdated);
            Assert.Equal(definition.CredentialsLinks.Count, model.CredentialsLinks.Count);
            Assert.Equal(definition.Aliases.Count, model.Aliases.Count);
        }

        /// <summary>
        /// Test the Data Item Definition class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemDefinition_To_Model()
        {
            String culture = "en-GB";
            String encoding = "utf-8";
            DataItemDefinition definition = new DataItemDefinition()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Culture = CultureInfo.GetCultureInfo(culture),
                EncodingFormat = Encoding.GetEncoding(encoding)
            };

            // Act
            DataItemDefinitionModel model =
                fixture.TestMapper.Map<DataItemDefinitionModel>(definition);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
        }

        /// <summary>
        /// Test the Data Item Property class transformed to the API Model
        /// </summary>
        [Fact]
        public void DataItemProperty_To_Model()
        {
            String dataType = "System.Int64";
            DataItemProperty property = new DataItemProperty()
            {
                Id = Guid.NewGuid(),
                Calculation = "calculated value",
                DataType = Type.GetType(dataType),
                Description = "description value",
                Key = true,
                Name = "name value",
                OrdinalPosition = 100001,
                Path = "path/to/the/value",
                Pattern = "dd MMM yyyy",
                PropertyType = DataItemPropertyType.Calculated,
                Quoted = true
            };

            // Act
            DataItemPropertyModel model =
                fixture.TestMapper.Map<DataItemPropertyModel>(property);

            // Assert
            Assert.Equal(property.Id, model.Id);
            Assert.Equal(property.Calculation, model.Calculation);
            Assert.Equal(dataType, model.DataType);
            Assert.Equal(property.Description, model.Description);
            Assert.Equal(property.Key, model.Key);
            Assert.Equal(property.Name, model.Name);
            Assert.Equal(property.OrdinalPosition, model.OrdinalPosition);
            Assert.Equal(property.Path, model.Path);
            Assert.Equal(property.Pattern, model.Pattern);
            Assert.Equal(property.PropertyType, model.PropertyType);
            Assert.Equal(property.Quoted, model.Quoted);
        }

        /// <summary>
        /// Test the Data Item Definition Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataItemDefinitionModel_To_Domain()
        {
            String culture = "en-GB";
            String encoding = "utf-8";
            DataItemDefinitionModel model = new DataItemDefinitionModel()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Culture = culture,
                EncodingFormat = encoding
            };

            // Act
            DataItemDefinition definition =
                fixture.TestMapper.Map<DataItemDefinition>(model);

            // Assert
            Assert.Equal(definition.ItemProperties.Count, model.ItemProperties.Count);
            Assert.Equal(culture, model.Culture);
            Assert.Equal(encoding, model.EncodingFormat);
            Assert.Equal(definition.Id, model.Id);
            Assert.Equal(definition.Name, model.Name);
            Assert.Equal(definition.Description, model.Description);
        }

        /// <summary>
        /// Test the Data Item Property Model transformed to the Domain object
        /// </summary>
        [Fact]
        public void DataItemPropertyModel_To_Domain()
        {
            String dataType = "System.Int64";
            DataItemPropertyModel model = new DataItemPropertyModel()
            {
                Calculation = "calculated value",
                DataType = dataType,
                Description = "description value",
                Key = true,
                Name = "name value",
                OrdinalPosition = 100001,
                Path = "path/to/the/value",
                Pattern = "dd MMM yyyy",
                PropertyType = DataItemPropertyType.Calculated,
                Quoted = true
            };

            // Act
            DataItemProperty property =
                fixture.TestMapper.Map<DataItemProperty>(model);

            // Assert
            Assert.Equal(model.Calculation, property.Calculation);
            Assert.Equal(Type.GetType(dataType), property.DataType);
            Assert.Equal(model.Description, property.Description);
            Assert.Equal(model.Key, property.Key);
            Assert.Equal(model.Name, property.Name);
            Assert.Equal(model.OrdinalPosition, property.OrdinalPosition);
            Assert.Equal(model.Path, property.Path);
            Assert.Equal(model.Pattern, property.Pattern);
            Assert.Equal(model.PropertyType, property.PropertyType);
            Assert.Equal(model.Quoted, property.Quoted);
        }

        /// <summary>
        /// Test The Base Data Provider Domain Object transformed to the Model
        /// Note: Property Bag Types cannot be tested from the provider base
        /// </summary>
        [Fact]
        public void DataProviderBase_To_Model()
        {
            // Arrange
            IDataProvider dataProvider = new DataProviderBase()
            {
                CanAnalyse = true,
                CanList = true,
                CanRead = true,
                CanWrite = true,
                Connection = null,
                ObjectName = "",
                TestMode = false
            };

            // Act
            DataProviderModel result = fixture.TestMapper.Map<DataProviderModel>(dataProvider);

            // Assert
            Assert.Equal(dataProvider.CanAnalyse, result.CanAnalyse);
            Assert.Equal(dataProvider.CanList, result.CanList);
            Assert.Equal(dataProvider.CanRead, result.CanRead);
            Assert.Equal(dataProvider.CanWrite, result.CanWrite);
        }

        /// <summary>
        /// Test A Concreate Data Provider Domain Object transformed to the Model
        /// </summary>
        [Fact]
        public void DelimitedDataProvider_To_Model()
        {
            // Arrange
            DelimitedFileProvider dataProvider = new DelimitedFileProvider()
            {
                CanAnalyse = true,
                CanList = true,
                CanRead = true,
                CanWrite = true,
                Connection = null,
                ObjectName = "",
                TestMode = false
            };

            // Act
            DataProviderModel result = fixture.TestMapper.Map<DataProviderModel>(dataProvider);

            // Assert
            Assert.Equal(dataProvider.CanAnalyse, result.CanAnalyse);
            Assert.Equal(dataProvider.CanList, result.CanList);
            Assert.Equal(dataProvider.CanRead, result.CanRead);
            Assert.Equal(dataProvider.CanWrite, result.CanWrite);
        }

    }
}
