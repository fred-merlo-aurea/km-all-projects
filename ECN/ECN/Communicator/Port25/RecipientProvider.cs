using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.IO;
using System.Data;

namespace ecn.communicator.classes.Port25
{
    public class RecipientProvider
    {
        private List<Recipient> RecipientQueue;
        private int _left = 0;

        public RecipientProvider()
        {
            RecipientQueue = new List<Recipient>();
        }

        public void Add(int EmailID, string From, string To, string VMTA, string Message)
        {
            lock (this)
            {
                RecipientQueue.Add(new Recipient(EmailID, From, To, VMTA, Message));
            }
        }

        public Recipient GetNext()
        {
            lock (this)
            {
                if (_left < RecipientQueue.Count)
                {
                    Recipient r = RecipientQueue[_left];
                    _left++;
                    return r;
                }
                return null;
            }
        }
    }


}