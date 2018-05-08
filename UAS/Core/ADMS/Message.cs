using System;
using System.Data;
using System.Linq;

namespace Core.ADMS
{
    public class Message
    {
        public string MessageFilePath { get; set; }

        public Message()
        {
            if (string.IsNullOrEmpty(MessageFilePath))
            {
                MessageFilePath = ADMS.BaseDirs.createDirectory(ADMS.Settings.MessageFilePath, "Messages.xml");
            }
        }
        public Message(string messageFilePath) { MessageFilePath = messageFilePath; }

        public string GetMessage(MessageTag myMessage)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(MessageFilePath);
            DataTable dt = ds.Tables[0];
            ds.Dispose();

            string msg = string.Empty;
            if (dt.DefaultView.Count > 0)
                msg = dt.DefaultView[0].Row[myMessage.ToString()].ToString();
            dt.Dispose();
            return msg;
        }

        public enum MessageTag
        {
            ImportFileReport_PlainText,
            ImportFileReport_HTML
        }
        public static MessageTag GetMessageTag(string messageTag)
        {
            return (MessageTag)System.Enum.Parse(typeof(MessageTag), messageTag, true);
        }
    }
}
