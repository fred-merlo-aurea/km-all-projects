using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.FormDesigner
{
    [Serializable]
    public class Form
    {
        //static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.FORMSDESIGNER;
        //static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Groups;

        public static bool ActiveByGroup(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.FormDesigner.Form.ActiveByGroup(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActiveByGDF(int groupDataFieldsID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.FormDesigner.Form.ActiveByGDF(groupDataFieldsID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static DataSet GetBySearchStringPaging(int BaseChannelID, int CustomerID, string FormType, string FormStatus, string FormName, string SearchCriteria, int Active, int PageNumber, int PageSize, string sortDirection, string sortColumn)
        {
            DataSet dsProfilesList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                 dsProfilesList = ECN_Framework_DataLayer.FormDesigner.Form.GetBySearchStringPaging(BaseChannelID, CustomerID, FormType, FormStatus, FormName, SearchCriteria, Active, PageNumber, PageSize, sortDirection, sortColumn);
                scope.Complete();
            }

            return dsProfilesList;
        }
        public static ECN_Framework_Entities.FormDesigner.Form GetByID(int BaseChannelID, int Form_Seq_ID)
        {
            ECN_Framework_Entities.FormDesigner.Form retItem = new ECN_Framework_Entities.FormDesigner.Form();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.FormDesigner.Form.GetByForm_Seq_ID(BaseChannelID, Form_Seq_ID);
                scope.Complete();
            }
            return retItem;
        }
        public static ECN_Framework_Entities.FormDesigner.Form GetByFormID_NoAccessCheck(int Form_Seq_ID)
        {
            ECN_Framework_Entities.FormDesigner.Form retItem = new ECN_Framework_Entities.FormDesigner.Form();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.FormDesigner.Form.GetByFormID_NoAccessCheck(Form_Seq_ID);
                scope.Complete();
            }
            return retItem;
        }
    }
}
