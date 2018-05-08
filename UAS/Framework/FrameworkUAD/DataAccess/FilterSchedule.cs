using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class FilterSchedule
    {
        #region Data
        public static bool ExistsByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and FilterID = @FilterID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Connection= DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsByFilterSegmentationID(KMPlatform.Object.ClientConnections clientconnection, int filterSegmentationID)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and FilterSegmentationID = @FilterSegmentationID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterSegmentationID", filterSegmentationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsByFileName(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, string fileName)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterSchedule where IsDeleted  = 0 and filterScheduleID <> @FilterScheduleID and FileName = @FileName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterScheduleID", filterScheduleID);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static List<Entity.FilterSchedule> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, bool IsFilterSegmentation)
        {
            List<Entity.FilterSchedule> retList = new List<Entity.FilterSchedule>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_BrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Parameters.Add(new SqlParameter("@IsFilterSegmentation", IsFilterSegmentation));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.FilterSchedule x = builder.Build(rdr);
                    if (x != null)
                    {
                        if (rdr["FilterGroupID_Selected"].ToString() != null && rdr["FilterGroupID_Selected"].ToString() != "")
                        {
                            x.FilterGroupID_Selected = GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                        }
                        if (rdr["FilterGroupID_Suppressed"].ToString() != null && rdr["FilterGroupID_Suppressed"].ToString() != "")
                        {
                            x.FilterGroupID_Suppressed = GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
                        }
                        retList.Add(x);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<Entity.FilterSchedule> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int BrandID, int UserID, bool IsFilterSegmentation)
        {
            List<Entity.FilterSchedule> retList = new List<Entity.FilterSchedule>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_UserID_BrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Parameters.Add(new SqlParameter("@IsFilterSegmentation", IsFilterSegmentation));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.FilterSchedule x = builder.Build(rdr);
                    if (x != null)
                    {
                        if (rdr["FilterGroupID_Selected"].ToString() != null && rdr["FilterGroupID_Selected"].ToString() != "")
                        {
                            x.FilterGroupID_Selected = GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                        }
                        if (rdr["FilterGroupID_Suppressed"].ToString() != null && rdr["FilterGroupID_Suppressed"].ToString() != "")
                        {
                            x.FilterGroupID_Suppressed = GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
                        }
                        retList.Add(x);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        private static List<int> GetFilterGroupID(string FilterGroupIDs)
        {
            return FilterGroupIDs.Split(',').Select(n => int.Parse(n)).ToList();
        }

        public static Entity.FilterSchedule GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterscheduleID)
        {
            Entity.FilterSchedule retItem = new Entity.FilterSchedule();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_FilterScheduleID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterscheduleID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                    if (retItem != null)
                    {
                        retItem.FilterGroupID_Selected = rdr["FilterGroupID_Selected"].ToString() == null || rdr["FilterGroupID_Selected"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                        retItem.FilterGroupID_Suppressed = rdr["FilterGroupID_Suppressed"].ToString() == null || rdr["FilterGroupID_Suppressed"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }

        public static List<Entity.FilterSchedule> GetScheduleByDateTime(KMPlatform.Object.ClientConnections clientconnection, string dt, string time)
        {
            List<Entity.FilterSchedule> retList = new List<Entity.FilterSchedule>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Select_BySchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@time", time);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.FilterSchedule> builder = DynamicBuilder<Entity.FilterSchedule>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.FilterSchedule x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        //public static Tuple<DataTable, string, DataTable, bool> Export(KMPlatform.Object.ClientConnections clientconnection, int filterscheduleID)
        //{
        //    DataTable dtSubscription = new DataTable();
        //    DataTable dt = new DataTable();
        //    string CombinationFilterIDs = string.Empty;
        //    List<string> Selected_FilterNos = new List<string>();
        //    List<string> Suppressed_FilterNos = new List<string>();
        //    string headerText = string.Empty;
        //    bool IsDataMaskError = false;

        //    Entity.FilterSchedule fs = FrameworkUAD.DataAccess.FilterSchedule.GetByID(clientconnection, filterscheduleID);

        //    int userID = fs.UpdatedBy > 0 ? fs.UpdatedBy : fs.CreatedBy;

        //    KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);

        //    if (user == null)
        //    {
        //        throw new Exception("User does not exists");
        //    }


        //    List<Entity.UserDataMask> udm = FrameworkUAD.DataAccess.UserDataMask.GetByUserID(clientconnection, userID);
        //    bool hasEmailMask = udm.Any(u => u.MaskField.ToUpper() == "EMAIL");

        //    //if (hasEmailMask && !KM.Platform.User.IsAdministrator(user) && (fs.ExportTypeID == Enums.ExportType.ECN || fs.ExportTypeID == Enums.ExportType.Marketo))
        //    if (hasEmailMask && (fs.ExportTypeID == FrameworkUAD.BusinessLogic.Enums.ExportType.ECN || fs.ExportTypeID == FrameworkUAD.BusinessLogic.Enums.ExportType.Marketo))
        //    {
        //        IsDataMaskError = true;
        //    }
        //    else
        //    {
        //        Filters fc = MDFilter.LoadFilters(clientconnection, fs.FilterID, userID);

        //        if (fc.Count > 0)
        //        {
        //            if (fs.FilterGroupID_Selected != null)
        //            {
        //                if (fs.FilterGroupID_Selected.Count > 0)
        //                {
        //                    foreach (int filtergroupID in fs.FilterGroupID_Selected)
        //                    {
        //                        string filterID = fc.SingleOrDefault(sf => sf.FilterGroupID == filtergroupID).FilterNo.ToString();
        //                        Selected_FilterNos.Add(filterID);
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (Filter filter in fc)
        //                    {
        //                        Selected_FilterNos.Add(filter.FilterNo.ToString());
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (Filter filter in fc)
        //                {
        //                    Selected_FilterNos.Add(filter.FilterNo.ToString());
        //                }
        //            }

        //            if (fs.FilterGroupID_Suppressed != null)
        //            {
        //                if (fs.FilterGroupID_Suppressed.Count > 0)
        //                {
        //                    foreach (int filtergroupID in fs.FilterGroupID_Suppressed)
        //                    {
        //                        string filterID = fc.SingleOrDefault(sf => sf.FilterGroupID == filtergroupID).FilterNo.ToString();
        //                        Suppressed_FilterNos.Add(filterID);
        //                    }
        //                }
        //            }

        //            StringBuilder Queries = new StringBuilder();

        //            Queries = Filter.generateCombinationQuery(fc, fs.SelectedOperation, fs.SuppressedOperation, string.Join(",", Selected_FilterNos), string.Join(",", Suppressed_FilterNos), "", 0, 0);

        //            Filter f = fc.First();

        //            List<FilterExportField> fef = new List<FilterExportField>();

        //            if (fs.ExportTypeID == Enums.ExportType.ECN)
        //            {
        //                fef = FilterExportField.getByFilterScheduleID(clientconnection, filterscheduleID).FindAll(x => x.IsCustomValue == false && x.IsDescription == false);
        //            }
        //            else
        //            {
        //                fef = FilterExportField.getByFilterScheduleID(clientconnection, filterscheduleID).FindAll(x => x.IsCustomValue == false);
        //            }

        //            List<string> StandardColumnsList = new List<string>();
        //            List<string> MasterGroupColumnList = new List<string>();
        //            List<string> MasterGroupColumnDescList = new List<string>();
        //            List<string> SubscriptionsExtMapperValueList = new List<string>();
        //            List<string> PubSubscriptionsExtMapperValueList = new List<string>();
        //            List<string> StandardColumns = new List<string>();
        //            List<int> PubIDs = new List<int>();
        //            List<string> ResponseGroupIDList = new List<string>();
        //            List<string> ResponseGroupDescIDList = new List<string>();
        //            List<string> selectedItem = new List<string>();

        //            foreach (FilterExportField feField in fef)
        //            {
        //                if (feField.FieldCase == null || feField.FieldCase == "")
        //                    selectedItem.Add(feField.ExportColumn + "|other|None");
        //                else
        //                    selectedItem.Add(feField.ExportColumn + "|other|" + feField.FieldCase);
        //            }

        //            if (f.ViewType == Enums.ViewType.ProductView)
        //            {
        //                PubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(clientconnection, selectedItem, f.PubID);
        //                Tuple<List<string>, List<string>, List<string>> rg = Utilities.GetSelectedResponseGroupStandardExportColumns(clientconnection, selectedItem, f.PubID, fs.ExportTypeID == Enums.ExportType.ECN ? true : false, true);
        //                ResponseGroupIDList = rg.Item1;
        //                if (fs.ExportTypeID != Enums.ExportType.ECN)
        //                    ResponseGroupDescIDList = rg.Item2;
        //                StandardColumnsList = rg.Item3;
        //            }
        //            else
        //            {
        //                StandardColumnsList = Utilities.GetSelectedStandardExportColumns(clientconnection, selectedItem, f.BrandID);
        //                Tuple<List<string>, List<string>> mg = Utilities.GetSelectedMasterGroupExportColumns(clientconnection, selectedItem, f.BrandID);
        //                MasterGroupColumnList = mg.Item1;
        //                if (fs.ExportTypeID != Enums.ExportType.ECN)
        //                    MasterGroupColumnDescList = mg.Item2;
        //                SubscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(clientconnection, selectedItem);
        //            }

        //            StandardColumnsList = Utilities.GetStandardExportColumnFieldName(StandardColumnsList, f.ViewType, f.BrandID, fs.ExportTypeID == Enums.ExportType.ECN ? true : false);
        //            List<string> customColumnList = Utilities.GetSelectedCustomExportColumns(selectedItem);

        //            if (f.ViewType == Enums.ViewType.ProductView)
        //            {
        //                PubIDs = f.PubID.ToString().Split(',').Select(int.Parse).ToList();
        //                dtSubscription = Subscriber.GetProductDimensionSubscriberData(clientconnection, Queries, StandardColumnsList, PubIDs, ResponseGroupIDList, ResponseGroupDescIDList, PubSubscriptionsExtMapperValueList, customColumnList, f.BrandID, 0);
        //            }
        //            else
        //            {
        //                if (fc.Count == 1)
        //                {
        //                    var field = fc[0].Fields.Find(x => x.Group.ToUpper() == "OPENCRITERIA" && x.SearchCondition.ToUpper() == "SEARCH SELECTED PRODUCTS");

        //                    if (field != null)
        //                        PubIDs = fc[0].Fields.Find(x => x.Name.ToUpper() == "PRODUCT").Values.Split(',').Select(int.Parse).ToList();
        //                }

        //                dtSubscription = Subscriber.GetSubscriberData(clientconnection, Queries, StandardColumnsList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, f.BrandID, PubIDs, f.ViewType == Enums.ViewType.RecencyView ? true : false, 0);
        //            }

        //            if (fs.ExportTypeID == Enums.ExportType.FTP)
        //            {
        //                //sort columns
        //                List<FilterExportField> fefName = FilterExportField.getDisplayName(clientconnection, filterscheduleID);
        //                string[] columnsOrder;
        //                int i = 0;

        //                if (!fefName.Exists(x => x.ExportColumn.ToUpper() == "SUBSCRIPTIONID"))
        //                {
        //                    columnsOrder = new string[fefName.Count + 1];
        //                    columnsOrder[0] = "SubscriptionID";
        //                    i = 1;
        //                }
        //                else
        //                {
        //                    columnsOrder = new string[fefName.Count];
        //                }

        //                foreach (FilterExportField e in fefName)
        //                {

        //                    if (f.ViewType == Enums.ViewType.ProductView)
        //                    {
        //                        switch (e.DisplayName.ToUpper())
        //                        {
        //                            case "ADDRESS1":
        //                                columnsOrder[i] = "Address";
        //                                break;
        //                            case "REGIONCODE":
        //                                columnsOrder[i] = "State";
        //                                break;
        //                            case "ZIPCODE":
        //                                columnsOrder[i] = "Zip";
        //                                break;
        //                            case "PUBTRANSACTIONDATE":
        //                                columnsOrder[i] = "TransactionDate";
        //                                break;
        //                            case "QUALIFICATIONDATE":
        //                                columnsOrder[i] = "QDate";
        //                                break;
        //                            default:
        //                                columnsOrder[i] = e.DisplayName;
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (e.DisplayName.ToUpper())
        //                        {
        //                            case "FNAME":
        //                                columnsOrder[i] = "FirstName";
        //                                break;
        //                            case "LNAME":
        //                                columnsOrder[i] = "LastName";
        //                                break;
        //                            case "ISLATLONVALID":
        //                                columnsOrder[i] = "GeoLocated";
        //                                break;
        //                            default:
        //                                columnsOrder[i] = e.DisplayName;
        //                                break;
        //                        }
        //                    }
        //                    i++;
        //                }

        //                for (int j = 0; j < columnsOrder.Length; j++)
        //                {
        //                    dtSubscription.Columns[columnsOrder[j]].SetOrdinal(j);
        //                }

        //                dtSubscription = (DataTable) ProfileFieldMask.MaskData(clientconnection, dtSubscription, user);

        //                if (!fefName.Exists(x => x.ExportColumn.ToUpper() == "SUBSCRIPTIONID"))
        //                    dtSubscription.Columns.Remove("subscriptionid");

        //                if (fs.ShowHeader)
        //                {
        //                    if (Selected_FilterNos.Any())
        //                    {
        //                        if (fs.SelectedOperation == string.Empty || fs.SelectedOperation == null)
        //                            headerText = "Operations = " + "Single" + "\r\n";
        //                        else
        //                        {
        //                            headerText = "Operations = " + fs.SelectedOperation + "\r\n";


        //                        }

        //                        int k = 0;
        //                        headerText += "Filters In";

        //                        foreach (string s in Selected_FilterNos)
        //                        {
        //                            List<Field> lfield = fc.SingleOrDefault(f1 => f1.FilterNo.ToString() == s).Fields;

        //                            if (k > 0)
        //                                headerText += "\r\n";

        //                            foreach (Field f1 in lfield)
        //                            {
        //                                headerText += "\r\n";
        //                                headerText += f1.Name + " = " + f1.Text;
        //                                headerText += f1.Name == "Adhoc" || f1.Name == "Open Activity" || f1.Name == "Click Activity" || f1.Name == "Open Email Sent Date" || f1.Name == "Click Email Sent Date" || f1.Name == "Visit Activity" ? f1.Name == "Open Activity" || f1.Name == "Click Activity" || f1.Name == "Open Email Sent Date" || f1.Name == "Click Email Sent Date" || f1.Name == "Visit Activity" ? f1.SearchCondition + " - " + f1.Values : " - " + f1.SearchCondition + " - " + f1.Values : "";
        //                            }
        //                            k++;
        //                        }

        //                        if (Suppressed_FilterNos.Any())
        //                        {
        //                            headerText += "\r\n";

        //                            if (!(fs.SuppressedOperation == string.Empty || fs.SuppressedOperation == null))
        //                            {
        //                                headerText += "\r\n" + "Operations Not In = " + fs.SuppressedOperation + "\r\n";
        //                            }

        //                            headerText += "Filters NotIn";
        //                            int l = 0;

        //                            foreach (string s in Suppressed_FilterNos)
        //                            {
        //                                List<Field> lfield = fc.SingleOrDefault(f2 => f2.FilterNo.ToString() == s).Fields;

        //                                if (l > 0)
        //                                    headerText += "\r\n";

        //                                foreach (Field f2 in lfield)
        //                                {
        //                                    headerText += "\r\n";
        //                                    headerText += f2.Name + " = " + f2.Text;
        //                                    headerText += f2.Name == "Adhoc" || f2.Name == "Open Activity" || f2.Name == "Click Activity" || f2.Name == "Open Email Sent Date" || f2.Name == "Click Email Sent Date" || f2.Name == "Visit Activity" ? f2.Name == "Open Activity" || f2.Name == "Click Activity" || f2.Name == "Open Email Sent Date" || f2.Name == "Click Email Sent Date" || f2.Name == "Visit Activity" ? f2.SearchCondition + " - " + f2.Values : " - " + f2.SearchCondition + " - " + f2.Values : "";
        //                                }
        //                                l++;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return Tuple.Create(dtSubscription, headerText, dt, IsDataMaskError);
        //}
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.FilterSchedule filterSchedule)
        {
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterSchedule.FilterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterSchedule.FilterID));
            cmd.Parameters.Add(new SqlParameter("@ExportTypeID", filterSchedule.ExportTypeID));
            cmd.Parameters.Add(new SqlParameter("@IsRecurring", filterSchedule.IsRecurring));
            cmd.Parameters.Add(new SqlParameter("@RecurrenceTypeID", (object) filterSchedule.RecurrenceTypeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartDate", filterSchedule.StartDate));
            cmd.Parameters.Add(new SqlParameter("@StartTime", filterSchedule.StartTime));
            cmd.Parameters.Add(new SqlParameter("@EndDate", (object) filterSchedule.EndDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunSunday", filterSchedule.RunSunday));
            cmd.Parameters.Add(new SqlParameter("@RunMonday", filterSchedule.RunMonday));
            cmd.Parameters.Add(new SqlParameter("@RunTuesday", filterSchedule.RunTuesday));
            cmd.Parameters.Add(new SqlParameter("@RunWednesday", filterSchedule.RunWednesday));
            cmd.Parameters.Add(new SqlParameter("@RunThursday", filterSchedule.RunThursday));
            cmd.Parameters.Add(new SqlParameter("@RunFriday", filterSchedule.RunFriday));
            cmd.Parameters.Add(new SqlParameter("@RunSaturday", filterSchedule.RunSaturday));
            cmd.Parameters.Add(new SqlParameter("@MonthScheduleDay", (object) filterSchedule.MonthScheduleDay ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MonthLastDay", filterSchedule.MonthLastDay));
            if (filterSchedule.FilterScheduleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", filterSchedule.UpdatedBy));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", filterSchedule.CreatedBy));

            cmd.Parameters.Add(new SqlParameter("@EmailNotification", filterSchedule.EmailNotification));
            cmd.Parameters.Add(new SqlParameter("@Server", (object) filterSchedule.Server ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserName", (object) filterSchedule.UserName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Password", (object) filterSchedule.Password ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Folder", (object) filterSchedule.Folder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExportFormat", (object) filterSchedule.ExportFormat ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FileName", (object) filterSchedule.FileName ?? DBNull.Value));
            //cmd.Parameters.Add(new SqlParameter("@Operation", filterSchedule.Operation));

            string fgID_Selected = string.Empty;
            string fgID_Suppressed = string.Empty;

            if (filterSchedule.FilterGroupID_Selected != null)
            {
                foreach (int i in filterSchedule.FilterGroupID_Selected)
                {
                    fgID_Selected += fgID_Selected == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            if (filterSchedule.FilterGroupID_Suppressed != null)
            {
                foreach (int i in filterSchedule.FilterGroupID_Suppressed)
                {
                    fgID_Suppressed += fgID_Suppressed == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Selected", fgID_Selected));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID_Suppressed", fgID_Suppressed));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object) filterSchedule.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FolderID", (object) filterSchedule.FolderID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object) filterSchedule.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ShowHeader", filterSchedule.ShowHeader));
            cmd.Parameters.Add(new SqlParameter("@AppendDateTimeToFileName", filterSchedule.AppendDateTimeToFileName));
            cmd.Parameters.Add(new SqlParameter("@ExportName", filterSchedule.ExportName));
            cmd.Parameters.Add(new SqlParameter("@ExportNotes", (object) filterSchedule.ExportNotes ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", (object) filterSchedule.FilterSegmentationID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SelectedOperation", (object) filterSchedule.SelectedOperation ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SuppressedOperation", (object) filterSchedule.SuppressedOperation ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterScheduleID, int userID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterSchedule_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", filterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
