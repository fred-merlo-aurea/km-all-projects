using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.Salesforce.Controls
{
    public partial class Message : System.Web.UI.UserControl
    {
        public enum Message_Icon
        {
            info,
            error
        }
        private Message_Icon _messageIcon;
        public Message_Icon MessageIcon
        {
            get
            {
                return _messageIcon;
            }
            set
            {
                _messageIcon = value;
            }
        }
        private string _text;
        public string MessageText
        {
            get
            {
                return _text;//Session["MessageText"].ToString();
            }
            set
            {
                _text = value;//Session["MessageText"] = value;
            }
        }
        private string _title;
        public string MessageTitle
        {
            get
            {
                return _title;//Session["MessageText"].ToString();
            }
            set
            {
                _title = value;//Session["MessageText"] = value;
            }
        }
        public void Show(string message, string title, Message_Icon msgIcon)
        {
            MessageText = message;
            MessageTitle = title;
            MessageIcon = msgIcon;

            string icon = "<img id=\"imgIcon\" alt=\"messageIcon\" src=";
            if (MessageIcon.Equals(Message_Icon.error))
                icon += "\"http://images.ecn5.com/Images/errorEx.jpg\" /> ";
            else if (MessageIcon.Equals(Message_Icon.info))
                icon += "\"http://images.ecn5.com/Images/Info_24x24.png\" /> ";
            else
                icon = string.Empty;

            MessageText = "<table><tr><td>" + icon + "</td><td style=\"font-size: medium; padding-left: 15px;\">" + MessageText + "</td></tr></table>";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + MessageText + "','" + MessageTitle + "');", true);


        }

        public void Show(string message, string title)
        {
            MessageText = message;
            MessageTitle = title;

            string icon = "<img id=\"imgIcon\" alt=\"messageIcon\" src=";
            if (MessageIcon.Equals(Message_Icon.error))
                icon += "\"http://images.ecn5.com/Images/errorEx.jpg\" /> ";
            else if (MessageIcon.Equals(Message_Icon.info))
                icon += "\"http://images.ecn5.com/Images/Info_24x24.png\" /> ";
            else
                icon = string.Empty;

            MessageText = "<table><tr><td>" + icon + "</td><td style=\"font-size: medium; padding-left: 15px;\">" + MessageText + "</td></tr></table>";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + MessageText + "','" + MessageTitle + "');", true);

        }
        public void Show()
        {
            string icon = "<img id=\"imgIcon\" alt=\"messageIcon\" src=";
            if (MessageIcon.Equals(Message_Icon.error))
                icon += "\"http://images.ecn5.com/Images/errorEx.jpg\" /> ";
            else
                icon += "\"http://images.ecn5.com/Images/Info_24x24.png\" /> ";

            MessageText = "<table><tr><td>" + icon + "</td><td style=\"font-size: medium; padding-left: 15px;\">" + MessageText + "</td></tr></table>";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + MessageText + "','" + MessageTitle + "');", true);
        }
    }
}