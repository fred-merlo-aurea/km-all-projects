using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class CssItem
    {
        public CssItem()
        {
            ListElement = null;
            ListClass = null;
            ListId = null;
            HasElement = false;
            HasClass = false;
            HasId = false;
        }
        public CssItem(List<Element> listElem)
        {
            ListElement = listElem;
            ListClass = null;
            ListId = null;
            HasElement = true;
            HasClass = false;
            HasId = false;
        }
        public CssItem(List<Element> listElem, List<Class> listClass)
        {
            ListElement = listElem;
            ListClass = listClass;
            ListId = null;
            HasElement = true;
            HasClass = true;
            HasId = false;
        }
        public CssItem(List<Element> listElem, List<Id> listId)
        {
            ListElement = listElem;
            ListClass = null;
            ListId = listId;
            HasElement = true;
            HasClass = false;
            HasId = true;
        }
        public CssItem(List<Element> listElem, List<Class> listClass, List<Id> listId)
        {
            ListElement = listElem;
            ListClass = listClass;
            ListId = listId;
            HasElement = true;
            HasClass = true;
            HasId = true;
        }
        public CssItem(List<Class> listClass)
        {
            ListElement = null;
            ListClass = listClass;
            ListId = null;
            HasElement = false;
            HasClass = true;
            HasId = false;
        }
        public CssItem(List<Class> listClass, List<Id> listId)
        {
            ListElement = null;
            ListClass = listClass;
            ListId = listId;
            HasElement = false;
            HasClass = true;
            HasId = true;
        }
        public CssItem(List<Id> listId)
        {
            ListElement = null;
            ListClass = null;
            ListId = listId;
            HasElement = false;
            HasClass = false;
            HasId = true;
        }

        public List<Element> ListElement { get; set; }
        public List<Class> ListClass { get; set; }
        public List<Id> ListId { get; set; }
        public bool HasElement { get; set; }
        public bool HasClass { get; set; }
        public bool HasId { get; set; }
    }
}
