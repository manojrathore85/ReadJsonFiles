using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ReadJsonFiles.Controllers;
using ReadJsonFiles.Interface;
using ReadJsonFiles.Model;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;

namespace ReadJsonFiles.Repository
{
    public class ReadJsonFileRepository : IReadJsonFile
    {
        private readonly ILogger<ReadJsonFileRepository> _logger;
        public ReadJsonFileRepository(ILogger<ReadJsonFileRepository> logger)
        {
            _logger = logger;
        }

        public Task<string> CreateRRRJsonFile()
        {
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStringsSQL").GetSection("ConnectionString").Value;
            RRRFileModel rrrmodel = new RRRFileModel();
            try
            {
                string query = "";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        rrrmodel.ContentDate = reader["itleird_sRelationshipStartNodeNodeID"].ToString();
                                        rrrmodel.StartNodeID = reader["itleird_sRelationshipStartNodeNodeID"].ToString();
                                        rrrmodel.StartNodeIDType = reader["itleird_sRelationshipStartNodeNodeIDType"].ToString();
                                        rrrmodel.EndNodeID = reader["itleird_sRelationshipEndNodeNodeID"].ToString();
                                        rrrmodel.RecordCount = reader["itleird_sRelationshipEndNodeNodeIDType"].ToString();
                                        rrrmodel.RelationshipType = reader["itleird_sRelationshipRelationshipType"].ToString();
                                        rrrmodel.RelationshipStatus = reader["itleird_sRelationshipRelationshipStatus"].ToString();
                                        rrrmodel.Period1startDate = reader["itleird_sRelationshipPeriod1startDate"].ToString();
                                        rrrmodel.Period1endDate = reader["itleird_sRelationshipPeriod1endDate"].ToString();
                                        rrrmodel.Period1periodType = reader["itleird_sRelationshipPeriod1periodType"].ToString();
                                        rrrmodel.Period2startDate = reader["itleird_sRelationshipPeriod2startDate"].ToString();
                                        rrrmodel.Period2endDate = reader["itleird_sRelationshipPeriod2endDate"].ToString();
                                        rrrmodel.Period2periodType = reader["itleird_sRelationshipPeriod2periodType"].ToString();
                                        rrrmodel.LastUpdateDate = reader["itleird_sRegistrationLastUpdateDate"].ToString();
                                        rrrmodel.RegistrationStatus = reader["itleird_sRegistrationRegistrationStatus"].ToString();
                                        rrrmodel.NextRenewalDate = reader["itleird_sRegistrationNextRenewalDate"].ToString();
                                        rrrmodel.ManagingLOU = reader["itleird_sRegistrationManagingLOU"].ToString();
                                        rrrmodel.ValidationSources = reader["itleird_sRegistrationValidationSources"].ToString();
                                        rrrmodel.ValidationDocuments = reader["itleird_sRegistrationValidationDocuments"].ToString();                                 


                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No rows found.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }

               

                XmlDocument xmlDoc = new XmlDocument();

                // Create the XML declaration
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                // Add the XML declaration to the document
                xmlDoc.AppendChild(xmlDeclaration);

                // Create the root element
                XmlElement relationshipDataElement = xmlDoc.CreateElement("rr", "RelationshipData", "http://www.gleif.org/data/schema/rr/2016");
                xmlDoc.AppendChild(relationshipDataElement);

                // Create the Header element
                XmlElement headerElement = xmlDoc.CreateElement("rr", "Header", "http://www.gleif.org/data/schema/rr/2016");
                relationshipDataElement.AppendChild(headerElement);

                // Create and append elements inside Header
                AppendElement(xmlDoc, headerElement, "rr", "ContentDate", rrrmodel.ContentDate);
                AppendElement(xmlDoc, headerElement, "rr", "Originator", "abc");
                AppendElement(xmlDoc, headerElement, "rr", "FileContent", "LOU_FULL_PUBLISHED");
                AppendElement(xmlDoc, headerElement, "rr", "RecordCount", "1");

                // Create the RelationshipRecords element
                XmlElement relationshipRecordsElement = xmlDoc.CreateElement("rr", "RelationshipRecords", "http://www.gleif.org/data/schema/rr/2016");
                relationshipDataElement.AppendChild(relationshipRecordsElement);

                // Create the RelationshipRecord element
                XmlElement relationshipRecordElement = xmlDoc.CreateElement("rr", "RelationshipRecord", "http://www.gleif.org/data/schema/rr/2016");
                relationshipRecordsElement.AppendChild(relationshipRecordElement);

                // Create and append elements inside RelationshipRecord
                XmlElement relationshipElement = xmlDoc.CreateElement("rr", "Relationship", "http://www.gleif.org/data/schema/rr/2016");
                relationshipRecordElement.AppendChild(relationshipElement);

                AppendElement(xmlDoc, relationshipElement, "rr", "StartNode", rrrmodel.StartNodeID, new string[] { "NodeIDType", "LEI" });
                AppendElement(xmlDoc, relationshipElement, "rr", "EndNode", rrrmodel.EndNodeID, new string[] { "NodeIDType", "LEI" });
                AppendElement(xmlDoc, relationshipElement, "rr", "RelationshipType", rrrmodel.RelationshipType);

                XmlElement relationshipPeriodsElement = xmlDoc.CreateElement("rr", "RelationshipPeriods", "http://www.gleif.org/data/schema/rr/2016");
                relationshipElement.AppendChild(relationshipPeriodsElement);

                XmlElement relationshipPeriodElement1 = xmlDoc.CreateElement("rr", "RelationshipPeriod", "http://www.gleif.org/data/schema/rr/2016");
                relationshipPeriodsElement.AppendChild(relationshipPeriodElement1);
                AppendElement(xmlDoc, relationshipPeriodElement1, "rr", "StartDate", rrrmodel.Period1startDate);
                AppendElement(xmlDoc, relationshipPeriodElement1, "rr", "PeriodType", rrrmodel.Period1endDate);

                XmlElement relationshipPeriodElement2 = xmlDoc.CreateElement("rr", "RelationshipPeriod", "http://www.gleif.org/data/schema/rr/2016");
                relationshipPeriodsElement.AppendChild(relationshipPeriodElement2);
                AppendElement(xmlDoc, relationshipPeriodElement2, "rr", "StartDate", rrrmodel.Period2startDate);
                AppendElement(xmlDoc, relationshipPeriodElement2, "rr", "EndDate", rrrmodel.Period2endDate);
                AppendElement(xmlDoc, relationshipPeriodElement2, "rr", "PeriodType", rrrmodel.Period2periodType);

                AppendElement(xmlDoc, relationshipElement, "rr", "RelationshipStatus", rrrmodel.RegistrationStatus);

                // Create the Registration element
                XmlElement registrationElement = xmlDoc.CreateElement("rr", "Registration", "http://www.gleif.org/data/schema/rr/2016");
                relationshipRecordElement.AppendChild(registrationElement);

                AppendElement(xmlDoc, registrationElement, "rr", "InitialRegistrationDate", rrrmodel.InitialRegistrationDate);
                AppendElement(xmlDoc, registrationElement, "rr", "LastUpdateDate", rrrmodel.LastUpdateDate);
                AppendElement(xmlDoc, registrationElement, "rr", "RegistrationStatus", rrrmodel.RegistrationStatus);
                AppendElement(xmlDoc, registrationElement, "rr", "NextRenewalDate", rrrmodel.NextRenewalDate);
                AppendElement(xmlDoc, registrationElement, "rr", "ManagingLOU", rrrmodel.ManagingLOU);
                AppendElement(xmlDoc, registrationElement, "rr", "ValidationSources", rrrmodel.ValidationSources);
                AppendElement(xmlDoc, registrationElement, "rr", "ValidationDocuments", rrrmodel.ValidationDocuments);

                string path = "C:\\Mohan\\RelationshipData.xml";
                // Save the XML document to a file
                xmlDoc.Save(path);
                Console.WriteLine("XML file created successfully!");
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        static void AppendElement(XmlDocument xmlDoc, XmlElement parentElement, string prefix, string elementName, string elementValue, string[] attributeData = null)
        {
            XmlElement newElement = xmlDoc.CreateElement(prefix, elementName, "http://www.gleif.org/data/schema/rr/2016");
            newElement.InnerText = elementValue;
            if (attributeData != null)
            {
                for (int i = 0; i < attributeData.Length; i += 2)
                {
                    newElement.SetAttribute(attributeData[i], attributeData[i + 1]);
                }
            }
            parentElement.AppendChild(newElement);
        }
        public async Task<string> ReadEcpJsonFileAsync()
        {
            _logger.LogInformation("Calling ReadJsonFileAsync");
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStringsSQL").GetSection("ConnectionString").Value;
            string filePath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("ExcPath").Value;
            string filename = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("ExcFileName").Value;
            string path = $"{filePath}\\{filename}";
            string status = "";
            RenameFiles(filePath);
            _logger.LogInformation($"filePath {filePath}");
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                _logger.LogInformation($"successfully collected json records from the files");
                int i = 0;
                try
                {
                    JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);
                    JsonElement rootElement = jsonDocument.RootElement;
                    JsonElement relationsElement = rootElement.GetProperty("exceptions");
                    ReadExceptionJsonFile readExceptionJsonFile = new ReadExceptionJsonFile();
                    // Iterate through each item in the "relations" array
                    foreach (JsonElement relationElement in relationsElement.EnumerateArray())
                    {
                        i++;
                        ReadException exception = new ReadException();
                        if (relationElement.TryGetProperty("LEI", out JsonElement LEIElement))
                        {
                            exception.LEI = LEIElement.GetProperty("$").GetString();
                        }
                        if (relationElement.TryGetProperty("ExceptionCategory", out JsonElement ExceptionCategoryElement))
                        {
                            exception.ExceptionCategory = ExceptionCategoryElement.GetProperty("$").GetString();
                        }
                        List<ExceptionReason> listexceptionReasons = new List<ExceptionReason>();
                        if (relationElement.TryGetProperty("ExceptionReason", out JsonElement ExceptionReasonElement))
                        {

                            if (ExceptionReasonElement.ValueKind == JsonValueKind.Object)
                            {
                                ExceptionReason ExceptionReason = new ExceptionReason();
                                ExceptionReason.ExceptionReasons = ExceptionReasonElement.GetProperty("$").GetString();
                                listexceptionReasons.Add(ExceptionReason);
                            }
                            else if (ExceptionReasonElement.ValueKind == JsonValueKind.Array)
                            {
                                // Handle the case where "RelationshipPeriods" is an array
                                foreach (JsonElement AdditionalAddressLineElement1 in ExceptionReasonElement.EnumerateArray())
                                {
                                    ExceptionReason ExceptionReason = new ExceptionReason();
                                    ExceptionReason.ExceptionReasons = AdditionalAddressLineElement1.GetProperty("$").GetString();
                                    listexceptionReasons.Add(ExceptionReason);

                                }
                            }
                        }


                        string insertQuery = @"
                INSERT INTO [dbo].[itleied_ImportTempForeignExceptionDetailsNew] (
                    [itleied_sLEINumber],
                    [itleied_sExceptionCategory],
                    [itleied_sExceptionReason1],
                    [itleied_sExceptionReason2],
                    [itleied_sExceptionReason3],
                    [itleied_sExceptionReason4],
                    [itleied_sExceptionReason5],
                    [itleied_sExceptionReference1],
                    [itleied_sExceptionReference2],
                    [itleied_sExceptionReference3],
                    [itleied_sExceptionReference4],
                    [itleied_sExceptionReference5],
                    [itleied_dCreatedDate],
                    [itleied_nModifiedBy],
                    [itleied_dModifiedDate],
                    [itleied_dDeletedAt]
                )
                VALUES (
                    N'" + exception.LEI + @"',
                    N'" + exception.ExceptionCategory + @"',
                    N'" + (listexceptionReasons.Count > 0 ? listexceptionReasons?[0]?.ExceptionReasons : "") + @"',
                    N'" + (listexceptionReasons.Count >= 2 ? listexceptionReasons?[1]?.ExceptionReasons : "") + @"',
                    N'" + (listexceptionReasons.Count >= 3 ? listexceptionReasons?[2]?.ExceptionReasons : "") + @"',
                    N'" + (listexceptionReasons.Count >= 4 ? listexceptionReasons?[3]?.ExceptionReasons : "") + @"',
                    N'" + (listexceptionReasons.Count >= 5 ? listexceptionReasons?[4]?.ExceptionReasons : "") + @"',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    ''
                )";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                try
                                {
                                    connection.Open();
                                    int rowsAffected = command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Error: {ex.Message}---{i}");
                                }
                            }
                        }
                    }
                    status = "records added successfully";
                }
                catch (Exception ex)
                {
                    status = $"Getting error in json parsing {ex.Message} ---line number {i}";
                    _logger.LogError($"Getting error in json parsing {ex.Message} ---line number {i}");
                }
            }
            else
            {
                _logger.LogInformation("File does not exist.");
                status = "File does not exist.";
            }
            return status;
        }

        public async Task<string> ReadJsonFileAsync()
        {
            _logger.LogInformation("Calling ReadJsonFileAsync");
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStringsSQL").GetSection("ConnectionString").Value;
            string filePath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("Path").Value;
            string filename = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("FileName").Value;
            string path = $"{filePath}\\{filename}";
            string status = "";
            RenameFiles(filePath);
            _logger.LogInformation($"filePath {filePath}");
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                _logger.LogInformation($"successfully collected json records from the file");
                int i = 0;
                try
                {
                    JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);
                    JsonElement rootElement = jsonDocument.RootElement;
                    JsonElement relationsElement = rootElement.GetProperty("relations");
                    // Iterate through each item in the "relations" array
                    foreach (JsonElement relationElement in relationsElement.EnumerateArray())
                    {
                        ReadJsonFile readJsonFile = new ReadJsonFile();
                        JsonElement relationshipRecordElement = relationElement.GetProperty("RelationshipRecord");
                        JsonElement relationshipElement = relationshipRecordElement.GetProperty("Relationship");
                        readJsonFile.RelationshipType = relationshipElement.GetProperty("RelationshipType").GetProperty("$").GetString();
                        JsonElement startNodeElement = relationshipElement.GetProperty("StartNode");
                        readJsonFile.RelationshipStartNodeNodeID = startNodeElement.GetProperty("NodeID").GetProperty("$").GetString();
                        readJsonFile.RelationshipStartNodeNodeIDType = startNodeElement.GetProperty("NodeIDType").GetProperty("$").GetString();

                        JsonElement endNodeElement = relationshipElement.GetProperty("EndNode");
                        readJsonFile.RelationshipEndNodeNodeID = endNodeElement.GetProperty("NodeID").GetProperty("$").GetString();
                        readJsonFile.RelationshipEndNodeNodeIDType = endNodeElement.GetProperty("NodeIDType").GetProperty("$").GetString();

                        // Access the "RelationshipPeriods" property
                        readJsonFile.relationshipPeriods = new List<RelationshipPeriods>();
                        List<RelationshipPeriods> relationshipPeriodslist = new List<RelationshipPeriods>();
                        JsonElement relationshipPeriodsElement = relationshipElement.GetProperty("RelationshipPeriods");
                        JsonElement relationshipPeriodsElement1 = relationshipPeriodsElement.GetProperty("RelationshipPeriod");
                        if (relationshipPeriodsElement1.ValueKind == JsonValueKind.Object)
                        {
                            RelationshipPeriods relationshipPeriods = new RelationshipPeriods();
                            if (relationshipPeriodsElement1.TryGetProperty("StartDate", out JsonElement startDateElement))
                            {
                                relationshipPeriods.RelationshipPeriodStartDate = startDateElement.GetProperty("$").GetString();
                            }
                            if (relationshipPeriodsElement1.TryGetProperty("PeriodType", out JsonElement periodTypeElement))
                            {
                                relationshipPeriods.RelationshipPeriodPeriodType = periodTypeElement.GetProperty("$").GetString();
                            }
                            //relationshipPeriods.RelationshipPeriodStartDate = relationshipPeriodsElement1.GetProperty("StartDate").GetProperty("$").GetString();
                            //relationshipPeriods.RelationshipPeriodPeriodType = relationshipPeriodsElement1.GetProperty("PeriodType").GetProperty("$").GetString();
                            relationshipPeriodslist.Add(relationshipPeriods);
                        }
                        else if (relationshipPeriodsElement1.ValueKind == JsonValueKind.Array)
                        {
                            // Handle the case where "RelationshipPeriods" is an array
                            foreach (JsonElement relationshipPeriodElement in relationshipPeriodsElement1.EnumerateArray())
                            {
                                RelationshipPeriods relationshipPeriods = new RelationshipPeriods();
                                if (relationshipPeriodElement.TryGetProperty("StartDate", out JsonElement startDateElement))
                                {
                                    relationshipPeriods.RelationshipPeriodStartDate = startDateElement.GetProperty("$").GetString();
                                }
                                //relationshipPeriods.RelationshipPeriodStartDate = relationshipPeriodElement.GetProperty("StartDate").GetProperty("$").GetString();
                                if (relationshipPeriodElement.TryGetProperty("EndDate", out JsonElement endDateElement))
                                {
                                    relationshipPeriods.RelationshipPeriodEndDate = endDateElement.GetProperty("$").GetString();
                                }
                                if (relationshipPeriodElement.TryGetProperty("PeriodType", out JsonElement periodTypeElement))
                                {
                                    relationshipPeriods.RelationshipPeriodPeriodType = periodTypeElement.GetProperty("$").GetString();
                                }
                                //relationshipPeriods.RelationshipPeriodPeriodType = relationshipPeriodElement.GetProperty("PeriodType").GetProperty("$").GetString();
                                relationshipPeriodslist.Add(relationshipPeriods);
                            }
                        }
                        readJsonFile.relationshipPeriods = relationshipPeriodslist;

                        readJsonFile.RelationshipStatus = relationshipElement.GetProperty("RelationshipStatus").GetProperty("$").GetString();

                        //Access the "RelationshipQualifiers" property
                        readJsonFile.relationshipQualifiers = new List<RelationshipQualifiers>();
                        List<RelationshipQualifiers> relationshipQualifierslist = new List<RelationshipQualifiers>();
                        if (relationshipElement.TryGetProperty("RelationshipQualifiers", out JsonElement relationshipQualifierElement))
                        {
                            JsonElement RelationshipQualifiersElement = relationshipElement.GetProperty("RelationshipQualifiers");
                            JsonElement RelationshipQualifierElement = RelationshipQualifiersElement.GetProperty("RelationshipQualifier");
                            if (RelationshipQualifierElement.ValueKind == JsonValueKind.Object)
                            {
                                RelationshipQualifiers relationshipQualifiers = new RelationshipQualifiers();
                                if (RelationshipQualifierElement.TryGetProperty("QualifierDimension", out JsonElement qualifierDimensionElement))
                                {
                                    relationshipQualifiers.RelationshipQualifiersQualifierDimension = qualifierDimensionElement.GetProperty("$").GetString();
                                }
                                if (RelationshipQualifierElement.TryGetProperty("QualifierCategory", out JsonElement qualifierCategoryElement))
                                {
                                    relationshipQualifiers.RelationshipQualifiersQualifierCategory = qualifierCategoryElement.GetProperty("$").GetString();
                                }
                                // relationshipQualifiers.RelationshipQualifiersQualifierDimension = RelationshipQualifierElement.GetProperty("QualifierDimension").GetProperty("$").GetString();
                                //relationshipQualifiers.RelationshipQualifiersQualifierCategory = RelationshipQualifierElement.GetProperty("QualifierCategory").GetProperty("$").GetString();
                                relationshipQualifierslist.Add(relationshipQualifiers);
                            }
                            else if (RelationshipQualifierElement.ValueKind == JsonValueKind.Array)
                            {
                                // Handle the case where "RelationshipPeriods" is an array
                                foreach (JsonElement relationshipPeriodElement in RelationshipQualifierElement.EnumerateArray())
                                {
                                    RelationshipQualifiers relationshipQualifiers = new RelationshipQualifiers();
                                    if (relationshipPeriodElement.TryGetProperty("QualifierDimension", out JsonElement qualifierDimensionElement))
                                    {
                                        // Access the property only if it exists
                                        relationshipQualifiers.RelationshipQualifiersQualifierDimension = qualifierDimensionElement.GetProperty("$").GetString();
                                    }
                                    if (relationshipPeriodElement.TryGetProperty("QualifierCategory", out JsonElement qualifierCategoryElement))
                                    {
                                        // Access the property only if it exists
                                        relationshipQualifiers.RelationshipQualifiersQualifierCategory = qualifierCategoryElement.GetProperty("$").GetString();
                                    }
                                    //relationshipQualifiers.RelationshipQualifiersQualifierDimension = relationshipPeriodElement.GetProperty("QualifierDimension").GetProperty("$").GetString();
                                    //relationshipQualifiers.RelationshipQualifiersQualifierCategory = relationshipPeriodElement.GetProperty("QualifierCategory").GetProperty("$").GetString();
                                    relationshipQualifierslist.Add(relationshipQualifiers);
                                }
                            }
                        }
                        readJsonFile.relationshipQualifiers = relationshipQualifierslist;

                        //Access the "RelationshipQualifiers" property
                        readJsonFile.relationshipQuantifiers = new List<RelationshipQuantifiers>();
                        List<RelationshipQuantifiers> relationshipQuantifierslist = new List<RelationshipQuantifiers>();
                        if (relationshipElement.TryGetProperty("RelationshipQuantifiers", out JsonElement RelationshipQuantifiers))
                        {
                            JsonElement RelationshipQuantifiersElement = relationshipElement.GetProperty("RelationshipQuantifiers");
                            JsonElement RelationshipQuantifierElement = RelationshipQuantifiersElement.GetProperty("RelationshipQuantifier");
                            if (RelationshipQuantifierElement.ValueKind == JsonValueKind.Object)
                            {
                                RelationshipQuantifiers Quantifiers = new RelationshipQuantifiers();
                                if (RelationshipQuantifierElement.TryGetProperty("MeasurementMethod", out JsonElement measurementMethodElement))
                                {
                                    Quantifiers.RelationshipQuantifiersMeasurementMethod = measurementMethodElement.GetProperty("$").GetString();
                                }
                                if (RelationshipQuantifierElement.TryGetProperty("QuantifierAmount", out JsonElement quantifierAmountElement))
                                {
                                    Quantifiers.RelationshipQuantifiersQuantifierAmount = quantifierAmountElement.GetProperty("$").GetString();
                                }
                                //Quantifiers.RelationshipQuantifiersMeasurementMethod = RelationshipQuantifierElement.GetProperty("MeasurementMethod").GetProperty("$").GetString();
                                //Quantifiers.RelationshipQuantifiersQuantifierAmount = RelationshipQuantifierElement.GetProperty("QuantifierAmount").GetProperty("$").GetString();
                                relationshipQuantifierslist.Add(Quantifiers);
                            }
                            else if (RelationshipQuantifierElement.ValueKind == JsonValueKind.Array)
                            {
                                // Handle the case where "RelationshipPeriods" is an array
                                foreach (JsonElement relationshipPeriodElement in RelationshipQuantifierElement.EnumerateArray())
                                {
                                    RelationshipQuantifiers Quantifiers = new RelationshipQuantifiers();
                                    if (relationshipPeriodElement.TryGetProperty("MeasurementMethod", out JsonElement measurementMethodElement))
                                    {
                                        Quantifiers.RelationshipQuantifiersMeasurementMethod = measurementMethodElement.GetProperty("$").GetString();
                                    }
                                    if (relationshipPeriodElement.TryGetProperty("QuantifierAmount", out JsonElement quantifierAmountElement))
                                    {
                                        Quantifiers.RelationshipQuantifiersQuantifierAmount = quantifierAmountElement.GetProperty("$").GetString();
                                    }
                                    //Quantifiers.RelationshipQuantifiersMeasurementMethod = relationshipPeriodElement.GetProperty("MeasurementMethod").GetProperty("$").GetString();
                                    //Quantifiers.RelationshipQuantifiersQuantifierAmount = relationshipPeriodElement.GetProperty("QuantifierAmount").GetProperty("$").GetString();
                                    relationshipQuantifierslist.Add(Quantifiers);
                                }
                            }
                        }
                        readJsonFile.relationshipQuantifiers = relationshipQuantifierslist;
                        // Access the "Registration" property
                        JsonElement registrationElement = relationshipRecordElement.GetProperty("Registration");
                        readJsonFile.RegistrationInitialRegistrationDate = registrationElement.GetProperty("InitialRegistrationDate").GetProperty("$").GetString();
                        readJsonFile.RegistrationLastUpdateDate = registrationElement.GetProperty("LastUpdateDate").GetProperty("$").GetString();
                        readJsonFile.RegistrationStatus = registrationElement.GetProperty("RegistrationStatus").GetProperty("$").GetString();
                        readJsonFile.RegistrationNextRenewalDate = registrationElement.GetProperty("NextRenewalDate").GetProperty("$").GetString();
                        readJsonFile.RegistrationManagingLOU = registrationElement.GetProperty("ManagingLOU").GetProperty("$").GetString();
                        readJsonFile.RegistrationValidationSources = registrationElement.GetProperty("ValidationSources").GetProperty("$").GetString();
                        readJsonFile.RegistrationValidationDocuments = registrationElement.GetProperty("ValidationDocuments").GetProperty("$").GetString();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string insertQuery = @"
    INSERT INTO [dbo].[itleird_ImportTempForeignRelationshipDetailsNew]
    (
        [itleird_sRelationshipStartNodeNodeID],
        [itleird_sRelationshipStartNodeNodeIDType],
        [itleird_sRelationshipEndNodeNodeID],
        [itleird_sRelationshipEndNodeNodeIDType],
        [itleird_sRelationshipRelationshipType],
        [itleird_sRelationshipRelationshipStatus],
        [itleird_sRelationshipPeriod1startDate],
        [itleird_sRelationshipPeriod1endDate],
        [itleird_sRelationshipPeriod1periodType],
        [itleird_sRelationshipPeriod2startDate],
        [itleird_sRelationshipPeriod2endDate],
        [itleird_sRelationshipPeriod2periodType],
        [itleird_sRelationshipPeriod3startDate],
        [itleird_sRelationshipPeriod3endDate],
        [itleird_sRelationshipPeriod3periodType],
        [itleird_sRelationshipPeriod4startDate],
        [itleird_sRelationshipPeriod4endDate],
        [itleird_sRelationshipPeriod4periodType],
        [itleird_sRelationshipPeriod5startDate],
        [itleird_sRelationshipPeriod5endDate],
        [itleird_sRelationshipPeriod5periodType],
        [itleird_sRelationshipQualifiers1QualifierDimension],
        [itleird_sRelationshipQualifiers1QualifierCategory],
        [itleird_sRelationshipQualifiers2QualifierDimension],
        [itleird_sRelationshipQualifiers2QualifierCategory],
        [itleird_sRelationshipQualifiers3QualifierDimension],
        [itleird_sRelationshipQualifiers3QualifierCategory],
        [itleird_sRelationshipQualifiers4QualifierDimension],
        [itleird_sRelationshipQualifiers4QualifierCategory],
        [itleird_sRelationshipQualifiers5QualifierDimension],
        [itleird_sRelationshipQualifiers5QualifierCategory],
        [itleird_sRelationshipQuantifiers1MeasurementMethod],
        [itleird_sRelationshipQuantifiers1QuantifierAmount],
        [itleird_sRelationshipQuantifiers1QuantifierUnits],
        [itleird_sRelationshipQuantifiers2MeasurementMethod],
        [itleird_sRelationshipQuantifiers2QuantifierAmount],
        [itleird_sRelationshipQuantifiers2QuantifierUnits],
        [itleird_sRelationshipQuantifiers3MeasurementMethod],
        [itleird_sRelationshipQuantifiers3QuantifierAmount],
        [itleird_sRelationshipQuantifiers3QuantifierUnits],
        [itleird_sRelationshipQuantifiers4MeasurementMethod],
        [itleird_sRelationshipQuantifiers4QuantifierAmount],
        [itleird_sRelationshipQuantifiers4QuantifierUnits],
        [itleird_sRelationshipQuantifiers5MeasurementMethod],
        [itleird_sRelationshipQuantifiers5QuantifierAmount],
        [itleird_sRelationshipQuantifiers5QuantifierUnits],
        [itleird_sRegistrationInitialRegistrationDate],
        [itleird_sRegistrationLastUpdateDate],
        [itleird_sRegistrationRegistrationStatus],
        [itleird_sRegistrationNextRenewalDate],
        [itleird_sRegistrationManagingLOU],
        [itleird_sRegistrationValidationSources],
        [itleird_sRegistrationValidationDocuments],
        [itleird_sRegistrationValidationReference],
        [itleird_sDeletedAt]
    )
    VALUES
    (
        N'" + readJsonFile.RelationshipStartNodeNodeID + @"',
        N'" + readJsonFile.RelationshipStartNodeNodeIDType + @"',
        N'" + readJsonFile.RelationshipEndNodeNodeID + @"',
        N'" + readJsonFile.RelationshipEndNodeNodeIDType + @"',
        N'" + readJsonFile.RelationshipType + @"',
        N'" + readJsonFile.RegistrationStatus + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count > 0 ? readJsonFile.relationshipPeriods?[0]?.RelationshipPeriodStartDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count > 0 ? readJsonFile.relationshipPeriods?[0]?.RelationshipPeriodEndDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count > 0 ? readJsonFile.relationshipPeriods?[0]?.RelationshipPeriodPeriodType : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 2 ? readJsonFile.relationshipPeriods?[1]?.RelationshipPeriodStartDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 2 ? readJsonFile.relationshipPeriods?[1]?.RelationshipPeriodEndDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 2 ? readJsonFile.relationshipPeriods?[1]?.RelationshipPeriodPeriodType : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 3 ? readJsonFile.relationshipPeriods?[2]?.RelationshipPeriodStartDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 3 ? readJsonFile.relationshipPeriods?[2]?.RelationshipPeriodEndDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 3 ? readJsonFile.relationshipPeriods?[2]?.RelationshipPeriodPeriodType : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 4 ? readJsonFile.relationshipPeriods?[3]?.RelationshipPeriodStartDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 4 ? readJsonFile.relationshipPeriods?[3]?.RelationshipPeriodEndDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 4 ? readJsonFile.relationshipPeriods?[3]?.RelationshipPeriodPeriodType : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 5 ? readJsonFile.relationshipPeriods?[4]?.RelationshipPeriodStartDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 5 ? readJsonFile.relationshipPeriods?[4]?.RelationshipPeriodEndDate : "") + @"',
        N'" + (readJsonFile.relationshipPeriods?.Count >= 5 ? readJsonFile.relationshipPeriods?[4]?.RelationshipPeriodPeriodType : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count > 0 ? readJsonFile.relationshipQualifiers?[0]?.RelationshipQualifiersQualifierDimension : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count > 0 ? readJsonFile.relationshipQualifiers?[0]?.RelationshipQualifiersQualifierCategory : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 2 ? readJsonFile.relationshipQualifiers?[1]?.RelationshipQualifiersQualifierDimension : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 2 ? readJsonFile.relationshipQualifiers?[1]?.RelationshipQualifiersQualifierCategory : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 3 ? readJsonFile.relationshipQualifiers?[2]?.RelationshipQualifiersQualifierDimension : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 3 ? readJsonFile.relationshipQualifiers?[2]?.RelationshipQualifiersQualifierCategory : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 4 ? readJsonFile.relationshipQualifiers?[3]?.RelationshipQualifiersQualifierDimension : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 4 ? readJsonFile.relationshipQualifiers?[3]?.RelationshipQualifiersQualifierCategory : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 5 ? readJsonFile.relationshipQualifiers?[4]?.RelationshipQualifiersQualifierDimension : "") + @"',
        N'" + (readJsonFile.relationshipQualifiers?.Count >= 5 ? readJsonFile.relationshipQualifiers?[4]?.RelationshipQualifiersQualifierCategory : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count > 0 ? readJsonFile.relationshipQuantifiers?[0]?.RelationshipQuantifiersMeasurementMethod : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count > 0 ? readJsonFile.relationshipQuantifiers?[0]?.RelationshipQuantifiersQuantifierAmount : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count > 0 ? readJsonFile.relationshipQuantifiers?[0]?.RelationshipQuantifiersQuantifierUnits : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 2 ? readJsonFile.relationshipQuantifiers?[1]?.RelationshipQuantifiersMeasurementMethod : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 2 ? readJsonFile.relationshipQuantifiers?[1]?.RelationshipQuantifiersQuantifierAmount : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 2 ? readJsonFile.relationshipQuantifiers?[1]?.RelationshipQuantifiersQuantifierUnits : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 3 ? readJsonFile.relationshipQuantifiers?[2]?.RelationshipQuantifiersMeasurementMethod : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 3 ? readJsonFile.relationshipQuantifiers?[2]?.RelationshipQuantifiersQuantifierAmount : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 3 ? readJsonFile.relationshipQuantifiers?[2]?.RelationshipQuantifiersQuantifierUnits : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 4 ? readJsonFile.relationshipQuantifiers?[3]?.RelationshipQuantifiersMeasurementMethod : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 4 ? readJsonFile.relationshipQuantifiers?[3]?.RelationshipQuantifiersQuantifierAmount : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 4 ? readJsonFile.relationshipQuantifiers?[3]?.RelationshipQuantifiersQuantifierUnits : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 5 ? readJsonFile.relationshipQuantifiers?[4]?.RelationshipQuantifiersMeasurementMethod : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 5 ? readJsonFile.relationshipQuantifiers?[4]?.RelationshipQuantifiersQuantifierAmount : "") + @"',
        N'" + (readJsonFile.relationshipQuantifiers?.Count >= 5 ? readJsonFile.relationshipQuantifiers?[4]?.RelationshipQuantifiersQuantifierUnits : "") + @"',
        N'" + readJsonFile.RegistrationInitialRegistrationDate + @"',
        N'" + readJsonFile.RegistrationLastUpdateDate + @"',
        N'" + readJsonFile.RegistrationStatus + @"',
        N'" + readJsonFile.RegistrationNextRenewalDate + @"',
        N'" + readJsonFile.RegistrationManagingLOU + @"',
        N'" + readJsonFile.RegistrationValidationSources + @"',
        N'" + readJsonFile.RegistrationValidationDocuments + @"',
        N'" + readJsonFile.RegistrationValidationReference + @"',
        N'" + readJsonFile.DeletedAt + @"'
    )";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                try
                                {
                                    connection.Open();
                                    int rowsAffected = command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Error: {ex.Message}---{i}");
                                }
                            }
                        }
                    }
                    status = "Records added Successfully";
                    _logger.LogError("Records added Successfully");
                }
                catch (Exception ex)
                {
                    status = $"Error parsing JSON: {ex.Message}---{i}";
                    _logger.LogError($"Error parsing JSON: {ex.Message}---{i}");
                }
            }
            else
            {
                status = "File does not exist.";
                _logger.LogInformation("File does not exist.");
            }
            return status;
        }

        public async Task<string> ReadLEIJsonFileAsync()
        {
            _logger.LogInformation("Calling ReadLEIJsonFileAsync");
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStringsSQL").GetSection("ConnectionString").Value;
            string filePath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("LEIPath").Value;
            string filename = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilesPath").GetSection("LEIFileName").Value;
            string path = $"{filePath}\\{filename}";
            RenameFiles(filePath);
            string status = "";
            _logger.LogInformation($"filePath {filePath}");
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                _logger.LogInformation($"successfully collected json records from the file");
                ReadLEIJsonFile list = new ReadLEIJsonFile();
                int i = 0;
                try
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(jsonContent))
                    {
                        JsonElement rootElement = jsonDocument.RootElement;
                        JsonElement recordsElement = rootElement.GetProperty("records");

                        foreach (JsonElement recordElement in recordsElement.EnumerateArray())
                        {
                            i++;
                            LegalEntity legalEntity = new LegalEntity();
                            legalEntity.LEI = recordElement.GetProperty("LEI").GetProperty("$").GetString();
                            // Access LEI and Entity information
                            JsonElement entityElement = recordElement.GetProperty("Entity");
                            JsonElement RegistrationElement = recordElement.GetProperty("Registration");
                            EntityDetails entityDetails = new EntityDetails();
                            entityDetails.LegalName = entityElement.GetProperty("LegalName").GetProperty("$").GetString();

                            JsonElement legalAddressElement = entityElement.GetProperty("LegalAddress");
                            Address address = new Address();
                            if (entityElement.TryGetProperty("LegalAddress", out JsonElement xmllang))
                            {
                                address.LegalNamexmllang = xmllang.GetProperty("@xml:lang").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("AddressNumber", out JsonElement AddressNumber))
                            {
                                address.FirstAddressNumber = AddressNumber.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("AddressNumberWithinBuilding", out JsonElement AddressNumberWithinBuilding))
                            {
                                address.AddressNumberWithinBuilding = AddressNumberWithinBuilding.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("FirstAddressLine", out JsonElement FirstAddressLine))
                            {
                                address.FirstAddressLine = FirstAddressLine.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("MailRouting", out JsonElement MailRouting))
                            {
                                address.MailRouting = MailRouting.GetProperty("$").GetString();
                            }
                            JsonElement AdditionalAddressLineElement;

                            if (legalAddressElement.TryGetProperty("AdditionalAddressLine", out AdditionalAddressLineElement))
                            {
                                if (AdditionalAddressLineElement.ValueKind == JsonValueKind.Object)
                                {
                                    address.AdditionalAddressLine = AdditionalAddressLineElement.GetProperty("$").GetString();
                                }
                                else if (AdditionalAddressLineElement.ValueKind == JsonValueKind.Array)
                                {
                                    // Handle the case where "RelationshipPeriods" is an array
                                    foreach (JsonElement AdditionalAddressLineElement1 in AdditionalAddressLineElement.EnumerateArray())
                                    {
                                        address.AdditionalAddressLine = AdditionalAddressLineElement1.GetProperty("$").GetString();
                                    }
                                }
                            }


                            if (legalAddressElement.TryGetProperty("City", out JsonElement CityElement))
                            {
                                address.City = CityElement.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("Region", out JsonElement regionElement))
                            {
                                address.Region = regionElement.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("Country", out JsonElement CountryElement))
                            {
                                address.Country = CountryElement.GetProperty("$").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("PostalCode", out JsonElement PostalCodeElement))
                            {
                                address.PostalCode = PostalCodeElement.GetProperty("$").GetString();
                            }

                            Address HQaddress = new Address();
                            JsonElement HeadquartersAddressElement = entityElement.GetProperty("HeadquartersAddress");
                            if (entityElement.TryGetProperty("HeadquartersAddress", out JsonElement HeadquartersAddress))
                            {
                                HQaddress.LegalAddressxmllang = HeadquartersAddress.GetProperty("@xml:lang").GetString();
                            }
                            if (legalAddressElement.TryGetProperty("AdditionalAddressLine", out JsonElement HQAdditionalAddressLineElement))
                            {
                                if (HQAdditionalAddressLineElement.ValueKind == JsonValueKind.Object)
                                {
                                    HQaddress.AdditionalAddressLine = HQAdditionalAddressLineElement.GetProperty("$").GetString();
                                }
                                else if (HQAdditionalAddressLineElement.ValueKind == JsonValueKind.Array)
                                {
                                    // Handle the case where "RelationshipPeriods" is an array
                                    foreach (JsonElement AdditionalAddressLineElement1 in HQAdditionalAddressLineElement.EnumerateArray())
                                    {
                                        HQaddress.AdditionalAddressLine = AdditionalAddressLineElement1.GetProperty("$").GetString();
                                    }
                                }
                            }
                            if (HeadquartersAddressElement.TryGetProperty("FirstAddressLine", out JsonElement HQFirstAddressLine1))
                            {
                                HQaddress.FirstAddressLine = HQFirstAddressLine1.GetProperty("$").GetString();
                            }
                            if (HeadquartersAddressElement.TryGetProperty("City", out JsonElement HQCityElement))
                            {
                                HQaddress.City = HQCityElement.GetProperty("$").GetString();
                            }
                            if (HeadquartersAddressElement.TryGetProperty("Region", out JsonElement HQRegionElement))
                            {
                                HQaddress.Region = HQRegionElement.GetProperty("$").GetString();
                            }
                            if (HeadquartersAddressElement.TryGetProperty("Country", out JsonElement HQCountryElement))
                            {
                                HQaddress.Country = HQCountryElement.GetProperty("$").GetString();
                            }
                            if (HeadquartersAddressElement.TryGetProperty("PostalCode", out JsonElement HQPostalCodeElement))
                            {
                                HQaddress.PostalCode = HQPostalCodeElement.GetProperty("$").GetString();
                            }
                            if (HeadquartersAddressElement.TryGetProperty("MailRouting", out JsonElement HQMailRouting))
                            {
                                HQaddress.MailRouting = HQMailRouting.GetProperty("$").GetString();
                            }
                            RegistrationAuthority registrationAuthority = new RegistrationAuthority();
                            if (entityElement.TryGetProperty("RegistrationAuthority", out JsonElement RegistrationAuthorityElement))
                            {
                                if (RegistrationAuthorityElement.TryGetProperty("RegistrationAuthorityID", out JsonElement RegistrationAuthorityID))
                                {
                                    registrationAuthority.RegistrationAuthorityID = RegistrationAuthorityID.GetProperty("$").GetString();
                                }
                                if (RegistrationAuthorityElement.TryGetProperty("RegistrationAuthorityEntityID", out JsonElement RegistrationAuthorityEntityID))
                                {
                                    registrationAuthority.RegistrationAuthorityEntityID = RegistrationAuthorityEntityID.GetProperty("$").GetString();
                                }
                            }

                            if (entityElement.TryGetProperty("LegalJurisdiction", out JsonElement LegalJurisdiction))
                            {
                                entityDetails.LegalJurisdiction = LegalJurisdiction.GetProperty("$").GetString();
                            }
                            if (entityElement.TryGetProperty("EntityCategory", out JsonElement EntityCategory))
                            {
                                entityDetails.EntityCategory = EntityCategory.GetProperty("$").GetString();
                            }

                            List<OtherEntityNames> ListotherEntityNames = new List<OtherEntityNames>();
                            if (entityElement.TryGetProperty("OtherEntityNames", out JsonElement OtherEntityNamesElement))
                            {
                                if (OtherEntityNamesElement.TryGetProperty("OtherEntityName", out JsonElement OtherEntityNameElement))
                                {
                                    OtherEntityNames otherEntityNames = new OtherEntityNames();
                                    if (OtherEntityNameElement.ValueKind == JsonValueKind.Object)
                                    {
                                        otherEntityNames.OtherEntityNamexmllang = OtherEntityNameElement.GetProperty("@xml:lang").GetString();
                                        otherEntityNames.OtherEntityNamexmllang = OtherEntityNameElement.GetProperty("@type").GetString();
                                        otherEntityNames.OtherEntityNamexmllang = OtherEntityNameElement.GetProperty("$").GetString();
                                        ListotherEntityNames.Add(otherEntityNames);
                                    }
                                    else if (OtherEntityNameElement.ValueKind == JsonValueKind.Array)
                                    {
                                        // Handle the case where "RelationshipPeriods" is an array
                                        foreach (JsonElement OtherEntity in OtherEntityNameElement.EnumerateArray())
                                        {
                                            OtherEntityNames otherEntity = new OtherEntityNames();
                                            otherEntity.OtherEntityNamexmllang = OtherEntity.GetProperty("@xml:lang").GetString();
                                            otherEntity.type = OtherEntity.GetProperty("@type").GetString();
                                            otherEntity.OtherEntityName = OtherEntity.GetProperty("$").GetString();
                                            ListotherEntityNames.Add(otherEntity);
                                        }
                                    }
                                }
                            }

                            List<TransliteratedOtherEntityNames> ListTransliteratedOtherEntityNames = new List<TransliteratedOtherEntityNames>();
                            if (entityElement.TryGetProperty("TransliteratedOtherEntityNames", out JsonElement TransliteratedOtherEntityNamesElement))
                            {
                                if (TransliteratedOtherEntityNamesElement.TryGetProperty("TransliteratedOtherEntityName", out JsonElement TransliteratedOtherEntityNameElement))
                                {
                                    TransliteratedOtherEntityNames otherEntityNames = new TransliteratedOtherEntityNames();
                                    if (TransliteratedOtherEntityNameElement.ValueKind == JsonValueKind.Object)
                                    {
                                        otherEntityNames.TranOtherEntityNamexmllang = TransliteratedOtherEntityNameElement.GetProperty("@xml:lang").GetString();
                                        otherEntityNames.TranOtherEntitytype = TransliteratedOtherEntityNameElement.GetProperty("@type").GetString();
                                        otherEntityNames.TranOtherEntityName = TransliteratedOtherEntityNameElement.GetProperty("$").GetString();
                                        ListTransliteratedOtherEntityNames.Add(otherEntityNames);
                                    }
                                    else if (TransliteratedOtherEntityNameElement.ValueKind == JsonValueKind.Array)
                                    {
                                        // Handle the case where "RelationshipPeriods" is an array
                                        foreach (JsonElement OtherEntity in TransliteratedOtherEntityNameElement.EnumerateArray())
                                        {
                                            TransliteratedOtherEntityNames TranotherEntity = new TransliteratedOtherEntityNames();
                                            TranotherEntity.TranOtherEntityNamexmllang = OtherEntity.GetProperty("@xml:lang").GetString();
                                            TranotherEntity.TranOtherEntitytype = OtherEntity.GetProperty("@type").GetString();
                                            TranotherEntity.TranOtherEntityName = OtherEntity.GetProperty("$").GetString();
                                            ListTransliteratedOtherEntityNames.Add(TranotherEntity);
                                        }
                                    }
                                }
                            }

                            List<OtherAddresses> ListOtherAddresses = new List<OtherAddresses>();
                            if (entityElement.TryGetProperty("OtherAddresses", out JsonElement OtherAddressesElement))
                            {
                                if (OtherAddressesElement.TryGetProperty("OtherAddress", out JsonElement OtherAddressElement))
                                {
                                    OtherAddresses OtherAddresses = new OtherAddresses();
                                    if (OtherAddressElement.ValueKind == JsonValueKind.Object)
                                    {
                                        OtherAddresses.xmllang = OtherAddressElement.GetProperty("@xml:lang").GetString();
                                        if (OtherAddressElement.TryGetProperty("@xml:lang", out JsonElement xmllang1))
                                        {
                                            OtherAddresses.xmllang = xmllang1.GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("@type", out JsonElement type))
                                        {
                                            OtherAddresses.type = type.GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AddressNumber", out JsonElement AddressNumber1))
                                        {
                                            OtherAddresses.AddressNumber = AddressNumber1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("FirstAddressLine", out JsonElement FirstAddressLine1))
                                        {
                                            OtherAddresses.FirstAddressLine = FirstAddressLine1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AddressNumberWithinBuilding", out JsonElement AddressNumberWithinBuilding1))
                                        {
                                            OtherAddresses.AddressNumberWithinBuilding = AddressNumberWithinBuilding1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("MailRouting", out JsonElement MailRouting1))
                                        {
                                            OtherAddresses.MailRouting = MailRouting1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("City", out JsonElement City1))
                                        {
                                            OtherAddresses.City = City1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("Region", out JsonElement Region1))
                                        {
                                            OtherAddresses.Region = Region1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("Country", out JsonElement Country1))
                                        {
                                            OtherAddresses.Country = Country1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("PostalCode", out JsonElement PostalCode1))
                                        {
                                            OtherAddresses.PostalCode = PostalCode1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AdditionalAddressLine", out JsonElement AdditionalAddressLine))
                                        {
                                            OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                            if (AdditionalAddressLine.ValueKind == JsonValueKind.Object)
                                            {
                                                OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherAddressElement.GetProperty("$").GetString();
                                            }
                                            else if (AdditionalAddressLine.ValueKind == JsonValueKind.Array)
                                            {
                                                // Handle the case where "RelationshipPeriods" is an array
                                                foreach (JsonElement OtherEntity in AdditionalAddressLine.EnumerateArray())
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity.GetProperty("$").GetString();
                                                }
                                            }
                                        }
                                        ListOtherAddresses.Add(OtherAddresses);
                                    }
                                    else if (OtherAddressElement.ValueKind == JsonValueKind.Array)
                                    {
                                        // Handle the case where "RelationshipPeriods" is an array
                                        foreach (JsonElement OtherEntity in OtherAddressElement.EnumerateArray())
                                        {
                                            if (OtherEntity.TryGetProperty("@xml:lang", out JsonElement xmllang1))
                                            {
                                                OtherAddresses.xmllang = xmllang1.GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("@type", out JsonElement type))
                                            {
                                                OtherAddresses.type = type.GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("AddressNumber", out JsonElement AddressNumber1))
                                            {
                                                OtherAddresses.AddressNumber = AddressNumber1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("FirstAddressLine", out JsonElement FirstAddressLine1))
                                            {
                                                OtherAddresses.FirstAddressLine = FirstAddressLine1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("AddressNumberWithinBuilding", out JsonElement AddressNumberWithinBuilding1))
                                            {
                                                OtherAddresses.AddressNumberWithinBuilding = AddressNumberWithinBuilding1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("MailRouting", out JsonElement MailRouting1))
                                            {
                                                OtherAddresses.MailRouting = MailRouting1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("City", out JsonElement City1))
                                            {
                                                OtherAddresses.City = City1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("Region", out JsonElement Region1))
                                            {
                                                OtherAddresses.Region = Region1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("Country", out JsonElement Country1))
                                            {
                                                OtherAddresses.Country = Country1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("PostalCode", out JsonElement PostalCode1))
                                            {
                                                OtherAddresses.PostalCode = PostalCode1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.ValueKind == JsonValueKind.Object && OtherEntity.TryGetProperty("AdditionalAddressLine", out JsonElement AdditionalAddressLine))
                                            {
                                                OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                                if (AdditionalAddressLine.ValueKind == JsonValueKind.Object)
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = AdditionalAddressLine.GetProperty("$").GetString();
                                                }
                                                else if (AdditionalAddressLine.ValueKind == JsonValueKind.Array)
                                                {
                                                    // Handle the case where "RelationshipPeriods" is an array
                                                    foreach (JsonElement OtherEntity1 in AdditionalAddressLine.EnumerateArray())
                                                    {
                                                        OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity1.GetProperty("$").GetString();
                                                        ListOtherAddresses.Add(OtherAddresses);

                                                    }
                                                }
                                            }
                                            else if (OtherEntity.ValueKind == JsonValueKind.Array)
                                            {
                                                foreach (JsonElement addressElement in OtherEntity.EnumerateArray())
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                                    if (addressElement.ValueKind == JsonValueKind.Object)
                                                    {
                                                        OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = addressElement.GetProperty("$").GetString();
                                                        ListOtherAddresses.Add(OtherAddresses);
                                                    }
                                                    else if (addressElement.ValueKind == JsonValueKind.Array)
                                                    {
                                                        // Handle the case where "RelationshipPeriods" is an array
                                                        foreach (JsonElement OtherEntity1 in addressElement.EnumerateArray())
                                                        {
                                                            OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity1.GetProperty("$").GetString();
                                                            ListOtherAddresses.Add(OtherAddresses);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            List<TransliteratedOtherAddresses> ListTransliteratedOtherAddresses = new List<TransliteratedOtherAddresses>();
                            if (entityElement.TryGetProperty("TransliteratedOtherAddresses", out JsonElement TransliteratedOtherAddressesElement))
                            {
                                if (TransliteratedOtherAddressesElement.TryGetProperty("TransliteratedOtherAddress", out JsonElement OtherAddressElement))
                                {
                                    TransliteratedOtherAddresses OtherAddresses = new TransliteratedOtherAddresses();
                                    if (OtherAddressElement.ValueKind == JsonValueKind.Object)
                                    {
                                        OtherAddresses.xmllang = OtherAddressElement.GetProperty("@xml:lang").GetString();
                                        if (OtherAddressElement.TryGetProperty("@xml:lang", out JsonElement xmllang1))
                                        {
                                            OtherAddresses.xmllang = xmllang1.GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("@type", out JsonElement type))
                                        {
                                            OtherAddresses.type = type.GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AddressNumber", out JsonElement AddressNumber1))
                                        {
                                            OtherAddresses.AddressNumber = AddressNumber1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("FirstAddressLine", out JsonElement FirstAddressLine1))
                                        {
                                            OtherAddresses.FirstAddressLine = FirstAddressLine1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AddressNumberWithinBuilding", out JsonElement AddressNumberWithinBuilding1))
                                        {
                                            OtherAddresses.AddressNumberWithinBuilding = AddressNumberWithinBuilding1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("MailRouting", out JsonElement MailRouting1))
                                        {
                                            OtherAddresses.MailRouting = MailRouting1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("City", out JsonElement City1))
                                        {
                                            OtherAddresses.City = City1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("Region", out JsonElement Region1))
                                        {
                                            OtherAddresses.Region = Region1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("Country", out JsonElement Country1))
                                        {
                                            OtherAddresses.Country = Country1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("PostalCode", out JsonElement PostalCode1))
                                        {
                                            OtherAddresses.PostalCode = PostalCode1.GetProperty("$").GetString();
                                        }
                                        if (OtherAddressElement.TryGetProperty("AdditionalAddressLine", out JsonElement AdditionalAddressLine))
                                        {
                                            OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                            if (AdditionalAddressLine.ValueKind == JsonValueKind.Object)
                                            {
                                                OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherAddressElement.GetProperty("$").GetString();
                                            }
                                            else if (AdditionalAddressLine.ValueKind == JsonValueKind.Array)
                                            {
                                                // Handle the case where "RelationshipPeriods" is an array
                                                foreach (JsonElement OtherEntity in AdditionalAddressLine.EnumerateArray())
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity.GetProperty("$").GetString();
                                                }
                                            }
                                        }
                                        ListTransliteratedOtherAddresses.Add(OtherAddresses);
                                    }
                                    else if (OtherAddressElement.ValueKind == JsonValueKind.Array)
                                    {
                                        // Handle the case where "RelationshipPeriods" is an array
                                        foreach (JsonElement OtherEntity in OtherAddressElement.EnumerateArray())
                                        {
                                            if (OtherEntity.TryGetProperty("@xml:lang", out JsonElement xmllang1))
                                            {
                                                OtherAddresses.xmllang = xmllang1.GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("@type", out JsonElement type))
                                            {
                                                OtherAddresses.type = type.GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("AddressNumber", out JsonElement AddressNumber1))
                                            {
                                                OtherAddresses.AddressNumber = AddressNumber1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("FirstAddressLine", out JsonElement FirstAddressLine1))
                                            {
                                                OtherAddresses.FirstAddressLine = FirstAddressLine1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("AddressNumberWithinBuilding", out JsonElement AddressNumberWithinBuilding1))
                                            {
                                                OtherAddresses.AddressNumberWithinBuilding = AddressNumberWithinBuilding1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("MailRouting", out JsonElement MailRouting1))
                                            {
                                                OtherAddresses.MailRouting = MailRouting1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("City", out JsonElement City1))
                                            {
                                                OtherAddresses.City = City1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("Region", out JsonElement Region1))
                                            {
                                                OtherAddresses.Region = Region1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("Country", out JsonElement Country1))
                                            {
                                                OtherAddresses.Country = Country1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.TryGetProperty("PostalCode", out JsonElement PostalCode1))
                                            {
                                                OtherAddresses.PostalCode = PostalCode1.GetProperty("$").GetString();
                                            }
                                            if (OtherEntity.ValueKind == JsonValueKind.Object && OtherEntity.TryGetProperty("AdditionalAddressLine", out JsonElement AdditionalAddressLine))
                                            {
                                                OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                                if (AdditionalAddressLine.ValueKind == JsonValueKind.Object)
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = AdditionalAddressLine.GetProperty("$").GetString();
                                                }
                                                else if (AdditionalAddressLine.ValueKind == JsonValueKind.Array)
                                                {
                                                    // Handle the case where "RelationshipPeriods" is an array
                                                    foreach (JsonElement OtherEntity1 in AdditionalAddressLine.EnumerateArray())
                                                    {
                                                        OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity1.GetProperty("$").GetString();
                                                        ListTransliteratedOtherAddresses.Add(OtherAddresses);

                                                    }
                                                }
                                            }
                                            else if (OtherEntity.ValueKind == JsonValueKind.Array)
                                            {
                                                foreach (JsonElement addressElement in OtherEntity.EnumerateArray())
                                                {
                                                    OtherAddresses.OtherAdditionalAddresses = new OtherAdditionalAddresses();

                                                    if (addressElement.ValueKind == JsonValueKind.Object)
                                                    {
                                                        OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = addressElement.GetProperty("$").GetString();
                                                        ListTransliteratedOtherAddresses.Add(OtherAddresses);
                                                    }
                                                    else if (addressElement.ValueKind == JsonValueKind.Array)
                                                    {
                                                        // Handle the case where "RelationshipPeriods" is an array
                                                        foreach (JsonElement OtherEntity1 in addressElement.EnumerateArray())
                                                        {
                                                            OtherAddresses.OtherAdditionalAddresses.AdditionalAddressLine = OtherEntity1.GetProperty("$").GetString();
                                                            ListTransliteratedOtherAddresses.Add(OtherAddresses);
                                                        }
                                                    }
                                                }
                                            }
                                            ListTransliteratedOtherAddresses.Add(OtherAddresses);
                                        }
                                    }
                                }
                            }

                            LegalForm legalForm = new LegalForm();
                            if (entityElement.TryGetProperty("LegalForm", out JsonElement LegalFormElement))
                            {
                                if (LegalFormElement.ValueKind == JsonValueKind.Object)
                                {
                                    if (LegalFormElement.TryGetProperty("EntityLegalFormCode", out JsonElement EntityLegalFormCode))
                                    {
                                        legalForm.EntityLegalFormCode = EntityLegalFormCode.GetProperty("$").GetString();
                                    }
                                    if (LegalFormElement.TryGetProperty("OtherLegalForm", out JsonElement OtherLegalForm))
                                    {
                                        legalForm.OtherLegalForm = OtherLegalForm.GetProperty("$").GetString();
                                    }
                                }
                                else if (LegalFormElement.ValueKind == JsonValueKind.Array)
                                {
                                    // Handle the case where "RelationshipPeriods" is an array
                                    foreach (JsonElement LegalFormElement1 in LegalFormElement.EnumerateArray())
                                    {
                                        if (LegalFormElement.TryGetProperty("EntityLegalFormCode", out JsonElement EntityLegalFormCode))
                                        {
                                            legalForm.EntityLegalFormCode = EntityLegalFormCode.GetProperty("$").GetString();
                                        }
                                        if (LegalFormElement.TryGetProperty("OtherLegalForm", out JsonElement OtherLegalForm))
                                        {
                                            legalForm.OtherLegalForm = OtherLegalForm.GetProperty("$").GetString();
                                        }
                                    }
                                }
                            }

                            if (entityElement.TryGetProperty("EntityStatus", out JsonElement EntityStatus))
                            {
                                entityDetails.EntityStatus = EntityStatus.GetProperty("$").GetString();
                            }
                            if (entityElement.TryGetProperty("EntityCreationDate", out JsonElement EntityCreationDate))
                            {
                                entityDetails.EntityCreationDate = EntityCreationDate.GetProperty("$").GetString();
                            }

                            RegistrationDetails registrationDetails = new RegistrationDetails();
                            if (RegistrationElement.TryGetProperty("RegistrationAuthorityID", out JsonElement RegistrationAuthorityID1))
                            {
                                registrationDetails.RegistrationAuthorityID = RegistrationAuthorityID1.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("RegistrationAuthorityEntityID", out JsonElement RegistrationAuthorityEntityID1))
                            {
                                registrationDetails.RegistrationAuthorityEntityID = RegistrationAuthorityEntityID1.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("OtherRegistrationAuthorityID", out JsonElement OtherRegistrationAuthorityID1))
                            {
                                registrationDetails.OtherRegistrationAuthorityID = OtherRegistrationAuthorityID1.GetProperty("$").GetString();
                            }

                            if (RegistrationElement.TryGetProperty("InitialRegistrationDate", out JsonElement InitialRegistrationDate))
                            {
                                registrationDetails.InitialRegistrationDate = InitialRegistrationDate.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("LastUpdateDate", out JsonElement LastUpdateDate))
                            {
                                registrationDetails.LastUpdateDate = LastUpdateDate.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("RegistrationStatus", out JsonElement RegistrationStatus))
                            {
                                registrationDetails.RegistrationStatus = RegistrationStatus.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("NextRenewalDate", out JsonElement NextRenewalDate))
                            {
                                registrationDetails.NextRenewalDate = NextRenewalDate.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("ManagingLOU", out JsonElement ManagingLOU))
                            {
                                registrationDetails.ManagingLOU = ManagingLOU.GetProperty("$").GetString();
                            }
                            if (RegistrationElement.TryGetProperty("ValidationSources", out JsonElement ValidationSources))
                            {
                                registrationDetails.ValidationSources = ValidationSources.GetProperty("$").GetString();
                            }
                            List<ValidationAuthority> listValidationAuthority = new List<ValidationAuthority>();
                            if (RegistrationElement.TryGetProperty("ValidationAuthority", out JsonElement ValidationAuthorityElement))
                            {
                                if (ValidationAuthorityElement.ValueKind == JsonValueKind.Object)
                                {
                                    ValidationAuthority validationAuthority = new ValidationAuthority();

                                    if (ValidationAuthorityElement.TryGetProperty("ValidationAuthorityID", out JsonElement ValidationAuthorityID))
                                    {
                                        validationAuthority.ValidationAuthorityID = ValidationAuthorityID.GetProperty("$").GetString();
                                    }
                                    if (ValidationAuthorityElement.TryGetProperty("ValidationAuthorityEntityID", out JsonElement ValidationAuthorityEntityID))
                                    {
                                        validationAuthority.ValidationAuthorityEntityID = ValidationAuthorityEntityID.GetProperty("$").GetString();
                                    }
                                    listValidationAuthority.Add(validationAuthority);
                                }
                                else if (ValidationAuthorityElement.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (JsonElement ValidationAuthorityElement1 in ValidationAuthorityElement.EnumerateArray())
                                    {
                                        ValidationAuthority validationAuthority = new ValidationAuthority();

                                        if (ValidationAuthorityElement1.TryGetProperty("ValidationAuthorityID", out JsonElement ValidationAuthorityID))
                                        {
                                            validationAuthority.ValidationAuthorityID = ValidationAuthorityID.GetProperty("$").GetString();
                                        }
                                        if (ValidationAuthorityElement1.TryGetProperty("ValidationAuthorityEntityID", out JsonElement ValidationAuthorityEntityID))
                                        {
                                            validationAuthority.ValidationAuthorityEntityID = ValidationAuthorityEntityID.GetProperty("$").GetString();
                                        }
                                        if (ValidationAuthorityElement1.TryGetProperty("OtherValidationAuthorityID", out JsonElement OtherValidationAuthorityID))
                                        {
                                            validationAuthority.OtherValidationAuthorityID = OtherValidationAuthorityID.GetProperty("$").GetString();
                                        }
                                        listValidationAuthority.Add(validationAuthority);
                                    }
                                }
                            }

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                string insertQuery = @"
    INSERT INTO [dbo].[gleif-goldencopy-lei2-last-week]
    (
[LEI],
[Entity LegalName],
[Entity LegalName xmllang],
[Entity OtherEntityNames OtherEntityName 1],
[Entity OtherEntityNames OtherEntityName 1 xmllang],
[Entity OtherEntityNames OtherEntityName 1 type],
[Entity OtherEntityNames OtherEntityName 2],
[Entity OtherEntityNames OtherEntityName 2 xmllang],
[Entity OtherEntityNames OtherEntityName 2 type],
[Entity OtherEntityNames OtherEntityName 3],
[Entity OtherEntityNames OtherEntityName 3 xmllang],
[Entity OtherEntityNames OtherEntityName 3 type],
[Entity OtherEntityNames OtherEntityName 4],
[Entity OtherEntityNames OtherEntityName 4 xmllang],
[Entity OtherEntityNames OtherEntityName 4 type],
[Entity OtherEntityNames OtherEntityName 5],
[Entity OtherEntityNames OtherEntityName 5 xmllang],
[Entity OtherEntityNames OtherEntityName 5 type],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 1],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 1 xmllang],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 1 type],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 2],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 2 xmllang],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 2 type],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 3],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 3 xmllang],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 3 type],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 4],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 4 xmllang],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 4 type],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 5],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 5 xmllang],
[Entity TransliteratedOtherEntityNames TransliteratedOtherEntityName 5 type],
[Entity LegalAddress xmllang],
[Entity LegalAddress FirstAddressLine],
[Entity LegalAddress AddressNumber],
[Entity LegalAddress AddressNumberWithinBuilding],
[Entity LegalAddress MailRouting],
[Entity LegalAddress AdditionalAddressLine 1],
[Entity LegalAddress AdditionalAddressLine 2],
[Entity LegalAddress AdditionalAddressLine 3],
[Entity LegalAddress City],
[Entity LegalAddress Region],
[Entity LegalAddress Country],
[Entity LegalAddress PostalCode],
[Entity HeadquartersAddress xmllang],
[Entity HeadquartersAddress FirstAddressLine],
[Entity HeadquartersAddress AddressNumber],
[Entity HeadquartersAddress AddressNumberWithinBuilding],
[Entity HeadquartersAddress MailRouting],
[Entity HeadquartersAddress AdditionalAddressLine 1],
[Entity HeadquartersAddress AdditionalAddressLine 2],
[Entity HeadquartersAddress AdditionalAddressLine 3],
[Entity HeadquartersAddress City],
[Entity HeadquartersAddress Region],
[Entity HeadquartersAddress Country],
[Entity HeadquartersAddress PostalCode],
[Entity OtherAddresses OtherAddress 1 xmllang],
[Entity OtherAddresses OtherAddress 1 type],
[Entity OtherAddresses OtherAddress 1 FirstAddressLine],
[Entity OtherAddresses OtherAddress 1 AddressNumber],
[Entity OtherAddresses OtherAddress 1 AddressNumberWithinBuilding],
[Entity OtherAddresses OtherAddress 1 MailRouting],
[Entity OtherAddresses OtherAddress 1 AdditionalAddressLine 1],
[Entity OtherAddresses OtherAddress 1 AdditionalAddressLine 2],
[Entity OtherAddresses OtherAddress 1 AdditionalAddressLine 3],
[Entity OtherAddresses OtherAddress 1 City],
[Entity OtherAddresses OtherAddress 1 Region],
[Entity OtherAddresses OtherAddress 1 Country],
[Entity OtherAddresses OtherAddress 1 PostalCode],
[Entity OtherAddresses OtherAddress 2 xmllang],
[Entity OtherAddresses OtherAddress 2 type],
[Entity OtherAddresses OtherAddress 2 FirstAddressLine],
[Entity OtherAddresses OtherAddress 2 AddressNumber],
[Entity OtherAddresses OtherAddress 2 AddressNumberWithinBuilding],
[Entity OtherAddresses OtherAddress 2 MailRouting],
[Entity OtherAddresses OtherAddress 2 AdditionalAddressLine 1],
[Entity OtherAddresses OtherAddress 2 AdditionalAddressLine 2],
[Entity OtherAddresses OtherAddress 2 AdditionalAddressLine 3],
[Entity OtherAddresses OtherAddress 2 City],
[Entity OtherAddresses OtherAddress 2 Region],
[Entity OtherAddresses OtherAddress 2 Country],
[Entity OtherAddresses OtherAddress 2 PostalCode],
[Entity OtherAddresses OtherAddress 3 xmllang],
[Entity OtherAddresses OtherAddress 3 type],
[Entity OtherAddresses OtherAddress 3 FirstAddressLine],
[Entity OtherAddresses OtherAddress 3 AddressNumber],
[Entity OtherAddresses OtherAddress 3 AddressNumberWithinBuilding],
[Entity OtherAddresses OtherAddress 3 MailRouting],
[Entity OtherAddresses OtherAddress 3 AdditionalAddressLine 1],
[Entity OtherAddresses OtherAddress 3 AdditionalAddressLine 2],
[Entity OtherAddresses OtherAddress 3 AdditionalAddressLine 3],
[Entity OtherAddresses OtherAddress 3 City],
[Entity OtherAddresses OtherAddress 3 Region],
[Entity OtherAddresses OtherAddress 3 Country],
[Entity OtherAddresses OtherAddress 3 PostalCode],
[Entity OtherAddresses OtherAddress 4 xmllang],
[Entity OtherAddresses OtherAddress 4 type],
[Entity OtherAddresses OtherAddress 4 FirstAddressLine],
[Entity OtherAddresses OtherAddress 4 AddressNumber],
[Entity OtherAddresses OtherAddress 4 AddressNumberWithinBuilding],
[Entity OtherAddresses OtherAddress 4 MailRouting],
[Entity OtherAddresses OtherAddress 4 AdditionalAddressLine 1],
[Entity OtherAddresses OtherAddress 4 AdditionalAddressLine 2],
[Entity OtherAddresses OtherAddress 4 AdditionalAddressLine 3],
[Entity OtherAddresses OtherAddress 4 City],
[Entity OtherAddresses OtherAddress 4 Region],
[Entity OtherAddresses OtherAddress 4 Country],
[Entity OtherAddresses OtherAddress 4 PostalCode],
[Entity OtherAddresses OtherAddress 5 xmllang],
[Entity OtherAddresses OtherAddress 5 type],
[Entity OtherAddresses OtherAddress 5 FirstAddressLine],
[Entity OtherAddresses OtherAddress 5 AddressNumber],
[Entity OtherAddresses OtherAddress 5 AddressNumberWithinBuilding],
[Entity OtherAddresses OtherAddress 5 MailRouting],
[Entity OtherAddresses OtherAddress 5 AdditionalAddressLine 1],
[Entity OtherAddresses OtherAddress 5 AdditionalAddressLine 2],
[Entity OtherAddresses OtherAddress 5 AdditionalAddressLine 3],
[Entity OtherAddresses OtherAddress 5 City],
[Entity OtherAddresses OtherAddress 5 Region],
[Entity OtherAddresses OtherAddress 5 Country],
[Entity OtherAddresses OtherAddress 5 PostalCode],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 xmllang],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 type],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 FirstAddressLine],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 AddressNumber],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 AddressNumberWithinBuilding],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 MailRouting],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 AdditionalAddressLine 1],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 AdditionalAddressLine 2],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 AdditionalAddressLine 3],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 City],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 Region],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 Country],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 1 PostalCode],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 xmllang],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 type],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 FirstAddressLine],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 AddressNumber],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 AddressNumberWithinBuilding],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 MailRouting],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 AdditionalAddressLine 1],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 AdditionalAddressLine 2],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 AdditionalAddressLine 3],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 City],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 Region],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 Country],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 2 PostalCode],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 xmllang],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 type],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 FirstAddressLine],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 AddressNumber],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 AddressNumberWithinBuilding],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 MailRouting],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 AdditionalAddressLine 1],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 AdditionalAddressLine 2],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 AdditionalAddressLine 3],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 City],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 Region],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 Country],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 3 PostalCode],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 xmllang],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 type],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 FirstAddressLine],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 AddressNumber],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 AddressNumberWithinBuilding],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 MailRouting],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 AdditionalAddressLine 1],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 AdditionalAddressLine 2],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 AdditionalAddressLine 3],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 City],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 Region],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 Country],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 4 PostalCode],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 xmllang],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 type],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 FirstAddressLine],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 AddressNumber],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 AddressNumberWithinBuilding],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 MailRouting],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 AdditionalAddressLine 1],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 AdditionalAddressLine 2],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 AdditionalAddressLine 3],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 City],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 Region],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 Country],
[Entity TransliteratedOtherAddresses TransliteratedOtherAddress 5 PostalCode],
[Entity RegistrationAuthority RegistrationAuthorityID],
[Entity RegistrationAuthority OtherRegistrationAuthorityID],
[Entity RegistrationAuthority RegistrationAuthorityEntityID],
[Entity LegalJurisdiction],
[Entity EntityCategory],
[Entity LegalForm EntityLegalFormCode],
[Entity LegalForm OtherLegalForm],
[Entity AssociatedEntity type],
[Entity AssociatedEntity AssociatedLEI],
[Entity AssociatedEntity AssociatedEntityName],
[Entity AssociatedEntity AssociatedEntityName xmllang],
[Entity EntityExpirationDate],
[Entity EntityExpirationReason],
[Entity SuccessorEntity SuccessorLEI],
[Entity SuccessorEntity SuccessorEntityName],
[Entity SuccessorEntity SuccessorEntityName xmllang],
[Registration InitialRegistrationDate],
[Registration LastUpdateDate],
[Registration RegistrationStatus],
[Registration NextRenewalDate],
[Registration ManagingLOU],
[Registration ValidationSources],
[Registration ValidationAuthority ValidationAuthorityID],
[Registration ValidationAuthority OtherValidationAuthorityID],
[Registration ValidationAuthority ValidationAuthorityEntityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 1 ValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 1 OtherValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 1 ValidationAuthorityEntityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 2 ValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 2 OtherValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 2 ValidationAuthorityEntityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 3 ValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 3 OtherValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 3 ValidationAuthorityEntityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 4 ValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 4 OtherValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 4 ValidationAuthorityEntityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 5 ValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 5 OtherValidationAuthorityID],
[Registration OtherValidationAuthorities OtherValidationAuthority 5 ValidationAuthorityEntityID],
[GLEIFCDFDeltaExtraCol1],
[GLEIFCDFDeltaExtraCol2],
[GLEIFCDFDeltaExtraCol3],
[GLEIFCDFDeltaExtraCol4],
[GLEIFCDFDeltaExtraCol5],
[GLEIFCDFDeltaExtraCol6]
    )
    VALUES
    (
N'" + legalEntity.LEI + @"',
N'" + entityDetails.LegalName + @"',
N'" + address.LegalNamexmllang + @"',
N'" + (ListotherEntityNames?.Count > 0 ? ListotherEntityNames[0]?.OtherEntityName : "") + @"',
N'" + (ListotherEntityNames?.Count > 0 ? ListotherEntityNames[0]?.OtherEntityNamexmllang : "") + @"',
N'" + (ListotherEntityNames?.Count > 0 ? ListotherEntityNames[0]?.type : "") + @"',
N'" + (ListotherEntityNames?.Count >= 2 ? ListotherEntityNames[1]?.OtherEntityName : "") + @"',
N'" + (ListotherEntityNames?.Count >= 2 ? ListotherEntityNames[1]?.OtherEntityNamexmllang : "") + @"',
N'" + (ListotherEntityNames?.Count >= 2 ? ListotherEntityNames[1]?.type : "") + @"',
N'" + (ListotherEntityNames?.Count >= 3 ? ListotherEntityNames[2]?.OtherEntityName : "") + @"',
N'" + (ListotherEntityNames?.Count >= 3 ? ListotherEntityNames[2]?.OtherEntityNamexmllang : "") + @"',
N'" + (ListotherEntityNames?.Count >= 3 ? ListotherEntityNames[2]?.type : "") + @"',
N'" + (ListotherEntityNames?.Count >= 4 ? ListotherEntityNames[3]?.OtherEntityName : "") + @"',
N'" + (ListotherEntityNames?.Count >= 4 ? ListotherEntityNames[3]?.OtherEntityNamexmllang : "") + @"',
N'" + (ListotherEntityNames?.Count >= 4 ? ListotherEntityNames[3]?.type : "") + @"',
N'" + (ListotherEntityNames?.Count >= 5 ? ListotherEntityNames[4]?.OtherEntityName : "") + @"',
N'" + (ListotherEntityNames?.Count >= 5 ? ListotherEntityNames[4]?.OtherEntityNamexmllang : "") + @"',
N'" + (ListotherEntityNames?.Count >= 5 ? ListotherEntityNames[4]?.type : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count > 0 ? ListTransliteratedOtherEntityNames[0]?.TranOtherEntityName : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count > 0 ? ListTransliteratedOtherEntityNames[0]?.TranOtherEntityNamexmllang : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count > 0 ? ListTransliteratedOtherEntityNames[0]?.TranOtherEntitytype : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 2 ? ListTransliteratedOtherEntityNames[1]?.TranOtherEntityName : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 2 ? ListTransliteratedOtherEntityNames[1]?.TranOtherEntityNamexmllang : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 2 ? ListTransliteratedOtherEntityNames[1]?.TranOtherEntitytype : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 3 ? ListTransliteratedOtherEntityNames[2]?.TranOtherEntityName : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 3 ? ListTransliteratedOtherEntityNames[2]?.TranOtherEntityNamexmllang : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 3 ? ListTransliteratedOtherEntityNames[2]?.TranOtherEntitytype : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 4 ? ListTransliteratedOtherEntityNames[3]?.TranOtherEntityName : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 4 ? ListTransliteratedOtherEntityNames[3]?.TranOtherEntityNamexmllang : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 4 ? ListTransliteratedOtherEntityNames[3]?.TranOtherEntitytype : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 5 ? ListTransliteratedOtherEntityNames[4]?.TranOtherEntityName : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 5 ? ListTransliteratedOtherEntityNames[4]?.TranOtherEntityNamexmllang : "") + @"',
N'" + (ListTransliteratedOtherEntityNames?.Count >= 5 ? ListTransliteratedOtherEntityNames[4]?.TranOtherEntitytype : "") + @"',
N'" + address.LegalAddressxmllang + @"',
N'" + address.FirstAddressLine + @"',
N'" + address.FirstAddressNumber + @"',
N'" + address.AddressNumberWithinBuilding + @"',
N'" + address.MailRouting + @"',
N'" + address.AdditionalAddressLine + @"',
N'" + address.AdditionalAddressLine + @"',
N'" + address.AdditionalAddressLine + @"',
N'" + address.City + @"',
N'" + address.Region + @"',
N'" + address.Country + @"',
N'" + address.PostalCode + @"',
N'" + HQaddress.LegalAddressxmllang + @"',
N'" + HQaddress.FirstAddressLine + @"',
N'" + HQaddress.FirstAddressNumber + @"',
N'" + HQaddress.AddressNumberWithinBuilding + @"',
N'" + HQaddress.MailRouting + @"',
N'" + HQaddress.AdditionalAddressLine + @"',
N'" + HQaddress.AdditionalAddressLine + @"',
N'" + HQaddress.AdditionalAddressLine + @"',
N'" + HQaddress.City + @"',
N'" + HQaddress.Region + @"',
N'" + HQaddress.Country + @"',
N'" + HQaddress.PostalCode + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.xmllang : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.type : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.FirstAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.AddressNumber : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.MailRouting : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count > 1 ? ListOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count > 2 ? ListOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.City : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.Region : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.Country : "") + @"',
N'" + (ListOtherAddresses?.Count > 0 ? ListOtherAddresses[0]?.PostalCode : "PostalCode1") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.xmllang : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.type : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.FirstAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.AddressNumber : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.MailRouting : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.City : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.Region : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.Country : "") + @"',
N'" + (ListOtherAddresses?.Count >= 2 ? ListOtherAddresses[1]?.PostalCode : "PostalCode2") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.xmllang : "xmllang3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.type : "type3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.FirstAddressLine : "FirstAddressLine3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.AddressNumber : "AddressNumber3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.AddressNumberWithinBuilding : "AddressNumberWithinBuilding3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.MailRouting : "MailRouting3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "AdditionalAddressLine31") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "AdditionalAddressLine32") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "AdditionalAddressLine33") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.City : "City3") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.Region : "") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.Country : "") + @"',
N'" + (ListOtherAddresses?.Count >= 3 ? ListOtherAddresses[2]?.PostalCode : "PostalCode3") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.xmllang : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.type : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.FirstAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.AddressNumber : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.MailRouting : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.City : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.Region : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.Country : "") + @"',
N'" + (ListOtherAddresses?.Count >= 4 ? ListOtherAddresses[3]?.PostalCode : "PostalCode4") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.xmllang : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.type : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.FirstAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.AddressNumber : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.MailRouting : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.City : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.Region : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.Country : "") + @"',
N'" + (ListOtherAddresses?.Count >= 5 ? ListOtherAddresses[4]?.PostalCode : "PostalCode") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.xmllang : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.type : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.FirstAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.AddressNumber : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.MailRouting : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.City : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.Region : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.Country : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count > 0 ? ListTransliteratedOtherAddresses[0]?.PostalCode : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.xmllang : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.type : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.FirstAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.AddressNumber : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.MailRouting : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.City : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.Region : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.Country : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 2 ? ListTransliteratedOtherAddresses[1]?.PostalCode : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.xmllang : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.type : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.FirstAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.AddressNumber : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.MailRouting : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.City : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.Region : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.Country : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 3 ? ListTransliteratedOtherAddresses[2]?.PostalCode : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.xmllang : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.type : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.FirstAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.AddressNumber : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.MailRouting : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.City : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.Region : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.Country : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 4 ? ListTransliteratedOtherAddresses[3]?.PostalCode : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.xmllang : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.type : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.FirstAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.AddressNumber : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.AddressNumberWithinBuilding : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.MailRouting : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.OtherAdditionalAddresses?.AdditionalAddressLine : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.City : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.Region : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.Country : "") + @"',
N'" + (ListTransliteratedOtherAddresses?.Count >= 5 ? ListTransliteratedOtherAddresses[4]?.PostalCode : "") + @"',
N'" + registrationDetails.RegistrationAuthorityID + @"',
N'" + registrationDetails.OtherRegistrationAuthorityID + @"',
N'" + registrationDetails.RegistrationAuthorityEntityID + @"',
N'" + entityDetails.LegalJurisdiction + @"',
N'" + entityDetails.EntityCategory + @"',
N'" + legalForm.EntityLegalFormCode + @"',
N'" + (legalForm?.OtherLegalForm ?? "OtherLegalForm") + @"',
'',
'',
'',
'',
N'" + entityDetails.EntityStatus + @"',
N'" + entityDetails.EntityCreationDate + @"',
N'" + entityDetails.EntityCreationDate + @"',
'',
'',
'',
N'" + (registrationDetails?.InitialRegistrationDate ?? "InitialRegistrationDate") + @"',
N'" + registrationDetails.LastUpdateDate + @"',
N'" + registrationDetails.RegistrationStatus + @"',
N'" + registrationDetails.NextRenewalDate + @"',
N'" + registrationDetails.ManagingLOU + @"',
N'" + (registrationDetails?.ValidationSources ?? "ValidationSources") + @"',
N'" + (listValidationAuthority?.Count > 0 ? listValidationAuthority[0]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count > 0 ? listValidationAuthority[0]?.OtherValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count > 0 ? listValidationAuthority[0]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 2 ? listValidationAuthority[1]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 2 ? listValidationAuthority[1]?.OtherValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 2 ? listValidationAuthority[1]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 3 ? listValidationAuthority[2]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 3 ? listValidationAuthority[2]?.OtherValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 3 ? listValidationAuthority[2]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 4 ? listValidationAuthority[3]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 4 ? listValidationAuthority[3]?.OtherValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 4 ? listValidationAuthority[3]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 5 ? listValidationAuthority[4]?.ValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 5 ? listValidationAuthority[4]?.OtherValidationAuthorityID : "") + @"',
N'" + (listValidationAuthority?.Count >= 5 ? listValidationAuthority[4]?.ValidationAuthorityID : "") + @"',
'',
'',
'',
'',
'',
'',
'',
N'" + legalEntity.LEI + @"'
    )";
                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    try
                                    {
                                        connection.Open();
                                        int rowsAffected = command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError($"Error: {ex.Message}---{i}");
                                    }
                                }
                            }
                        }
                    }
                    status = "Records added successfully";
                }
                catch (Exception ex)
                {
                    status = $"Getting error in {ex.Message} line number ---{i}";
                    _logger.LogError($"Getting error in {ex.Message} line number ---{i}");
                }
            }
            else
            {
                status = "File does not exist.";
                _logger.LogInformation("File does not exist.");
            }
            return status;
        }

        private void RenameFiles(string directoryPath)
        {
            try
            {
                _logger.LogInformation($"Calling RenameFiles");
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                if (directory.Exists)
                {
                    FileInfo[] files = directory.GetFiles("*-gleif-goldencopy-rr-last-day*.json");
                    foreach (FileInfo file in files)
                    {
                        string pattern = @"(\d{8}-\d{4})-gleif-goldencopy-rr-last-day(?: - Copy)?\.json";
                        Match match = Regex.Match(file.Name, pattern);
                        if (match.Success)
                        {
                            string newFileName = $"gleif-goldencopy-lei2.json";
                            string newFilePath = Path.Combine(directory.FullName, newFileName);
                            file.MoveTo(newFilePath);
                            _logger.LogInformation($"File '{file.Name}' renamed to '{newFileName}'");
                        }
                        else
                        {
                            _logger.LogInformation($"Invalid file name format for '{file.Name}'. Skipping...");
                        }
                    }
                    _logger.LogInformation($"File renaming completed.");
                }
                else
                {
                    _logger.LogInformation($"Directory not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
