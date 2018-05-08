using System;
using System.Linq;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;
using BLAdHocDimensionGroupPubcodeMap = FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap;

namespace ADMS.ClientMethods
{
    public class TenMissions : ClientSpecialCommon
    {
        private const string CompanyBusGroupName = "TenMissions_StartsWith_Company_BUS_DESC";
        private const string CompanyBusStartWithGroupName = "TenMissions_StartsWith_Company_BUS_DESC";
        private const string CompanyBusDim = "BUS_DESC";
        private const string MasterCodeValueField = "Master Code";
        private const string CompanyNameField = "Company Name";
        private const string NumericDefaultMaxValue = "99";
        private const string VehicleTypeServiceGroupName = "TenMissions_Contains_Company_VEHICLETYPESERVICE";
        private const string VehicleTypeServiceDim = "VEHICLETYPESERVICE";
        private const string VehicleTypeServiceFieldName = "If term is anywhere in company name";
        private const char CommaDelimiter = ',';
        private const string CompanyBusContainsGroupName = "TenMissions_Contains_Company_BUS_DESC";

        public void TMCompanyContainsAdHocImport(FileMoved eventMessage)
        {
            var readAndFillParams = new FillAgGroupAndTableArgs()
            {
                EventMessage = eventMessage,
                AdHocDimensionGroupName = CompanyBusContainsGroupName,
                CreatedDimension = CompanyBusDim,
                DimensionOperator = ContainsOperation,
                DimensionValueField = MasterCodeValueField,
                MatchValueField = CompanyNameField,
                StandardField = CompanyStandardField,
                DefaultValue = NumericDefaultMaxValue,
                IsPubcodeSpecific = true
            };
            var adg = ClientMethodHelpers.ReadAndFillAgGroupAndTableReturnGroup(readAndFillParams);

            if (!adg.IsPubcodeSpecific)
            {
                return;
            }

            ImportDimensionMaps(readAndFillParams, adg);
        }

        
        public void TMCompanyStartsWithAdHocImport(FileMoved eventMessage)
        {
            var readAndFillParams = new FillAgGroupAndTableArgs()
            {
                EventMessage = eventMessage,
                AdHocDimensionGroupName = CompanyBusStartWithGroupName,
                CreatedDimension = CompanyBusDim,
                DimensionOperator = StartsWithOperation,
                DimensionValueField = MasterCodeValueField,
                MatchValueField = CompanyNameField,
                StandardField = CompanyStandardField,
                DefaultValue = NumericDefaultMaxValue,
                IsPubcodeSpecific = true
            };
            var adg = ClientMethodHelpers.ReadAndFillAgGroupAndTableReturnGroup(readAndFillParams);

            if (!adg.IsPubcodeSpecific)
            {
                return;
            }

            ImportDimensionMaps(readAndFillParams, adg);
        }

        public void VehicleTypesServicedMatching(FileMoved eventMessage)
        {
            var readAndFillParams = new FillAgGroupAndTableArgs()
            {
                EventMessage = eventMessage,
                AdHocDimensionGroupName = VehicleTypeServiceGroupName,
                CreatedDimension = VehicleTypeServiceDim,
                DimensionOperator = ContainsOperation,
                DimensionValueField = MasterCodeValueField,
                MatchValueField = VehicleTypeServiceFieldName,
                StandardField = CompanyStandardField,
                DefaultValue = string.Empty
            };
            var adg = ClientMethodHelpers.ReadAndFillAgGroupAndTableReturnGroup(readAndFillParams);

            if (adg.IsPubcodeSpecific != true)
            {
                return;
            }

            ImportDimensionMaps(readAndFillParams, adg);
        }

