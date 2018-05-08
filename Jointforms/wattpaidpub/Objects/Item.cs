using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattpaidpub.Objects
{
    public class Item
    {
        private string _itemName;
        private string _itemAmount;
        private string _itemqty;
        private int _custID;
        private int _groupID;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; } 
        }

        public string ItemAmount
        {
            get { return _itemAmount; }
            set { _itemAmount = value; }
        }

        public string ItemQty
        {
            get { return _itemqty; }
            set { _itemqty = value; }
        }

        public int CustID
        {
            get { return _custID; }
            set { _custID = value; }
        }

        public int GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }
    }
}
