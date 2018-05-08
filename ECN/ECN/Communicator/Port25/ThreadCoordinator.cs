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

    public class ThreadCoordinator
    {
        private int _blastID;
        private string _smtpserver;
        private int _smtpport;
        private bool _IsTestBlast = false;
        private int _connectionsPerServer;
        private int _threadsPerConnection;

        private ArrayList _threads;
        private RecipientProvider _recipientProvider;
        private ArrayList _port25Connections;

        public ThreadCoordinator(int BlastID, bool IsTestBlast, string SMTPServer, int SMTPPort)
        {
            _blastID = BlastID;
            _IsTestBlast = IsTestBlast;
            _smtpserver = SMTPServer;
            _smtpport = SMTPPort;
            _threadsPerConnection = 1;
            _connectionsPerServer = 1;

            _threads = new ArrayList();
            _port25Connections = new ArrayList();
        }

        public int ThreadsPerConnection
        {
            get
            {
                return _threadsPerConnection;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(
                        "Must have at least one thread per connection");
                }
                _threadsPerConnection = value;
            }
        }

        public int ConnectionsPerServer
        {
            get
            {
                return _connectionsPerServer;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(
                        "Must have at least one connection per server");
                }
                _connectionsPerServer = value;
            }
        }

        public RecipientProvider RecipientProvider
        {
            set
            {
                _recipientProvider = value;
            }
            get
            {
                return _recipientProvider;
            }
        }

        public void RunThreads()
        {
            try
            {
                StartThreads();
            }
            finally
            {
                WaitForAllThreads();
            }
        }


        private void StartThreads()
        {
            for (int c = 0; c < _connectionsPerServer; c++)
            {
                Connection _port25Connection = new Connection(_smtpserver, _smtpport);
                _port25Connections.Add(_port25Connection);

                for (int t = 0; t < _threadsPerConnection; t++)
                {
                    SendThread st = new SendThread(_blastID, _IsTestBlast, _port25Connection, _recipientProvider);
                    Thread thread = new Thread(new ThreadStart(st.Run));
                    _threads.Add(thread);
                    thread.Start();
                }
            }
        }


        private void WaitForAllThreads()
        {
            foreach (Thread t in _threads)
            {
                t.Join();
            }
        }

        private void CloseAllConnections()
        {
            foreach (Connection c in _port25Connections)
            {
                c.Close();
            }
        }
    }

}