using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContextT = ECN_Framework_Entities.Communicator.ContentReplacement.RSSFeedByCampaignItemTokenReplacementContext;

namespace ECN_Framework_BusinessLayer.Communicator.ContentReplacement
{
    using System.Net;
    using System.Xml;
    using IReplace = ECN_Framework_Entities.Communicator.ContentReplacement.IContextTokenReplace<ContextT>;

    public class RSSFeed : IReplace
    {
        public class BlastCached : RSSFeed
        {
            public int BlastID { get; set; }
            public bool IsText { get; set; }
            public bool ExistsInCache(string token, ContextT context)
            {
                if (BlastID > 0 && context != null && context.RSSFeed != null && 0 < context.RSSFeed.FeedID)
                {
                    if (ECN_Framework_BusinessLayer.Communicator.BlastRSS.ExistsByBlastID_FeedID(BlastID, context.RSSFeed.FeedID))
                    {
                        ECN_Framework_Entities.Communicator.BlastRSS bRSS = ECN_Framework_BusinessLayer.Communicator.BlastRSS.GetByBlastID_FeedID(BlastID, context.RSSFeed.FeedID);
                        if (bRSS != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            public override string ReplaceHtml(string token, ContextT context)
            {
                if (ExistsInCache(token, context))
                {
                    ECN_Framework_Entities.Communicator.BlastRSS bRSS = ECN_Framework_BusinessLayer.Communicator.BlastRSS.GetByBlastID_FeedID(BlastID, context.RSSFeed.FeedID);
                    if (bRSS != null)
                    {
                        return bRSS.FeedHTML;
                    }
                }

                // fall back to using the live RSS feed
                return base.ReplaceHtml(token, context);
            }

            public override string ReplaceText(string token, ContextT context)
            {
                if(ExistsInCache(token, context))
                {
                    ECN_Framework_Entities.Communicator.BlastRSS bRSS = ECN_Framework_BusinessLayer.Communicator.BlastRSS.GetByBlastID_FeedID(BlastID, context.RSSFeed.FeedID);
                    if (bRSS != null)
                    {
                        return bRSS.FeedTEXT;
                    }
                }

                // fall back to using the live RSS feed
                return base.ReplaceText(token, context);
            }
        }

        const string HtmlReplacementPattern = @"<a href='{0}'>{1}</a><br /><span>{2}</span><br />";
        const string TextReplacementPattern = "<{0}>\n{1}\n\n";

        readonly static System.Text.RegularExpressions.Regex _pattern = new System.Text.RegularExpressions.Regex(
                    @"ECN.RSSFEED\.(.*?)\.ECN.RSSFEED", // TOG followed by a literal dot 
                                                        // followed by as few as possible of any character (captured as match.Group[1])
                                                        // followed by a literal dot and then TAG, again
                    System.Text.RegularExpressions.RegexOptions.Singleline 
                  | System.Text.RegularExpressions.RegexOptions.Compiled);

        readonly static System.Text.RegularExpressions.Regex HtmlToTextTransformationPattern = new System.Text.RegularExpressions.Regex(
            @"\<a href='(.*?)'\>(.*?)\</a\>\<br /\>\<span\>(.*?)\</span\>\<br /\>",  // group 1 is the ID, group 2 is the summary
            System.Text.RegularExpressions.RegexOptions.Singleline 
          | System.Text.RegularExpressions.RegexOptions.Compiled);

        public static void Replace(ref string content, int customerID, bool isText=false, int? blastID = null)
        {
            IReplace rssTransformer = null;
            if (blastID.HasValue)
            {
                rssTransformer = new BlastCached
                {
                    CustomerID = customerID,
                    BlastID = blastID.Value,
                    IsText = isText
                };
            }
            else
            {
                rssTransformer = new RSSFeed { CustomerID = customerID };
            }
            
            System.Text.RegularExpressions.MatchEvaluator ev = isText
               ? new System.Text.RegularExpressions.MatchEvaluator(target => rssTransformer.ReplaceText(rssTransformer.GetToken(target), rssTransformer.BuildContext(target)))
               : new System.Text.RegularExpressions.MatchEvaluator(target => rssTransformer.ReplaceHtml(rssTransformer.GetToken(target), rssTransformer.BuildContext(target)));

            content = rssTransformer.Pattern.Replace(content, ev);
        }


        public static void ReplaceWithHandler(ref string originalHtml, ref string originalText, int customerID,int blastID, Action<ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>> beforeReplaceHandler = null)
        {
            try
            {
                RSSFeed rssTransformer = new RSSFeed { CustomerID = customerID };
                Dictionary<string, ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>> feeds =
                    new Dictionary<string, ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>>();

                // find all of the feeds listed in the HTML content
                foreach (System.Text.RegularExpressions.Match match in rssTransformer.Pattern.Matches(originalHtml))
                {
                    string feedName = InternalGetToken(match);
                    ContextT context = rssTransformer.BuildContext(match);
                    ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT> eventArgs = new ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>()
                    {
                        CustomerID = customerID,
                        Context = context,
                        FeedName = feedName,
                        Match = match,
                        HTML = rssTransformer.ReplaceHtml(feedName, context),
                        BlastID = blastID
                    };
                    eventArgs.Text = HtmlToTextTransformationPattern.Replace(eventArgs.HTML, (m) =>
                        String.Format(TextReplacementPattern, m.Groups[1].Value, !string.IsNullOrEmpty(m.Groups[3].Value) ? m.Groups[3].Value : m.Groups[2].Value));

                    feeds[feedName] = eventArgs;
                }

                // find any that might be present only in the Text version
                foreach (System.Text.RegularExpressions.Match match in rssTransformer.Pattern.Matches(originalText))
                {
                    string feedName = InternalGetToken(match);
                    if (false == feeds.ContainsKey(feedName))
                    {
                        ContextT context = rssTransformer.BuildContext(match);
                        ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT> eventArgs = new ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>()
                        {
                            CustomerID = customerID,
                            Context = context,
                            FeedName = feedName,
                            Match = match,
                            Text = rssTransformer.ReplaceText(feedName, context),
                            BlastID = blastID
                        };
                        eventArgs.HTML = "";
                        feeds[feedName] = eventArgs;
                    }
                }

                // fire off a message then replace within both text and html.  replacement value is taken from eventArgs so beforeReplace method
                // can potentially alter the content before the replacement is made
                foreach (ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT> eventArgs in feeds.Values)
                {
                    if (beforeReplaceHandler != null)
                    {
                        beforeReplaceHandler(eventArgs);
                    }

                    originalText = originalText.Replace(eventArgs.Match.Groups[0].Value, eventArgs.Text);
                    originalHtml = originalHtml.Replace(eventArgs.Match.Groups[0].Value, eventArgs.HTML);
                }

            }
            catch(Exception ex)
            {
                throw new ECN_Framework_Common.Objects.BlastHoldException("RSS Feed generation failed", ex, "RSSHOLD", blastID,customerID);
            }
        }

        public int CustomerID { get; set; }

        public event EventHandler<ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>> OnAfterReplace;

        public System.Text.RegularExpressions.Regex Pattern
        {
            get{ return _pattern; }
        }

        public virtual string GetToken(System.Text.RegularExpressions.Match match)
        {
            return ( InternalGetToken(match) ?? "" ).Trim();
        }
        
        public static string InternalGetToken(System.Text.RegularExpressions.Match match)
        {
            if(match == null || null == match.Groups || 1 > match.Groups.Count || null == match.Groups[1])
            {
                return String.Empty;
            }

            return match.Groups[1].Value;
        }

        public virtual ContextT BuildContext(System.Text.RegularExpressions.Match match)
        {
            string feedName = InternalGetToken(match);
            return new ContextT
            {
                FeedName = feedName,
                RSSFeed = ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByFeedName(feedName, CustomerID)
            };
        }

        public virtual string Replace<T>(T si, string replacementPattern, Func<T, string[]> getArgs)
        {
            if(String.IsNullOrEmpty(replacementPattern) || si == null)
            {
                return "";
            }
            
            return String.Format(replacementPattern, getArgs(si));
        }

        string Replace(string token, ContextT context, string replacementPattern, Func<System.ServiceModel.Syndication.SyndicationItem, string[]> getArgs, int storiesToShow=10)
        {
            if (context == null || context.RSSFeed == null)
            {
                return "";
            }

            //int storiesToShow = context.RSSFeed.StoriesToShow ?? 10;            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            return this.Replace<System.ServiceModel.Syndication.SyndicationItem>(context.RSSFeed.URL, replacementPattern, 
                (url) => System.ServiceModel.Syndication.SyndicationFeed.Load(XmlReader.Create(url)).Items.OrderByDescending( y => y.PublishDate ).Take( storiesToShow ),
                (si) => getArgs(si)                         
            );

            /*StringBuilder sbRSSFeed = new StringBuilder();
            StringBuilder sbRSSFeedText = new StringBuilder();
            XmlReader reader = XmlReader.Create(context.RSSFeed.URL);
            System.ServiceModel.Syndication.SyndicationFeed sf = System.ServiceModel.Syndication.SyndicationFeed.Load(reader);

            int storiesToShow = context.RSSFeed.StoriesToShow ?? 10;
            int index = 0;
            foreach (System.ServiceModel.Syndication.SyndicationItem si in sf.Items.OrderByDescending(x => x.PublishDate))
            {
                if (index++ < storiesToShow)
                {
                    sbRSSFeed.Append(Replace(si, replacementPattern, getArgs));
                }
                else
                    break;
            }

            return sbRSSFeed.ToString();*/
        }

        public virtual string Replace<T>(string feedName, string replacementPattern, Func<string, IEnumerable<T>> getIterator, Func<T, string[]> getArgs)
        {
            if (String.IsNullOrWhiteSpace(feedName) || String.IsNullOrWhiteSpace(replacementPattern) || null == getIterator || null == getArgs)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            foreach (T si in getIterator(feedName))
            {
                sb.Append(Replace<T>(si, replacementPattern, getArgs));
            }

            return sb.ToString();
        }

        public virtual string ReplaceHtml(string token, ContextT context)
        {            
            return Replace(token, context,
                HtmlReplacementPattern, 
                si => new string[] 
                { 
                    si.Id, 
                    si.Title != null   ? si.Title.Text   : "", 
                    si.Summary != null ? si.Summary.Text : "" 
                }, context != null && context.RSSFeed != null && context.RSSFeed.StoriesToShow.HasValue ? context.RSSFeed.StoriesToShow.Value : 10);
        }

        public virtual string ReplaceText(string token, ContextT context)
        {
            return Replace(token, context,
                TextReplacementPattern,
                si => new string[]
                {
                    si.Id,
                    si.Summary != null ? si.Summary.Text : ""
                }, context != null && context.RSSFeed != null && context.RSSFeed.StoriesToShow.HasValue ? context.RSSFeed.StoriesToShow.Value : 10);
        }
    }
}
