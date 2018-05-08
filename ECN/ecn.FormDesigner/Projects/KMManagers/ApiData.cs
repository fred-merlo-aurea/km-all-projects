using System;
using KMEntities;

namespace KMManagers
{
    internal class ApiData : IEquatable<ApiData>
    {
        public string CustomerID;
        public string AccessKey;
        public string GroupID;
        public bool IsNewsletter;
        public Guid TokenUID;

        public ApiData(Form form)
        {
            // TODO: Complete member initialization
            CustomerID = form.CustomerID.ToString();
            AccessKey = form.CustomerAccessKey;
            GroupID = form.GroupID.ToString();
            TokenUID = form.TokenUID;
            IsNewsletter = false;
        }

        public ApiData(string customerId, string accessKey, string groupId,bool isNewsletter)
        {
            // TODO: Complete member initialization
            CustomerID = customerId;
            AccessKey = accessKey;
            GroupID = groupId;
            IsNewsletter = isNewsletter;
        }

        public string Key
        {
            get
            {
                return CustomerID + '_' + GroupID.ToString();
            }
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        #region IEquatable<ApiData> Members

        public bool Equals(ApiData other)
        {
            return Key == other.Key;
        }

        #endregion
    }
}