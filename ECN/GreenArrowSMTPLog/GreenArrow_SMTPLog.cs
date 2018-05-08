using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Data;


namespace GreenArrow_SMTPLog
{
    class GreenArrow_SMTPLog
    {
        public static string outLog = "";
        public static StreamWriter outFile = null;
        public static string BPALog = "";
        public static StreamWriter BPAFile = null;

        static void Main(string[] args)
        {

            outLog = ConfigurationManager.AppSettings["OutLog"] + DateTime.Now.Date.ToString("d") + ".log";
            outLog = outLog.Replace(@"/", "");
            outFile = new StreamWriter(new FileStream(outLog, System.IO.FileMode.Append));
            try
            {
                //get blasts for 7 days prior
                Console.WriteLine(DateTime.Now.ToString() + ": Getting Blasts");
                outFile.WriteLine(DateTime.Now.ToString() + ": Getting Blasts");
                List<Models.Blasts> blastList = DAL.Blasts.GetBlastsForCustomer();
                if (blastList.Count > 0)
                {
                    Console.WriteLine(DateTime.Now.ToString() + ": " + blastList.Count.ToString() + " Blasts to process");
                    outFile.WriteLine(DateTime.Now.ToString() + ": " + blastList.Count.ToString() + " Blasts to process");
                    bool sendIssueAlert = false;
                    foreach (Models.Blasts blast in blastList)
                    {
                        Console.WriteLine(DateTime.Now.ToString() + ": Current Customer - " + Convert.ToInt32(blast.CustomerID).ToString() + ", Current Blast - " + Convert.ToInt32(blast.BlastID).ToString());
                        outFile.WriteLine(DateTime.Now.ToString() + ": Current Customer - " + Convert.ToInt32(blast.CustomerID).ToString() + ", Current Blast - " + Convert.ToInt32(blast.BlastID).ToString());
                        BPAFile = Helper.SetupBPAAudit(blast.CustomerName, blast);
                        if (BPAFile != null)
                        {
                            //get email activity for this blast
                            Console.WriteLine(DateTime.Now.ToString() + ": Getting EAL");
                            outFile.WriteLine(DateTime.Now.ToString() + ": Getting EAL");
                            List<Models.EmailActivityLog> ealList = DAL.EmailActivityLog.GetEmailsForBlast(Convert.ToInt32(blast.BlastID));
                            //get logfile from GA for this blast                            
                            DateTime startTime = DateTime.Now;
                            DateTime endTime = DateTime.Now;
                            string gaFile = "";
                            int retry = 0;
                            //try getting the record 3 times if necessary due to timeouts
                            while (retry <= 3)
                            {
                                try
                                {
                                    Console.WriteLine(DateTime.Now.ToString() + ": Getting Logfile");
                                    outFile.WriteLine(DateTime.Now.ToString() + ": Getting Logfile");
                                    retry++;
                                    gaFile = Helper.GetGALogFile(Convert.ToInt32(blast.BlastID), ref startTime, ref endTime);
                                    retry = 99;
                                }
                                catch (System.Net.WebException)
                                {
                                    if (retry == 3)
                                    {
                                        throw;
                                    }
                                    else
                                    {
                                        Console.WriteLine(DateTime.Now.ToString() + ": Timed Out Getting Logfile on attempt number " + retry.ToString());
                                        outFile.WriteLine(DateTime.Now.ToString() + ": Timed Out Getting Logfile on attempt number " + retry.ToString());
                                    }
                                }
                            }


                            //put logfile in datatable
                            Console.WriteLine(DateTime.Now.ToString() + ": Getting Logtable");
                            outFile.WriteLine(DateTime.Now.ToString() + ": Getting Logtable");
                            DataTable dt = Helper.GetGALogTable(gaFile);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                TimeSpan span = endTime.Subtract(startTime);

                                Console.WriteLine("It took " + span.Minutes.ToString() + " minutes and " + span.Seconds.ToString() + " seconds to get " + dt.Rows.Count.ToString() + " rows from GA");
                                outFile.WriteLine("It took " + span.Minutes.ToString() + " minutes and " + span.Seconds.ToString() + " seconds to get " + dt.Rows.Count.ToString() + " rows from GA");
                            }

                            if (ealList.Count > 0)
                            {
                                //loop thru eal and find matching record in log
                                DataRow[] foundRows;
                                string prevEmailAddress = "";
                                foreach (Models.EmailActivityLog eal in ealList)
                                {
                                    if (eal.EmailName == "t_fastriver" && eal.EmailDomain == "yahoo.co.jp")
                                    {
                                        string action = eal.ActionTypeCode;
                                    }
                                    //only need 1 record for each emailaddress as bounce, if present, will be the first record
                                    if ((eal.EmailName + "@" + eal.EmailDomain) != prevEmailAddress)
                                    {
                                        foundRows = dt.Select("EmailName = '" + eal.EmailName + "' AND EmailDomain = '" + eal.EmailDomain + "'");
                                        if (foundRows != null && foundRows.Count() > 0)
                                        {
                                            foreach (DataRow row in foundRows)
                                            {
                                                if (eal.ActionTypeCode.ToLower() == "send")
                                                {
                                                    if (row["Status"].ToString().ToLower() == "accepted")
                                                    {
                                                        Helper.LogRecord(ref BPAFile, eal, row, blast);
                                                    }
                                                    else
                                                    {
                                                        //eal says send and log says something different
                                                        Console.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") in EmailActivityLog says 'send' and GA log file says differently.");
                                                        outFile.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") in EmailActivityLog says 'send' and GA log file says differently.");
                                                        sendIssueAlert = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (row["Status"].ToString().ToLower() != "accepted")
                                                    {
                                                        Helper.LogRecord(ref BPAFile, eal, row, blast);
                                                    }
                                                    else
                                                    {
                                                        //eal says bounce and log says something different
                                                        //Console.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") in EmailActivityLog says 'bounce' and GA log file says differently.");
                                                        //outFile.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") in EmailActivityLog says 'bounce' and GA log file says differently.");
                                                        //sendIssueAlert = true;

                                                        //now we are just going to take the bounce notes from ECN
                                                        Helper.LogRecordUsingEAL(ref BPAFile, eal, row, blast);
                                                    }
                                                }

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //no log record found
                                            Console.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") not found in GA log file.");
                                            outFile.WriteLine("BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " - EmailAddress(" + eal.EmailName + "@" + eal.EmailDomain + ") not found in GA log file.");
                                            sendIssueAlert = true;
                                        }
                                    }
                                    prevEmailAddress = eal.EmailName + "@" + eal.EmailDomain;
                                }
                            }

                            BPAFile.Close();
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ": Error creating BPALog file for BlastID: " + Convert.ToInt32(blast.BlastID).ToString());
                            outFile.WriteLine(DateTime.Now.ToString() + ": Error creating BPALog file for BlastID: " + Convert.ToInt32(blast.BlastID).ToString() + " \n");
                            sendIssueAlert = true;
                        }
                    }
                    if (sendIssueAlert)
                    {
                        Helper.NotifyAdmin("Data issue in GreenArrow_SMTPLog Engine", "Error: Check daily log for details.");
                    }
                    Console.WriteLine(DateTime.Now.ToString() + ":  Finished processing");
                    outFile.WriteLine(DateTime.Now.ToString() + ":  Finished processing \n");
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + ": No Blasts to process");
                    outFile.WriteLine(DateTime.Now.ToString() + ": No Blasts to process \n");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("General Error / Exception \n");
                outFile.WriteLine("General Error / Exception");
                outFile.WriteLine("Exception stack trace:  \n");
                outFile.WriteLine(Ex.ToString());
                Helper.NotifyAdmin("Error Occurred in the GreenArrow_SMTPLog Engine", "Error: " + Ex.Message + " \n");
            }
            outFile.Close();
        }

    }
}
