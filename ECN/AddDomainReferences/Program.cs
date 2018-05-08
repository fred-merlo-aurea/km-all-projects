using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace AddDomainReferences
{
    class Program
    {
        public static SqlConnection MyConnection = new SqlConnection(GetConnectionString());
        public static string outLog = "";
        public static StreamWriter outFile = null;
        //public static IPDomain ipDomain = null;
        public static List<IPDomain> refIPDomainList = new List<IPDomain>();

        static void Main(string[] args)
        {
            outLog = ConfigurationManager.AppSettings["OutLog"] + DateTime.Now.Date.ToString("d") + ".log";
            outLog = outLog.Replace(@"/", "");
            outFile = new StreamWriter(new FileStream(outLog, System.IO.FileMode.Append));
            try
            {
                string path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Output\\";
                if (Directory.Exists(path))
                {
                    Console.WriteLine(DateTime.Now.ToString() + ": " + Directory.GetDirectories(path).Count().ToString() + " directories to process");
                    outFile.WriteLine(DateTime.Now.ToString() + ": " + Directory.GetDirectories(path).Count().ToString() + " directories to process");
                    foreach (string dirName in Directory.GetDirectories(path))
                    {
                        Console.WriteLine(DateTime.Now.ToString() + ": Processing - " + dirName + " directory");
                        outFile.WriteLine(DateTime.Now.ToString() + ": Processing - " + dirName + " directory");
                        Console.WriteLine(DateTime.Now.ToString() + ": " + Directory.GetFiles(dirName + "\\", "*.*").Count().ToString() + " files to process");
                        outFile.WriteLine(DateTime.Now.ToString() + ": " + Directory.GetFiles(dirName + "\\", "*.*").Count().ToString() + " files to process");
                        foreach (string fileName in Directory.GetFiles(dirName + "\\", "*.*"))
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ": Processing - " + fileName + " file");
                            outFile.WriteLine(DateTime.Now.ToString() + ": Processing - " + fileName + " file");
                            ProcessFile(fileName);
                        }
                    }
                    //if (ipDomainList.Count > 0)
                    //{
                    //    string xmlList = IPDomain.CreateXML(ipDomainList);
                    //    AddToDB(xmlList);
                    //}
                    Console.WriteLine(DateTime.Now.ToString() + ": Finished Processing");
                    outFile.WriteLine(DateTime.Now.ToString() + ": Finished Processing");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error / Exception \n");
                outFile.WriteLine("General Error / Exception");
                outFile.WriteLine("Exception stack trace:  \n");
                outFile.WriteLine(ex.ToString());
            }
            outFile.Close();
        }

        private static void ProcessFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                int lineNumber = 0;
                int count = 0;
                string line = string.Empty;
                string subscriber = string.Empty;
                string domain = string.Empty;
                string message = string.Empty;
                string ip = string.Empty;
                IPDomain ipDomain = null;
                List<IPDomain> ipDomainList = new List<IPDomain>();

                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    count++;
                    switch (lineNumber)
                    {
                        case 3: //get domain
                            domain = string.Empty;
                            if (line.IndexOf("TO:") > 0)
                            {
                                subscriber = line.Substring(line.IndexOf("TO:") + 4).Trim().ToLower();
                                if ((subscriber.IndexOf("@") > 0) && (subscriber.IndexOf("@") != (subscriber.Length - 1)))
                                {
                                    try
                                    {
                                        domain = subscriber.Substring(subscriber.IndexOf("@") + 1, subscriber.Length - (subscriber.IndexOf("@") + 1));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                            break;
                        case 5: //get ip
                            lineNumber = 0;
                            ip = string.Empty;
                            if (line.Contains("250 Email Sent Successfully to server ["))
                            {
                                try
                                {
                                    line = line.Substring(line.IndexOf("250 Email Sent Successfully to server [") + 38, line.Length - (line.IndexOf("250 Email Sent Successfully to server [") + 38));
                                    ip = line.Substring(line.IndexOf("[") + 1, (line.IndexOf("]") - (line.IndexOf("[") + 1))).Trim();
                                }
                                catch (Exception)
                                {
                                }
                            }



                            if (domain != string.Empty && ip != string.Empty)
                            {
                                //AddToDB(domain, ip);
                                ipDomain = new IPDomain();
                                ipDomain.IP = ip;
                                ipDomain.Domain = domain;
                                if (!refIPDomainList.Contains(ipDomain))
                                {
                                    refIPDomainList.Add(ipDomain);
                                    ipDomainList.Add(ipDomain);
                                }
                                else
                                {
                                    string test = "test";
                                }
                            }

                            break;

                    }
                    if (count % 500 == 0)
                    {
                        Console.WriteLine(DateTime.Now.ToString() + ": Processed - " + (count / 5).ToString() + " records in file");
                        outFile.WriteLine(DateTime.Now.ToString() + ": Processed - " + (count / 5).ToString() + " records in file");
                    }
                }

                if (ipDomainList.Count > 0)
                {
                    string xmlList = IPDomain.CreateXML(ipDomainList);
                    AddToDB(xmlList);
                }

                Console.WriteLine(DateTime.Now.ToString() + ": Processed - " + count.ToString() + " lines");
                outFile.WriteLine(DateTime.Now.ToString() + ": Processed - " + count.ToString() + " lines");
            }
        }

        //private static void AddToDB(string domain, string ip)
        //{
        //    SqlCommand myCommand = new SqlCommand("IF NOT EXISTS(SELECT * FROM domainIPs WHERE domainName = @Domain AND IP = @IP) BEGIN INSERT INTO domainIPs(domainName, IP) VALUES (@Domain, @IP)END", MyConnection);
        //    myCommand.Parameters.Add(new SqlParameter("@Domain", SqlDbType.VarChar));
        //    myCommand.Parameters["@Domain"].Value = domain;
        //    myCommand.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar));
        //    myCommand.Parameters["@IP"].Value = ip;
        //    myCommand.CommandType = CommandType.Text;
        //    MyConnection.Open();
        //    try
        //    {
        //        myCommand.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        MyConnection.Close();
        //    }
        //}

        private static void AddToDB(string xmlList)
        {
            SqlCommand cmd = new SqlCommand("sp_InsertIPDomain", MyConnection);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@xmlDocument", SqlDbType.Text));
            cmd.Parameters["@xmlDocument"].Value = xmlList;

            MyConnection.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error / Exception \n");
                outFile.WriteLine("General Error / Exception");
                outFile.WriteLine("Exception stack trace:  \n");
                outFile.WriteLine(ex.ToString());
            }
            finally
            {
                MyConnection.Close();
            }
        }

        private static string GetConnectionString()
        {
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connection in connections)
            {
                if (connection.Name == "ecn5_CommunicatorConnectionString")
                {
                    return connection.ConnectionString;
                }
            }

            return "";
        }

    }

    public class IPDomain : IEquatable<IPDomain>
    {
        public IPDomain()
        {
            IP = "";
            Domain = "";
        }

        public string IP;
        public string Domain;

        public bool Equals(IPDomain other)
        {
            if (this.IP == other.IP && this.Domain == other.Domain)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string CreateXML(List<IPDomain> ipdomainList)
        {
            StringBuilder xmlList = new StringBuilder();
            xmlList.Append("<ROOT>");
            foreach (IPDomain ipDomain in ipdomainList)
            {
                xmlList.Append(String.Format("<ENTRY IP=\"{0}\" Domain=\"{1}\" />", ipDomain.IP, ipDomain.Domain));
            }

            xmlList.Append("</ROOT>");

            return xmlList.ToString();
        }
    }

}
