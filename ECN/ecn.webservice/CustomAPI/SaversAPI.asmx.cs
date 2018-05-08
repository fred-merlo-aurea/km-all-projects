using System;
using System.Collections.Generic;
using System.Web.Services;

namespace ecn.webservice.CustomAPI
{
    /// <summary>
    /// Summary description for SaversAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SaversAPI : System.Web.Services.WebService
    {
        [WebMethod(Description = 
            "Will create a filter for the specified GroupID.  " +
            "ZipCodes XML string must be in this format: " +
            "&lt;ZipCodes&gt;&lt;ZipCode&gt;55447&lt;/ZipCode&gt;&lt;ZipCode&gt;55388&gt;/ZipCode&gt;&lt;/ZipCodes&gt;")]
        public string CreateWeeklySolicitationFilter(
            string accesskey, 
            int groupId, 
            string solicitationStartDate, 
            string solicitationEndDate, 
            string zipCodes)
        {
            var filterCreator = new FilterCreator();
            var result = filterCreator.CreateWeeklySolicitationFilter(
                accesskey, 
                groupId, 
                solicitationStartDate,
                solicitationEndDate, 
                zipCodes);
            return result;
        }

        private string FormatECNException(ECN_Framework_Common.Objects.ECNException ex)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (ECN_Framework_Common.Objects.ECNError e in ex.ErrorList)
            {
                sb.AppendLine("Error: " + e.ErrorMessage);
            }

            sb.AppendLine(KM.Common.StringFunctions.FormatException(ex));

            return sb.ToString();
        }
        #region Create Filter Details
        private string CreateFilterCondition(int filterGroupID, List<string> zipCodeList, KMPlatform.Entity.User myUser, int customerID)
        {
            string result = string.Empty;
            foreach(string zip in zipCodeList)
            {
                //sqlQuery =
                //    " INSERT INTO FilterCondition (" +
                //        " FilterGroupID, SortOrder, Field, FieldType, Comparator, CompareValue, NotComparator, DatePart, CreatedUserID, CreatedDate, IsDeleted" +
                //        " ) VALUES ( " + filterGroupID + ", 1, 'Zip', 'varchar', 'contains', '" + zip + "', 0, 'full'," + myUser.UserID + ", '" + DateTime.Now.ToString() + "', 0)";

                try
                {
                    ECN_Framework_Entities.Communicator.FilterCondition myFC = new ECN_Framework_Entities.Communicator.FilterCondition();
                    myFC.Comparator = "contains";
                    myFC.CompareValue = zip;
                    myFC.CreatedDate = DateTime.Now;
                    myFC.CreatedUserID = myUser.UserID;
                    myFC.CustomerID = customerID;
                    myFC.DatePart = "full";
                    myFC.Field = "Zip";
                    myFC.FieldType = "String";
                    myFC.FilterGroupID = filterGroupID;
                    myFC.IsDeleted = false;
                    myFC.NotComparator = 0;
                    myFC.SortOrder = 1;
                    myFC.UpdatedDate = DateTime.Now;
                    myFC.UpdatedUserID = myUser.UserID;

                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.FilterCondition.Validate(myFC);
                    }
                    catch (ECN_Framework_Common.Objects.ECNException ex)
                    {
                        string error = "FilterCondition validation failed: " + FormatECNException(ex);
                        return error;
                    }

                    int fcID = ECN_Framework_BusinessLayer.Communicator.FilterCondition.Save(myFC, myUser);
                    if (fcID > 0)
                        result = "Success";
                    else
                        result = "Failure";
                    //ECN_Framework_DataLayer.DataFunctions.Execute(sqlQuery, ecnCommDB);
                }
                catch (Exception ex)
                {
                    string error = KM.Common.StringFunctions.FormatException(ex);
                    return error;
                }
            }
            return result;
        }
        #endregion
    }
}
