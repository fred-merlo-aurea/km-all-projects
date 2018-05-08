using System;
using System.Data;
using System.Linq;
using System.Transactions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace FrameworkUAS.BusinessLogic
{
    public class ClientMethods
    {
        private const string YearOfBirthColumn = "YearOfBirth";
        private const string PrimaryEmailAddressColumn = "PrimaryEmailAddress";
        private const string SubscriberIdField = "SubscriberId";
        private const string NullValue = "NULL";

        #region Haymarket
        public bool Haymarket_CreateTempCMSTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Haymarket_CreateTempCMSTables();
                scope.Complete();
            }

            return done;
        }
        public bool Haymarket_DropTempCMSTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Haymarket_DropTempCMSTables();
                scope.Complete();
            }

            return done;
        }

        public bool Haymarket_CMS_Insert_Subscriber(DataTable dtInsert)
        {
            CleanupDataTableValues(dtInsert, true);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtInsert,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Insert_Subscriber(table);
                },
                batchSize,
                row =>
                {
                    if (row[YearOfBirthColumn].ToString().Length > 4)
                    {
                        row[YearOfBirthColumn] = String.Empty;
                    }
                    
                    row[PrimaryEmailAddressColumn] = CleanupEmailValue(row[PrimaryEmailAddressColumn].ToString());
                });

            return result;
        }
        
        public bool Haymarket_CMS_Update_Address(DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate, true);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Update_Address(table);
                },
                batchSize);

            return result;
        }
        public bool Haymarket_CMS_Insert_PublicationPubCode(DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate, true);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Insert_PublicationPubCode(table);
                },
                batchSize, 
                writeStatusToConsole: true);

            return result;
        }

        public bool Haymarket_CMS_Insert_Activity(DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate, true);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Insert_Activity(table);
                },
                batchSize, 
                writeStatusToConsole: true);

            return result;
        }

        public bool Haymarket_CMS_Insert_Ecomm(DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate, true);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Insert_Ecomm(table);
                },
                batchSize, 
                writeStatusToConsole: true);

            return result;
        }

        public bool Haymarket_CMS_Update_Medical(DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate, true, new []{ SubscriberIdField });

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.Haymarket_CMS_Update_Medical(table);
                },
                batchSize);

            return result;
        }

        public bool Haymarket_CMS_SetPubCodes()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Haymarket_CMS_SetPubCodes();
                scope.Complete();
            }

            return done;
        }
        public DataTable Haymarket_CMS_Select_Publication()
        {
            DataTable dtCMS = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Publication();
                scope.Complete();
            }

            return dtCMS;
        }
        public DataTable Haymarket_CMS_Select_Publication_Paging(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Publication_Paging(page, records);
            return dtCMS;
        }

        public DataTable Haymarket_CMS_Select_Activity()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Activity();
            return dtCMS;
        }
        public DataTable Haymarket_CMS_Select_Activity_Paging(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Activity_Paging(page,records);
            return dtCMS;
        }
        public DataTable Haymarket_CMS_Select_Ecomm()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Ecomm();
            return dtCMS;
        }
        public DataTable Haymarket_CMS_Select_Ecomm_Paging(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Haymarket_CMS_Select_Ecomm_Paging(page, records);
            return dtCMS;
        }

        public int Haymarket_CMS_Get_Publication_Count()
        {
            int count = 0;
            count = DataAccess.ClientMethods.Haymarket_CMS_Get_Publication_Count();
            return count;
        }
        public int Haymarket_CMS_Get_Activity_Count()
        {
            int count = 0;
            count = DataAccess.ClientMethods.Haymarket_CMS_Get_Activity_Count();
            return count;
        }
        public int Haymarket_CMS_Get_Ecomm_Count()
        {
            int count = 0;
            count = DataAccess.ClientMethods.Haymarket_CMS_Get_Ecomm_Count();
            return count;
        }
        #endregion

        #region BriefMedia
        public DataTable BriefMedia_Select_Data(KMPlatform.Entity.Client client)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.BriefMedia_Select_Data(client);
            return dtCMS;
        }
        public DataTable BriefMedia_Select_Data_Paging(KMPlatform.Entity.Client client, int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.BriefMedia_Select_Data_Paging(client, page, records);
            return dtCMS;
        }

        public int BriefMedia_Relational_Get_Count(KMPlatform.Entity.Client client)
        {
            int count = 0;
            count = DataAccess.ClientMethods.BriefMedia_Relational_Get_Count(client);
            return count;
        }

        public bool BriefMedia_CreateTempCMSTables(KMPlatform.Entity.Client client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.BriefMedia_CreateTempCMSTables(client);
                scope.Complete();
            }

            return done;
        }
        public bool BriefMedia_DropTempCMSTables(KMPlatform.Entity.Client client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.BriefMedia_DropTempCMSTables(client);
                scope.Complete();
            }

            return done;
        }

        public bool BriefMedia_Relational_Insert_Access(KMPlatform.Entity.Client client, DataTable dtInsert)
        {
            CleanupDataTableValues(dtInsert);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtInsert,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Insert_Access(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_Update_Users(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Update_Users(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_Update_TaxBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Update_TaxBehavior(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_Update_PageBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Update_PageBehavior(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_Update_SearchBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Update_SearchBehavior(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_Update_TopicCode(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            CleanupDataTableValues(dtUpdate);

            var batchSize = 2500;
            var result = ExecuteInBatches(dtUpdate,
                table =>
                {
                    DataAccess.ClientMethods.BriefMedia_Relational_Update_TopicCode(client, table);
                },
                batchSize);

            return result;
        }

        public bool BriefMedia_Relational_CleanUpData(KMPlatform.Entity.Client client)
        {
            return TransactionCall(
                () => DataAccess.ClientMethods.BriefMedia_Relational_CleanUpData(client));
        }
        #endregion

        #region GLM
        public bool GLM_CreateTempCMSTables()
        {
            return TransactionCall(DataAccess.ClientMethods.GLM_CreateTempCMSTables);
        }
        public bool GLM_DropTempCMSTables()
        {
            return TransactionCall(DataAccess.ClientMethods.GLM_DropTempCMSTables);
        }

        public bool GLM_Relational_InsertData(DataTable dtInsert)
        {
            CleanupDataTableValues(dtInsert);

            var batchSize = 10000;
            var result = ExecuteInBatches(dtInsert,
                table =>
                {
                    DataAccess.ClientMethods.GLM_Relational_InsertData(table);
                }, 
                batchSize);

            return result;
        }

        public bool GLM_Relational_Update()
        {
            return TransactionCall(DataAccess.ClientMethods.GLM_Relational_Update);
        }
        #endregion

        #region WATT
        public bool WATT_CreateTempCMSTables()
        {
            return TransactionCall(DataAccess.ClientMethods.WATT_CreateTempCMSTables);
        }
        public bool WATT_DropTempCMSTables()
        {
            return TransactionCall(DataAccess.ClientMethods.WATT_DropTempCMSTables);
        }

        public bool WATT_Relational_Process_ECN_GROUPID_PUBCODE(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.WATT_Relational_Process_ECN_GROUPID_PUBCODE(dtTable);
                scope.Complete();
            }

            return done;
        }
        public bool WATT_Relational_Process_MacMic(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.WATT_Relational_Process_MacMic(dtTable);
                scope.Complete();
            }

            return done;
        }
        
        public DataTable WATT_Get_Mic_Data()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.WATT_Get_Mic_Data();
            return dtCMS;
        }
        public DataTable WATT_Get_Mac_Data()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.WATT_Get_Mac_Data();
            return dtCMS;
        }

        public DataTable WATT_Get_ECN_Data(string clientName)
        {
            DataTable dtCMS = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dtCMS = DataAccess.ClientMethods.WATT_Get_ECN_Data(clientName);
                scope.Complete();
            }

            return dtCMS;
        }
        #endregion

        #region Vcast
        public bool Vcast_CreateTempMXBooksTable()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_CreateTempMXBooksTable();
                scope.Complete();
            }

            return done;
        }
        public bool Vcast_DropTempMXBooksTable()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_DropTempMXBooksTable();
                scope.Complete();
            }

            return done;
        }
        public bool Vcast_Process_File_MX_Books(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_Process_File_MX_Books(dtTable);
                scope.Complete();
            }

            return done;
        }
        public DataTable Vcase_Get_Distinct_MX_Books()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Vcase_Get_Distinct_MX_Books();
            return dtCMS;
        }

        public bool Vcast_CreateTempMXElanTable()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_CreateTempMXElanTable();
                scope.Complete();
            }

            return done;
        }
        public bool Vcast_DropTempMXElanTable()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_DropTempMXElanTable();
                scope.Complete();
            }

            return done;
        }
        public bool Vcast_Process_File_MX_Elan(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Vcast_Process_File_MX_Elan(dtTable);
                scope.Complete();
            }

            return done;
        }
        public DataTable Vcase_Get_Distinct_MX_Elan()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Vcase_Get_Distinct_MX_Elan();
            return dtCMS;
        }
        #endregion

        #region Advanstar
        public DataTable Advanstar_Select_Data_Paging(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_Data_Paging(page, records);
            return dtCMS;
        }

        public bool Advanstar_Insert_PersonID(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_PersonID, batchSize);

            return result;
        }

        public DataTable Advanstar_Insert_PersonID_Final()
        {
            DataTable dtCMS = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dtCMS = DataAccess.ClientMethods.Advanstar_Insert_PersonID_Final();
                scope.Complete();
            }

            return dtCMS;
        }

        public bool Advanstar_CreateTempTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Advanstar_CreateTempTables();
                scope.Complete();
            }

            return done;
        }

        public bool Advanstar_Insert_RegCodeCompare(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_RegCodeCompare, batchSize);

            return result;
        }

        public bool Advanstar_Insert_RegCode(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_RegCode, batchSize);

            return result;
        }

        public DataTable Advanstar_Insert_RegCode_Final()
        {
            DataTable dtCMS = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dtCMS = DataAccess.ClientMethods.Advanstar_Insert_RegCode_Final();
                scope.Complete();
            }

            return dtCMS;
        }

        public int Advanstar_Get_Count(string name)
        {
            int count = 0;
            count = DataAccess.ClientMethods.Advanstar_GetCount(name);
            return count;
        }

        public DataTable Advanstar_Select_Data_PagingRegCode(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_Data_PagingRegCode(page, records);
            return dtCMS;
        }

        public bool Advanstar_DropTempTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Advanstar_DropTempTables();
                scope.Complete();
            }

            return done;
        }

        public bool Advanstar_Insert_SourceCode(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_SourceCode, batchSize);

            return result;
        }

        public bool Advanstar_Insert_PriCode(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_PriCode, batchSize);

            return result;
        }

        public bool Advanstar_Insert_RefreshDupes(DataTable temp)
        {
            var batchSize = 2500;
            var result = ExecuteInBatches(temp, DataAccess.ClientMethods.Advanstar_Insert_RefreshDupes, batchSize);

            return result;
        }

        public DataTable Advanstar_Select_Data()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_Data();
            return dtCMS;
        }

        public DataTable Advanstar_Select_InDupes()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_InDupes();
            return dtCMS;
        }

        public DataTable Advanstar_Select_Final()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_Final();
            return dtCMS;
        }

        public DataTable Advanstar_Select_PRDCDES()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_PRDCDES();
            return dtCMS;
        }

        public int Advanstar_Get_ECN_Count(string clientName)
        {
            int count = 0;
            count = DataAccess.ClientMethods.Advanstar_Get_ECN_Count(clientName);
            return count;
        }

        public DataTable Advanstar_Select_ECN_Paging(int page, int records, string clientName)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Advanstar_Select_ECN_Paging(page, records, clientName);
            return dtCMS;
        }

        #endregion

        #region Northstar
        public bool Northstar_CreateTempCMSTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Northstar_CreateTempCMSTables();
                scope.Complete();
            }

            return done;
        }
        public bool Northstar_DropTempCMSTables()
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Northstar_DropTempCMSTables();
                scope.Complete();
            }

            return done;
        }
        
        public bool Northstar_Relational_InsertPerson(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Northstar_Relational_InsertPerson(dtTable);
                scope.Complete();
            }

            return done;
        }
        public bool Northstar_Relational_AddGroup(DataTable dtTable)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Northstar_Relational_AddGroup(dtTable);
                scope.Complete();
            }

            return done;
        }
        
        public int Northstar_Get_WEB_Person_Group_Get_Count()
        {
            int count = 0;
            count = DataAccess.ClientMethods.Northstar_Get_WEB_Person_Group_Get_Count();
            return count;
        }
        public DataTable Northstar_Get_WEB_Person_Group_Data_Paging(int page, int records)
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Northstar_Get_WEB_Person_Group_Data_Paging(page, records);
            return dtCMS;
        }
        public DataTable Northstar_Get_WEB_Person_Group_Data()
        {
            DataTable dtCMS = null;
            dtCMS = DataAccess.ClientMethods.Northstar_Get_WEB_Person_Group_Data();
            return dtCMS;
        }
        #endregion

        #region Canon
        public bool Canon_ConsensusDimension_EventSwipe(string xml,KMPlatform.Entity.Client client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.Canon_ConsensusDimension_EventSwipe(xml, client);
                scope.Complete();
            }

            return done;
        }
        #endregion

        #region Specialty Foods
        public bool SpecialityFoods_ConsensusDimension_EventSwipe(string xml, KMPlatform.Entity.Client client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ClientMethods.SpecialityFoods_ConsensusDimension_EventSwipe(xml, client);
                scope.Complete();
            }

            return done;
        }
        #endregion

        private bool TransactionCall(Func<bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            bool done;
            using (var scope = new TransactionScope())
            {
                done = func();
                scope.Complete();
            }

            return done;
        }
        
        private bool ExecuteInBatches(DataTable dataTable, Action<DataTable> batchDataAction, 
            int batchSize, Action<DataRow> extraRowProcessingAction = null, bool writeStatusToConsole = false)
        {
            if (dataTable == null)
            {
                return true;
            }

            var done = true;
            var total = dataTable.Rows.Count;
            var counter = 0;
            var processedCount = 0;

            var batchTable = dataTable.Clone();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                done = false;
                counter++;
                processedCount++;

                var batchTableRow = batchTable.NewRow();
                batchTableRow.ItemArray = dataRow.ItemArray;
                batchTable.Rows.Add(batchTableRow);
                if (processedCount == total || counter == batchSize)
                {
                    batchTable.AcceptChanges();
                    try
                    {
                        if (writeStatusToConsole)
                        {
                            Console.WriteLine($"Processed: {processedCount} of {total}");
                        }
                        batchDataAction(batchTable);
                        done = true;
                    }
                    catch (Exception ex)
                    {
                        done = false;
                        var message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        var fileLog = new FileLog();
                        fileLog.Save(new Entity.FileLog(-99, -99, message, "ClientMethods"));
                    }

                    counter = 0;
                    batchTable.Clear();
                    batchTable.AcceptChanges();
                }
            }

            return done;
        }

        private static void CleanupDataTableValues(DataTable table, bool removeDoubleQuotes = false,
            string[] excludedColumns = null)
        {
            if (table != null)
            {
                foreach (DataRow dataRow in table.Rows)
                {
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        if (dataRow[dataColumn].ToString().Equals(NullValue, StringComparison.OrdinalIgnoreCase)
                            && (excludedColumns == null || !excludedColumns.Any(x => x.Equals(dataColumn.ColumnName, StringComparison.OrdinalIgnoreCase))))
                        {
                            dataRow[dataColumn] = DBNull.Value;
                        }
                        else if (removeDoubleQuotes)
                        {
                            dataRow[dataColumn] = CommonStringFunctions.RemoveDoubleQuotes(dataRow[dataColumn].ToString());
                        }
                    }
                }

                table.AcceptChanges();
            }
        }
        
        private static string CleanupEmailValue(string email)
        {
            const string comValue = "COM";
            const string dotComValue = ".COM";
            const string netValue = "NET";
            const string dotNetValue = ".NET";
            const string slashValue = "/";
            const string ampersandValue = "&";
            const string commaValue = ",";

            if (String.IsNullOrWhiteSpace(email) || email.Equals(NullValue, StringComparison.OrdinalIgnoreCase))
            {
                return String.Empty;
            }

            email = email.Replace(" ", String.Empty);
            email = email.Replace(slashValue, String.Empty);
            email = email.Replace(ampersandValue, String.Empty);
            email = email.Replace(commaValue, String.Empty);

            if (email.EndsWith(comValue, StringComparison.OrdinalIgnoreCase))
            {
                if (!email.EndsWith(dotComValue, StringComparison.OrdinalIgnoreCase))
                {
                    email = email.ToUpper().Replace(comValue, dotComValue);
                }
            }

            if (email.EndsWith(netValue, StringComparison.OrdinalIgnoreCase))
            {
                if (!email.EndsWith(dotNetValue, StringComparison.OrdinalIgnoreCase))
                {
                    email = email.ToUpper().Replace(netValue, dotNetValue);
                }
            }

            if (email.Length > 250 || !Core_AMS.Utilities.StringFunctions.isEmail(email))
            {
                email = String.Empty;
            }

            return email;
        }
    }
}
