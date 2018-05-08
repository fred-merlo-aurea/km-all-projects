using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace KMPS.MD.Objects
{
    public class EntryItem
    {
        private int _ItemID;
        private string _ItemType;
        private string _GroupID;
        private string _GroupTitle;
        private string _EntryID;
        private string _EntryTitle;

        public int ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        public string ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }

        public string GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }

        public string GroupTitle
        {
            get { return _GroupTitle; }
            set { _GroupTitle = value; }
        }

        public string EntryID
        {
            get { return _EntryID; }
            set { _EntryID = value; }
        }

        public string EntryTitle
        {
            get { return _EntryTitle; }
            set { _EntryTitle = value; }
        }               
        

        public EntryItem()
        { 
        }

        public EntryItem(int ItemID, string ItemType, string GroupID, string GroupTitle, string EntryID, string EntryTitle)
        {
            this._ItemID = ItemID;
            this._ItemType = ItemType;
            this._GroupID = GroupID;
            this._GroupTitle = GroupTitle;
            this._EntryID = EntryID;
            this._EntryTitle = EntryTitle;
        }
    }
}