        public void DonoSuppression(KMPlatform.Entity.Client client, int sourceFileId, string processCode)
        {
            //1. get xml file
            //2. call sproc

            string filePath = @"C:\ADMS\Suppression\TenMissions\DonoSuppression.xml";
            FileInfo donoFile = new FileInfo(filePath);
            System.Xml.Linq.XDocument xdoc = XmlFunctions.CreateFromFile(donoFile);

            SqlConnection conn = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
            try
            {
                SqlCommand cmd = new SqlCommand("ccp_TenMissions_DONO_Suppression", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SourceFileID", sourceFileId);
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Parameters.AddWithValue("@ClientId", client.ClientID.ToString());
                cmd.Parameters.AddWithValue("@Xml", xdoc.ToString());

                FrameworkUAD.DataAccess.DataFunctions.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".DonoSuppression");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public FrameworkUAD.Object.ImportFile ApplyConditionalAdHocs(FrameworkUAD.Object.ImportFile dataIV)
        {
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = ahdgWorker.Select(dataIV.ClientId, true).OrderBy(y => y.OrderOfOperation).ToList();
            FrameworkUAS.BusinessLogic.AdHocDimension adWorker = new FrameworkUAS.BusinessLogic.AdHocDimension();
            FrameworkUAS.Entity.AdHocDimensionGroup vehicleADG = null;
            FrameworkUAS.Entity.AdHocDimensionGroup bdStartsWithADG = null;
            FrameworkUAS.Entity.AdHocDimensionGroup bdContainsADG = null;

            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals(VehicleTypeServiceGroupName)))
            {
                vehicleADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals(VehicleTypeServiceGroupName));
            }

            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals(CompanyBusGroupName)))
            {
                bdStartsWithADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals(CompanyBusGroupName));
            }

            if (ahdGroups.Exists(x => x.AdHocDimensionGroupName.Equals(CompanyBusContainsGroupName)))
            {
                bdContainsADG = ahdGroups.Single(x => x.AdHocDimensionGroupName.Equals(CompanyBusContainsGroupName));
            }

            List<FrameworkUAS.Entity.AdHocDimension> bdStartsWithList = new List<AdHocDimension>();
            List<FrameworkUAS.Entity.AdHocDimension> bdContainsList = new List<AdHocDimension>();
            if (bdStartsWithADG != null)
                bdStartsWithList = adWorker.Select(bdStartsWithADG.AdHocDimensionGroupId);
            if (bdContainsADG != null)
                bdContainsList = adWorker.Select(bdContainsADG.AdHocDimensionGroupId);

            bool vehiclePubExisted = false;
            bool bdPubExisted = false;

            #region PubCodes
            List<string> busDescPubs = new List<string>();
            busDescPubs.Add("SVYKPI13");
            busDescPubs.Add("SVYKPI14");
            busDescPubs.Add("SVYOWN13");
            busDescPubs.Add("SVYREAD14");
            busDescPubs.Add("SVYTT14");
            busDescPubs.Add("SVYFBREAD16");
            busDescPubs.Add("SVYKPI15");
            busDescPubs.Add("SVYOWN15");
            busDescPubs.Add("SVYRWREAD15");
            busDescPubs.Add("SVYRWSP15");
            busDescPubs.Add("SVYRWST16");
            //Added products (SVYRWREAD17,SVYKPI17,SVYRWST17) per PBI: 44838
            busDescPubs.Add("SVYRWREAD17");
            busDescPubs.Add("SVYKPI17");
            busDescPubs.Add("SVYRWST17");

            FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap adgpmWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap();
            List<string> carPubs = new List<string>();
            List<FrameworkUAS.Entity.AdHocDimension> adVehicleList = new List<AdHocDimension>();
            if (vehicleADG != null)
            {
                List<FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap> adgpmList = adgpmWorker.Select(vehicleADG.AdHocDimensionGroupId);
                foreach (FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap p in adgpmList)
                    carPubs.Add(p.Pubcode.ToUpper());

                adVehicleList = adWorker.Select(vehicleADG.AdHocDimensionGroupId);
            }
            #endregion

            #region Create BUS_DESC and VEHICLETYPESERVICE adhoc columns
            #region VEHICLETYPESERVICE
            bool colExist = false;
            if (dataIV.HeadersTransformed.ContainsKey(vehicleADG.CreatedDimension))
                colExist = true;

            if (colExist == false)
                dataIV.HeadersTransformed.Add(vehicleADG.CreatedDimension, (dataIV.HeadersTransformed.Count + 1).ToString());

            foreach (var k in dataIV.DataTransformed.Keys)
            {
                StringDictionary r = dataIV.DataTransformed[k];
                if (!r.ContainsKey(vehicleADG.CreatedDimension.ToUpper()))
                    r.Add(vehicleADG.CreatedDimension.ToUpper(), vehicleADG.DefaultValue);
                else
                    r[vehicleADG.CreatedDimension.ToUpper()] = vehicleADG.DefaultValue;
            }       
            #endregion
            #region BUS_DESC
            colExist = false;
            string bdDimension = bdStartsWithADG.CreatedDimension.ToUpper();
            string bdDefaultValue = bdStartsWithADG.DefaultValue;
            if (dataIV.HeadersTransformed.ContainsKey(bdDimension))
                colExist = true;

            if (colExist == false)
                dataIV.HeadersTransformed.Add(bdDimension, (dataIV.HeadersTransformed.Count + 1).ToString());

            foreach (var k in dataIV.DataTransformed.Keys)
            {
                StringDictionary r = dataIV.DataTransformed[k];
                if (!r.ContainsKey(bdDimension))
                    r.Add(bdDimension, bdDefaultValue);
                else
                    r[bdDimension] = bdDefaultValue;
            }
            #endregion
            
            #endregion
            //loop through each record because based on PubCode we apply different rules
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                StringDictionary myRow = dataIV.DataTransformed[key];
                string pubCode = string.Empty;
                if (myRow.ContainsKey("PubCode"))
                    pubCode = myRow["PubCode"].ToString().ToUpper();

                string companyField = string.Empty;
                if (myRow.ContainsKey(bdStartsWithADG.StandardField))
                    companyField = myRow[bdStartsWithADG.StandardField].ToString();

                string business = string.Empty;
                string demo2 = string.Empty;
                string demo10 = string.Empty;
                if (myRow.ContainsKey("BUSINESS"))
                    business = myRow["BUSINESS"].ToString().ToUpper();
                if (myRow.ContainsKey("DEMO2"))
                    demo2 = myRow["DEMO2"].ToString().ToUpper();
                if (myRow.ContainsKey("DEMO10"))
                    demo10 = myRow["DEMO10"].ToString().ToUpper();

                #region BUS_DESC Logic
                if (pubCode.Equals("FNDR"))
                {
                    #region FNDR
                    bdPubExisted = true;
                    if (business == "2" || business == "02")
                        myRow[bdDimension] = "2";
                    else if ((business != "2" && business != "02") && (demo2 == "1" || demo2 == "01"))
                        myRow[bdDimension] = "1";
                    else if ((business != "2" && business != "02") && (demo2 != "1" && demo2 != "01") && !string.IsNullOrEmpty(companyField))
                    {
                        //check starts and contains list - if found then set to 3 else leave default
                        bool found = false;
                        foreach (FrameworkUAS.Entity.AdHocDimension c in bdContainsList)
                        {
                            if (companyField.ToLower().Contains(c.MatchValue.ToLower()))
                            {
                                found = true;
                                myRow[bdDimension] = "3";
                                break;
                            }
                        }

                        if(found == false)
                        {
                            foreach (FrameworkUAS.Entity.AdHocDimension m in bdStartsWithList)
                            {
                                if (companyField.StartsWith(m.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    found = true;
                                    myRow[bdDimension] = "3";
                                    break;
                                }
                            }
                        }

                    }
                    #endregion
                }
                else if (pubCode.Equals("RCHT"))
                {
                    #region RCHT
                    bdPubExisted = true;
                    if (business == "13")
                        myRow[bdDimension] = "13";
                    else if ((business != "13") && (demo10 == "1" || demo10 == "01"))
                        myRow[bdDimension] = "1";
                    else if (business != "13" && (demo10 != "1" && demo10 != "01") && !string.IsNullOrEmpty(companyField))
                    {
                        //check starts and contains list - if found then set to 3 else leave default
                        bool found = false;
                        foreach (FrameworkUAS.Entity.AdHocDimension c in bdContainsList)
                        {
                            if (companyField.ToLower().Contains(c.MatchValue.ToLower()))
                            {
                                found = true;
                                myRow[bdDimension] = "3";
                                break;
                            }
                        }

                        if (found == false)
                        {
                            foreach (FrameworkUAS.Entity.AdHocDimension m in bdStartsWithList)
                            {
                                if (companyField.StartsWith(m.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    found = true;
                                    myRow[bdDimension] = "3";
                                    break;
                                }
                            }
                        }

                    }
                    #endregion
                }
                else if (busDescPubs.Contains(pubCode))
                {
                    #region Surveys
                    bdPubExisted = true;
                    bool found = false;

                    foreach (FrameworkUAS.Entity.AdHocDimension c in bdContainsList)
                    {
                        if (companyField.ToLower().Contains(c.MatchValue.ToLower()))
                        {
                            found = true;
                            myRow[bdDimension] = c.DimensionValue;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        foreach (FrameworkUAS.Entity.AdHocDimension m in bdStartsWithList)
                        {
                            if (companyField.StartsWith(m.MatchValue, StringComparison.CurrentCultureIgnoreCase))
                            {
                                found = true;
                                myRow[bdDimension] = m.DimensionValue;
                                break;
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region VEHICLETYPESERVICE Logic
                if (vehicleADG != null)
                {
                    if (carPubs.Contains(pubCode))
                    {
                        vehiclePubExisted = true;
                        string sourceString = myRow[vehicleADG.StandardField].ToString();
                        foreach (FrameworkUAS.Entity.AdHocDimension ad in adVehicleList)
                        {
                            if (sourceString.ToLower().Contains(ad.MatchValue.ToLower()))
                            {
                                string generatedValue = ad.DimensionValue;
                                if (!myRow.ContainsKey(vehicleADG.CreatedDimension.ToUpper()))
                                    myRow.Add(vehicleADG.CreatedDimension.ToUpper(), generatedValue);
                                else
                                    myRow[vehicleADG.CreatedDimension.ToUpper()] = generatedValue;
                            }
                        }
                    }
                }
                #endregion
            }

            if (vehiclePubExisted == false)
            {
                //no pubs for vehicle so lets remove column
                dataIV.HeadersTransformed.Remove(vehicleADG.CreatedDimension);
                foreach (var k in dataIV.DataTransformed.Keys)
                {
                    StringDictionary r = dataIV.DataTransformed[k];
                    r.Remove(vehicleADG.CreatedDimension.ToUpper());
                }
            }

            if (bdPubExisted == false)
            {
                //no pubs for vehicle so lets remove column
                dataIV.HeadersTransformed.Remove(bdStartsWithADG.CreatedDimension);
                foreach (var k in dataIV.DataTransformed.Keys)
                {
                    StringDictionary r = dataIV.DataTransformed[k];
                    r.Remove(bdStartsWithADG.CreatedDimension.ToUpper());
                }
            }

            return dataIV;
        }

        private static void ImportDimensionMaps(FillAgGroupAndTableArgs readAndFillParams, AdHocDimensionGroup adg)
        {
            var dataRow = readAndFillParams.Dt.Rows[0];
            var pubCodes = dataRow[PubCodeFieldName].ToString().Split(CommaDelimiter);
            var mapList = pubCodes.Select(s => new AdHocDimensionGroupPubcodeMap
                {
                    AdHocDimensionGroupId = adg.AdHocDimensionGroupId,
                    CreatedByUserID = 1,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Pubcode = s.Trim()
                })
                .ToList();
            var mapWorker = new BLAdHocDimensionGroupPubcodeMap();
            mapWorker.SaveBulkSqlInsert(mapList);
        }
    }
}
