using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Wizard
    {
        public static bool Exists(int wizardID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 WizardID from wizard where WizardID = @WizardID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@WizardID", wizardID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Wizard wizard)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Wizard_Save";
            cmd.Parameters.Add(new SqlParameter("@WizardID", (object)wizard.WizardID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsNewMessage", wizard.IsNewMessage));
            cmd.Parameters.Add(new SqlParameter("@WizardName", wizard.WizardName));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", wizard.EmailSubject));
            cmd.Parameters.Add(new SqlParameter("@FromName", wizard.FromName));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", wizard.FromEmail));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", wizard.ReplyTo));
            cmd.Parameters.Add(new SqlParameter("@GroupID", wizard.GroupID));
            cmd.Parameters.Add(new SqlParameter("@ContentID", wizard.ContentID));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", wizard.LayoutID));
            cmd.Parameters.Add(new SqlParameter("@BlastID", wizard.BlastID));
            cmd.Parameters.Add(new SqlParameter("@FilterID", wizard.FilterID));
            cmd.Parameters.Add(new SqlParameter("@StatusCode", wizard.StatusCode));
            cmd.Parameters.Add(new SqlParameter("@CompletedStep", wizard.CompletedStep));
            cmd.Parameters.Add(new SqlParameter("@CardHolderName", wizard.CardHolderName));
            cmd.Parameters.Add(new SqlParameter("@CardType", wizard.CardType));
            cmd.Parameters.Add(new SqlParameter("@CardNumber", wizard.CardNumber));
            cmd.Parameters.Add(new SqlParameter("@CardcvNumber", wizard.CardcvNumber));
            cmd.Parameters.Add(new SqlParameter("@CardExpiration", wizard.CardExpiration));
            cmd.Parameters.Add(new SqlParameter("@TransactionNo", wizard.TransactionNo));
            cmd.Parameters.Add(new SqlParameter("@MultiGroupIDs", wizard.MultiGroupIDs));
            cmd.Parameters.Add(new SqlParameter("@SuppressionGroupIDs", wizard.SuppressionGroupIDs));
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", wizard.PageWatchID));
            cmd.Parameters.Add(new SqlParameter("@BlastType", wizard.BlastType));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject2", wizard.EmailSubject2));           
            cmd.Parameters.Add(new SqlParameter("@ContentID2", wizard.ContentID2));
            cmd.Parameters.Add(new SqlParameter("@LayoutID2", wizard.LayoutID2));
            cmd.Parameters.Add(new SqlParameter("@SampleWizardID", wizard.SampleWizardID));
            cmd.Parameters.Add(new SqlParameter("@RefBlastID", wizard.RefBlastID));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", wizard.CampaignID));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", wizard.DynamicFromEmail));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", wizard.DynamicFromName));
            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", wizard.DynamicReplyToEmail));

            if (wizard.WizardID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)wizard.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)wizard.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())); ;
        }
    }
}
