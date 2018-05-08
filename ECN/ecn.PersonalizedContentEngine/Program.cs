using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.common.classes;

namespace ecn.PersonalizedContentEngine
{
    class Program
    {
        private const string CommunicatorVirtualPathConfigurationKey = "Communicator_VirtualPath";

        static ConcurrentDictionary<string, int> cdBlastLinks = new ConcurrentDictionary<string, int>();
        static ConcurrentDictionary<string, int> cdUniqueLinks = new ConcurrentDictionary<string, int>();
        static ECN_Framework_Entities.Communicator.Group group;
        static ECN_Framework_Entities.Accounts.Customer customer;

        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            Console.WriteLine("Starting : " + DateTime.Now);

            var start0 = Environment.TickCount;
            var end0 = Environment.TickCount;
            bool bprocessnonstop = true;

            int processedcounts = 0;

            try
            {
                // get top 100 Dict<ID, PersonalizedContent> where isprocessed = 0 order by blastsend time asc
                start0 = Environment.TickCount;
                Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> dpc = ECN_Framework_BusinessLayer.Content.PersonalizedContent.GetNotProcessed();
                end0 = Environment.TickCount;

                Console.WriteLine(string.Format(" From DB {0} records //  {1} milliseconds.", (dpc != null ? dpc.Count : 0), (end0 - start0)));

                List<Int64> lBlastID = new List<Int64>();

                if (dpc != null)
                {
                    foreach (KeyValuePair<long, ECN_Framework_Entities.Content.PersonalizedContent> kvp in dpc)
                    {
                        if (!lBlastID.Contains(kvp.Value.BlastID))
                        {
                            lBlastID.Add(kvp.Value.BlastID);
                        }
                    }
                }
                else
                {
                    Environment.Exit(0);
                }

                foreach (Int64 blastID in lBlastID)
                {
                    start0 = Environment.TickCount;

                    Console.WriteLine(string.Format(" Starting BlastID {0}.", blastID));

                    bprocessnonstop = true;

                    cdBlastLinks.Clear();
                    cdUniqueLinks.Clear();

                    ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(int.Parse(blastID.ToString()), false);

                    group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.BlastID);
                    customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(blast.CustomerID.Value, false);
                    cdBlastLinks = ToConcurrent(ECN_Framework_BusinessLayer.Communicator.BlastLink.GetDictionaryByBlastID(blast.BlastID));
                    cdUniqueLinks = ToConcurrent(ECN_Framework_BusinessLayer.Communicator.UniqueLink.GetDictionaryByBlastID(blast.BlastID));

                    ECN_Framework.Common.ChannelCheck _ChannelCheck = new ECN_Framework.Common.ChannelCheck(blast.CustomerID.Value);

                    while (bprocessnonstop)
                    {
                        #region NON Stop Processing if there are more records to be processed

                        if (dpc == null)
                        {
                            end0 = Environment.TickCount;
                            Console.WriteLine(string.Format(" Processed {0} to {1} records in  {2} ms.", processedcounts, processedcounts + 100, (end0 - start0)));

                            processedcounts = processedcounts + 100;

                            start0 = Environment.TickCount;
                            dpc = ECN_Framework_BusinessLayer.Content.PersonalizedContent.GetNotProcessed();

                            end0 = Environment.TickCount;
                            Console.WriteLine(string.Format(" From DB {0} records //  {1} ms.", dpc != null ? dpc.Count : 0, (end0 - start0)));

                            start0 = Environment.TickCount;
                        }

                        if (dpc == null || dpc.Count == 0)
                        {
                            bprocessnonstop = false;
                        }
                        else
                        {
                            ParallelOptions options = new ParallelOptions();

                            try
                            {
                                options.MaxDegreeOfParallelism = int.Parse(ConfigurationManager.AppSettings["ParallelThreads"].ToString());
                            }
                            catch
                            {
                                options.MaxDegreeOfParallelism = 1;
                            }

                            Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> dpcbyBlastID = dpc.Where(kvp => kvp.Value.BlastID == blastID).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                            if (dpcbyBlastID == null || dpcbyBlastID.Count == 0)
                            {
                                bprocessnonstop = false;
                            }
                            else
                            {
                                Parallel.ForEach(dpcbyBlastID, options, kvp =>
                                {
                                    try
                                    {
                                        #region Parallel
                                        var tickstart1 = Environment.TickCount;
                                        var tickstart2 = Environment.TickCount;
                                        var tickstart3 = Environment.TickCount;
                                        var tickstart4 = Environment.TickCount;
                                        var tickend1 = Environment.TickCount;
                                        var tickend2 = Environment.TickCount;
                                        var tickend3 = Environment.TickCount;
                                        var tickend4 = Environment.TickCount;

                                        //Personalize EmailMessage for each email & send to SMTP writer to inject into MTA
                                        ECN_Framework_Entities.Content.PersonalizedContent pc = kvp.Value;

                                        StringBuilder PersonalizedContentHTML = new StringBuilder();
                                        StringBuilder PersonalizedContentText = new StringBuilder();

                                        tickstart1 = Environment.TickCount;

                                        if (blast.EnableCacheBuster.HasValue && blast.EnableCacheBuster.Value)
                                        {
                                            PersonalizedContentHTML.Append(TemplateFunctions.imgRewriter(pc.HTMLContent, blast.BlastID));
                                            PersonalizedContentText.Append(TemplateFunctions.imgRewriter(pc.TEXTContent, blast.BlastID));
                                        }
                                        else
                                        {
                                            PersonalizedContentHTML.Append(pc.HTMLContent);
                                            PersonalizedContentText.Append(pc.TEXTContent);
                                        }
                                        tickend1 = Environment.TickCount;

                                        tickstart2 = Environment.TickCount;

                                        var hostName = _ChannelCheck.getHostName();
                                        var communicatorVirtualPath = 
                                            ConfigurationManager.AppSettings[CommunicatorVirtualPathConfigurationKey];
                                        var text = TemplateFunctions.LinkReWriter(
                                            PersonalizedContentHTML.ToString(), 
                                            blast, 
                                            string.Empty,
                                            communicatorVirtualPath,
                                            hostName);
                                        PersonalizedContentHTML = PersonalizedContentHTML.Replace(PersonalizedContentHTML.ToString(), text);
                                        tickend2 = Environment.TickCount;

                                        tickstart3 = Environment.TickCount;
                                        PersonalizedContentText = PersonalizedContentText.Replace(PersonalizedContentText.ToString(), TemplateFunctions.LinkReWriterText(PersonalizedContentText.ToString(), blast, blast.CustomerID.Value.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), _ChannelCheck.getHostName(), ""));
                                        tickend3 = Environment.TickCount;

                                        pc.HTMLContent = PersonalizedContentHTML.ToString();
                                        pc.TEXTContent = PersonalizedContentText.ToString();
                                        pc.IsProcessed = true;

                                        tickstart4 = Environment.TickCount;
                                        ECN_Framework_BusinessLayer.Content.PersonalizedContent.UpdateProcessed(pc);
                                        tickend4 = Environment.TickCount;

                                        if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["DisplayinConsole"]))
                                        {
                                            Console.WriteLine(string.Format("ID : {0} --imgRewriter {1} -- HTMLRewrite : {2} -- TextRewrite : {3} -- UpdateDB : {4} // {5}", pc.PersonalizedContentID.ToString(), (tickend1 - tickstart1), (tickend2 - tickstart2), (tickend3 - tickstart3), (tickend4 - tickstart4), DateTime.Now));
                                            Console.WriteLine("======================================================================================");
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        //mark as invalid
                                        ECN_Framework_BusinessLayer.Content.PersonalizedContent.MarkAsFailed(kvp.Value.PersonalizedContentID);
                                    }
                                    #endregion
                                });
                            }

                            dpcbyBlastID.Clear();
                            dpcbyBlastID = null;

                            dpc.Clear();
                            dpc = null;
                        }

                        #endregion
                    }

                    blast = null;
                    group = null;
                    customer = null;
                    cdBlastLinks.Clear();
                    cdBlastLinks = null;
                    cdUniqueLinks.Clear();
                    cdUniqueLinks = null;
                    _ChannelCheck = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);

                KM.Common.Entity.ApplicationLog.LogCriticalError(ex,
                        string.Format("ecn.PersonalizedContentEngine({0}) encountered an exception.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)),
                        Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                        string.Format("An exception Happened when handling {0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                ex.Message, ex.Source, ex.StackTrace, ex.InnerException));

            }
        }

        private static ConcurrentDictionary<TKey, TValue> ToConcurrent<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            return new ConcurrentDictionary<TKey, TValue>(dic);
        }
    }
}
