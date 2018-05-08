using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DQM.Modules;
using FrameworkServices.Fakes;
using FrameworkUAD.Object;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using FrameworkUAS.Service;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS.UnitTests.DQM.Helpers.Validation.Common;
using static FrameworkUAD_Lookup.Enums;
using FrameworkUADBusinessLogic = FrameworkUAD.BusinessLogic;

namespace UAS.UnitTests.DQM.Modules
{
    /// <summary>
    /// Unit test for <see cref="DataImport"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class DataImportTest : Fakes
    {
        private DataImport _dataImport;
        private const string fileName = "ValidateFileAsObjectTest.csv";
        private const string Success = "Success";
        private const string Error = "Error";
        private const string AccessKeyValidated = "AccessKey Validated";
        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        private PrivateObject _privateObject;
        private bool SaveResult = false;
        private List<CircImportExport> CieList = new List<CircImportExport>();

        [SetUp]
        public void Setup()
        {
            _dataImport = new DataImport();
            _privateObject = new PrivateObject(_dataImport);
            InitializeFakes();
            CreateAuthorizedUser();
        }

        [Test]
        public void WorkerDoWork_ImporterdFileHaveRowsToProcess_UpdatedControlDetail()
        {
            // Arramge
            var filePath = Path.Combine(BasePath, fileName);
            var circImportExportList = Enumerable.Repeat(CreateCircImportExport(), 10).ToList();
            var content = GetCSV(circImportExportList);
            CreateSettings(filePath, content);
            var doWorkEventArgs = new DoWorkEventArgs(this);
            _privateObject.SetProperty("FileName", filePath);
            var myCheckFile = new FileInfo(filePath);

            SaveBulkSqlUpdate();
            CreateUadImportVesselClient(myCheckFile);

            // Act
            _dataImport.worker_DoWork(this, doWorkEventArgs);

            // Assert
            SaveResult.ShouldBeTrue();
            CieList.ShouldNotBeNull();
            CieList.Count.ShouldBe(10);
        }

        private void SaveBulkSqlUpdate()
        {
            ShimServiceClient.UAD_CircImportExportClient = () =>
            {
                return new ShimServiceClient<ICircImportExport>
                {
                    ProxyGet = () =>
                    {
                        return new StubICircImportExport()
                        {
                            SaveBulkSqlUpdateGuidInt32ListOfCircImportExportClientConnections =
                            (accessKey, UserID, list, client) =>
                            {
                                CieList = list;
                                SaveResult = true;
                                return new Response<bool>
                                {
                                    Message = Success,
                                    ProcessCode = "1123",
                                    Result = true,
                                    Status = ServiceResponseStatusTypes.Success
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUadImportVesselClient(FileInfo myCheckFile)
        {
            ShimServiceClient.UAD_ImportVesselClient = () =>
            {
                return new ShimServiceClient<IImportVessel>
                {
                    ProxyGet = () =>
                    {
                        return new StubIImportVessel()
                        {
                            GetImportVesselGuidFileInfoInt32Int32FileConfiguration = (accessKey, fileInfo, startRow, fileRowBatch, fileConfig) =>
                            {
                                var response = new Response<ImportVessel>();
                                var worker = new FrameworkUADBusinessLogic.ImportVessel();
                                response.Message = AccessKeyValidated;
                                response.Result = worker.GetImportVessel(myCheckFile, startRow, fileRowBatch, fileConfig);
                                if (response.Result != null)
                                {
                                    response.Message = Success;
                                    response.Status = ServiceResponseStatusTypes.Success;
                                }
                                else
                                {
                                    response.Message = Error;
                                    response.Status = ServiceResponseStatusTypes.Error;
                                }
                                return response;
                            }
                        };
                    }
                };
            };
        }

        private void CreateSettings(string path, string content)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = new FileInfo(path);
            file.Directory.Create();
            File.WriteAllText(path, content, Encoding.ASCII);
        }

        private CircImportExport CreateCircImportExport()
        {
            return new CircImportExport
            {
                SubscriberID = 1,
                PublicationID = 1,
                SubscriptionID = 1,
                Batch = 1,
                Hisbatch = string.Empty,
                Hisbatch1 = string.Empty,
                Hisbatch2 = string.Empty,
                Hisbatch3 = string.Empty,
                Pubcode = "1",
                SequenceID = 1,
                Cat = 1,
                Xact = 1,
                XactDate = DateTime.Now,
                Fname = "Test",
                Lname = "Tes",
                Title = "Unit Test",
                Company = "KM All",
                Address = "KM ALl",
                Mailstop = "mail@kmall.com",
                City = "US",
                State = "US",
                ZipCode = "175034",
                Plus4 = "4",
                Country = "US",
                County = "Us",
                CountryID = 1,
                Phone = "01905259335",
                Fax = "00123",
                Mobile = "9872267361",
                Email = "test@unittest.com",
                Website = "http://kmall.com",
                AcctNum = "11122334456676",
                ORIGSSRC = 1,
                Copies = 1,
                NANQ = 1,
                Qsource = 1,
                Qdate = DateTime.Now,
                Cdate = DateTime.Now,
                Par3C = 1,
                EmailID = 1,
                Verify = bool.TrueString.ToLower(),
                Interview = "Scheduled",
                Mail = "km@mail.com",
                Old_Date = DateTime.Now,
                Old_QSRC = string.Empty,
                MBR_ID = 1,
                MBR_Flag = string.Empty,
                MBR_Reject = string.Empty,
                SPECIFY = string.Empty,
                SIC = string.Empty,
                EMPLOY = string.Empty,
                SALES = string.Empty,
                IMB_SERIAL1 = 1,
                IMB_SERIAL2 = 1,
                IMB_SERIAL3 = 1,
                Business = string.Empty,
                Business1 = string.Empty,
                BUSNTEXT = string.Empty,
                Function = string.Empty,
                FUNCTEXT = string.Empty,
                DEMO1 = string.Empty,
                DEMO1TEXT = string.Empty,
                DEMO2 = string.Empty,
                DEMO3 = string.Empty,
                DEMO4 = string.Empty,
                DEMO5 = string.Empty,
                DEMO6 = string.Empty,
                DEMO6TEXT = string.Empty,
                DEMO7 = string.Empty,
                DEMO8 = string.Empty,
                DEMO9 = string.Empty,
                DEMO10 = string.Empty,
                DEMO10TEXT = string.Empty,
                DEMO11 = string.Empty,
                DEMO12 = string.Empty,
                DEMO14 = string.Empty,
                DEMO15 = string.Empty,
                DEMO16 = string.Empty,
                DEMO18 = string.Empty,
                DEMO19 = string.Empty,
                DEMO20 = string.Empty,
                DEMO21 = string.Empty,
                DEMO22 = string.Empty,
                DEMO23 = string.Empty,
                DEMO24 = string.Empty,
                DEMO25 = string.Empty,
                DEMO26 = string.Empty,
                DEMO27 = string.Empty,
                DEMO28 = string.Empty,
                DEMO29 = string.Empty,
                DEMO40 = string.Empty,
                DEMO41 = string.Empty,
                DEMO42 = string.Empty,
                DEMO43 = string.Empty,
                DEMO44 = string.Empty,
                DEMO45 = string.Empty,
                DEMO46 = string.Empty,
                DEMO31 = string.Empty,
                DEMO32 = string.Empty,
                DEMO33 = string.Empty,
                DEMO34 = string.Empty,
                DEMO35 = string.Empty,
                DEMO36 = string.Empty,
                DEMO37 = string.Empty,
                DEMO38 = string.Empty,
                SECBUS = string.Empty,
                SECFUNC = string.Empty,
                Function1 = string.Empty,
                Income1 = string.Empty,
                Age1 = 45,
                Home_Value = string.Empty,
                JOBT1 = string.Empty,
                JOBT1TEXT = string.Empty,
                JOBT2 = string.Empty,
                JOBT3 = string.Empty,
                TOE1 = string.Empty,
                TOE2 = string.Empty,
                AOI1 = string.Empty,
                AOI2 = string.Empty,
                AOI3 = string.Empty,
                PROD1 = string.Empty,
                PROD1TEXT = string.Empty,
                BUYAUTH = string.Empty,
                IND1 = string.Empty,
                IND1TEXT = string.Empty,
                STATUS = true,
                PRICECODE = 1,
                NUMISSUES = 1,
                CPRATE = 1,
                TERM = 12,
                ISSTOGO = 1,
                CARDTYPE = 1,
                CARDTYPECC = 1,
                CCNAME = string.Empty,
                AMOUNTPD = 100,
                AMOUNT = 1200,
                BALDUE = 12,
                AMTEARNED = 12,
                AMTDEFER = 12,
                PAYDATE = DateTime.Now,
                STARTISS = DateTime.Now,
                EXPIRE = DateTime.Now,
                NWEXPIRE = DateTime.Now,
                DELIVERCODE = "CC0123"
            };
        }

        private string GetCSV(List<CircImportExport> list)
        {
            var csvData = new StringBuilder();
            //Get the properties for type T for the headers
            var propInfos = typeof(CircImportExport).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                csvData.Append(propInfos[i].Name);
                if (i < propInfos.Length - 1)
                {
                    csvData.Append(",");
                }
            }
            csvData.AppendLine();

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    var csvProperty = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (csvProperty != null)
                    {
                        string value = csvProperty.ToString();
                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }
                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", "");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }
                        csvData.Append(value);
                    }
                    if (j < propInfos.Length - 1)
                    {
                        csvData.Append(",");
                    }
                }
                csvData.AppendLine();
            }
            return csvData.ToString();
        }

        private void CreateAuthorizedUser()
        {
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClient = new Client
                        {
                            ClientConnections = new KMPlatform.Object.ClientConnections()
                        }
                    }
                };
            };
        }
    }
}
