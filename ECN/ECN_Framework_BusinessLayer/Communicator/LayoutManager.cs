using System;
using System.Collections.Generic;
using System.Data;
using DataLayerCommunicator = ECN_Framework_DataLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LayoutManager : ILayoutManager
    {
        public bool Exists(int layoutId, int customerId)
        {
            return Layout.Exists(layoutId, customerId);
        }

        public bool IsArchived(int layoutId, int customerId)
        {
            return Layout.IsArchived(layoutId, customerId);
        }

        public bool IsValidated(int layoutId, int customerId)
        {
            return Layout.IsValidated(layoutId, customerId);
        }

        public DataTable GetByLayoutID(int layoutID, int customerID, int baseChannelID)
        {
            return DataLayerCommunicator.Layout.GetByLayoutID(layoutID, customerID, baseChannelID);
        }

        public EntitiesCommunicator.Layout GetByLayoutID(int layoutID)
        {
            return DataLayerCommunicator.Layout.GetByLayoutID(layoutID);
        }

        public IList<EntitiesCommunicator.Layout> GetByCustomerID(int customerID)
        {
            return DataLayerCommunicator.Layout.GetByCustomerID(customerID);
        }

        public IList<EntitiesCommunicator.Layout> GetByLayoutSearch(string name, int? folderID, int customerID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo, bool? archived = default(bool?))
        {
            return DataLayerCommunicator.Layout.GetByLayoutSearch(name, folderID, customerID, userID, updatedDateFrom, updatedDateTo, archived);
        }

        public IList<EntitiesCommunicator.Layout> GetByFolderIDCustomerID(int folderID, int CustomerID)
        {
            return DataLayerCommunicator.Layout.GetByFolderIDCustomerID(folderID, CustomerID);
        }
    }
}