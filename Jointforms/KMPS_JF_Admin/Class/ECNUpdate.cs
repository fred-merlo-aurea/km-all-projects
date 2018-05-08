using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMPS_JF_Setup
{
    public class ECNUpdate
    {

        public static int SaveGroup(int groupID, string groupName, string groupdesc, int CustomerID)
        {
            int ECNGroupID = 0;

            groupName = DataFunctions.CleanString(groupName.Trim());
            groupdesc = DataFunctions.CleanString(groupdesc.Trim());

            string sqlcheck =
                " SELECT COUNT(groupID) FROM Groups WHERE GroupName='" + groupName + "' AND CustomerID=" + CustomerID;
            int alreadyexist = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlcheck));

            if (alreadyexist == 0)
            {
                string sqlquery =
                    " INSERT INTO Groups ( " +
                    " GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder" +
                    " ) VALUES ( " + " '" + groupName + "', '" + groupdesc + "', " + CustomerID + ", NULL, 'customer' , 0);select @@IDENTITY ";

                ECNGroupID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery).ToString());

            }
            else
            {
                throw new Exception("Publication group already exists. ");
            }

            return ECNGroupID;
        }

        public static int SaveSmartForm(int groupID, string groupName)
        {
           string sqlquery =
                " INSERT INTO SmartFormsHistory (" +
                " GroupID, SmartFormName, SmartFormHtml, SmartFormFields, Response_UserMsgSubject, Response_UserMsgBody," +
                " Response_UserScreen, Response_FromEmail, Response_AdminEmail, Response_AdminMsgSubject, Response_AdminMsgBody," +
                " CreatedDate, UpdatedDate " +
                " ) VALUES (" + groupID + ", '" + DataFunctions.CleanString(groupName.Trim()) + "', '', '', '', '','', '', '', '', '', getdate(), getdate()); select @@IDENTITY ";

            return Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery).ToString());

        }

        public static int SaveUDF(int gdfID, int groupID, string shortname, string longname, bool IsPublic)
        {
            shortname = DataFunctions.CleanString(shortname.Trim());
            longname = DataFunctions.CleanString(longname.Trim());

            string sqlcheck = "SELECT COUNT(groupdatafieldsID) FROM GROUPDATAFIELDS WHERE replace(shortname,' ', '_') ='" + shortname.Replace(" ", "_") + "' AND  IsDeleted=0 and groupID =" + groupID + " and groupdatafieldsID <> " + gdfID;
            int alreadyexist = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlcheck));

            if (alreadyexist == 0)
            {
                string sqlquery = string.Empty;

                if (gdfID == 0)
                    sqlquery = " INSERT INTO GroupDataFields ( GroupID, ShortName, LongName, IsPublic, IsDeleted) VALUES ( " + groupID + ", '" + shortname.Replace(" ", "_") + "', '" + longname + "'," + (IsPublic ? "'Y'" : "'N'") + ",0);select @@IDENTITY ";
                else
                    sqlquery = " UPDATE GroupDataFields set ShortName = '" + shortname.Replace(" ", "_") + "', LongName = '" + longname + "', IsPublic = " + (IsPublic ? "'Y'" : "'N'") + " where groupdatafieldsID = " + gdfID + ";select " + gdfID.ToString();
                
                return Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery).ToString());

            }
            else
            {
                throw new Exception("UDF already exists. ");
            }
        }
    }
}
