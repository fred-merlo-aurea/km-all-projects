
using System;

namespace KMWeb.Services
{
    public class UserIdentificationData
    {
        const string user1Field = "user1";
        const string user2Field = "user2";
        const string user3Field = "user3";
        const string user4Field = "user4";
        const string user5Field = "user5";
        const string user6Field = "user6";

        public string UdfValue { get; private set; }

        public string User1 { get; private set; }

        public string User2 { get; private set; }

        public string User3 { get; private set; }

        public string User4 { get; private set; }

        public string User5 { get; private set; }

        public string User6 { get; private set; }

        public int UdfId { get; private set; }

        public UserIdentificationData()
        {
            UdfId = 0;
            UdfValue = String.Empty;
            User1 = String.Empty;
            User2 = String.Empty;
            User3 = String.Empty;
            User4 = String.Empty;
            User5 = String.Empty;
            User6 = String.Empty;
        }

        public void Initialize(string other, string otherIdentification)
        {
            var udfId = 0;
            if (Int32.TryParse(otherIdentification, out udfId))
            {
                UdfId = udfId;
                UdfValue = other;
            }
            else
            {
                User1 = otherIdentification.Equals(user1Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
                User2 = otherIdentification.Equals(user2Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
                User3 = otherIdentification.Equals(user3Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
                User4 = otherIdentification.Equals(user4Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
                User5 = otherIdentification.Equals(user5Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
                User6 = otherIdentification.Equals(user6Field, StringComparison.OrdinalIgnoreCase) ?
                    other : String.Empty;
            }
        }
    }
}