using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.IO;
using System.Data;
using ecn.communicator.classes;
using System.Data.SqlClient;
using ecn.common.classes;
using port25.pmta.api.submitter;

namespace ecn.communicator.classes.Port25
{

    public class SendThread
    {
        private int _blastID;
        private bool _IsTestBlast = false;
        private Connection _con;
        private RecipientProvider _recipientProvider;

        public SendThread(int BlastID, bool IsTestBlast, Connection con, RecipientProvider provider)
        {
            _blastID = BlastID;
            _IsTestBlast = IsTestBlast;
            _con = con;
            _recipientProvider = provider;
        }


        public void Run()
        {
            Recipient r;
            while ((r = _recipientProvider.GetNext()) != null)
            {
                Message msg = MakeMessage(r);
                _con.Submit(msg);

                if (r.EmailID > 0)
                {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed) values (@EmailID, @BlastID, @ActionTypeCode, getdate(), 'Send', null, 'n')";
                cmd.Parameters.Add(new SqlParameter("@EmailID", r.EmailID));
                cmd.Parameters.Add(new SqlParameter("@BlastID", _blastID));
                cmd.Parameters.Add(new SqlParameter("@ActionTypeCode", _IsTestBlast ? "testsend" : "send"));

                DataFunctions.Execute(cmd);
                cmd.Dispose();
                }
            }
        }

        // Composes a multi-part message with a plain text and an HTML part.
        // The HTML part comes with an attached picture.
        // Uses PowerMTA's mail merge feature to personalize the message.
        private Message MakeMessage(Recipient r)
        {
            Message msg = new Message(r.From);
            msg.VirtualMTA = r.VMTA;

            msg.AddRecipient(new port25.pmta.api.submitter.Recipient(r.To));

            msg.AddData(r.Message);
            return msg;
        }

        private void AddData(Message msg, String data)
        {
            // PowerMTA encodes strings in UTF-8.  If you need a different encoding,
            // use something like this:
            //
            //      msg.AddData(System.Text.Encoding.X.GetBytes(headers));
            //
            // where X is the desired encoding.  You must also set the body part's
            // character set accordingly. 
            msg.AddData(data);
        }

    }
}
