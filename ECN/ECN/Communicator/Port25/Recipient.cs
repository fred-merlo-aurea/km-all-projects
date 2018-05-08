using System;
using System.Text;

using ecn.common.classes;

namespace ecn.communicator.classes.Port25
{
    public class Recipient
    {
        public int EmailID { get; set; }
        public string VMTA { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }


        public Recipient(int EmailID, string From, string To, string VMTA, string Message)
        {
            this.EmailID = EmailID;
            this.From = From;
            this.To = To;
            this.VMTA = VMTA;
            this.Message = Message;
        }
    }
}