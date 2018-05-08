using System;
using System.Collections.Generic;
using System.Data;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ILayoutManager
    {
        bool Exists(int layoutId, int customerId);
        bool IsArchived(int layoutId, int customerId);
        bool IsValidated(int layoutId, int customerId);
        DataTable GetByLayoutID(int layoutID, int customerID, int baseChannelID);
        EntitiesCommunicator.Layout GetByLayoutID(int layoutID);
        IList<EntitiesCommunicator.Layout> GetByCustomerID(int customerID);
        IList<EntitiesCommunicator.Layout> GetByLayoutSearch(string name, int? folderID, int customerID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo, bool? archived = null);
        IList<EntitiesCommunicator.Layout> GetByFolderIDCustomerID(int folderID, int CustomerID);
    }
}