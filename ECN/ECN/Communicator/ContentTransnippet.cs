using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ecn.communicator.classes
{
    public class ContentTransnippet
    {
        public ContentTransnippet()
        {
        }

        public static int CheckForTransnippet(string html)
        {
            try
            {
                //make sure we have an equal number of opening and closing trans tags
                //open tags
                Regex transnippetRegEx = new Regex(@"<transnippet ", RegexOptions.IgnoreCase);
                MatchCollection transnippetMatchs = transnippetRegEx.Matches(html);
                int transOpenCount = transnippetMatchs.Count;
                //close tags
                transnippetRegEx = new Regex(@"</transnippet>", RegexOptions.IgnoreCase);
                transnippetMatchs = transnippetRegEx.Matches(html);
                int transCloseCount = transnippetMatchs.Count;

                if (transOpenCount > 0)
                {
                    if (transOpenCount != transCloseCount)
                    {
                        return -1;
                    }

                    transnippetRegEx = new Regex(@"<transnippet_detail>", RegexOptions.IgnoreCase);
                    transnippetMatchs = transnippetRegEx.Matches(html);
                    transOpenCount = transnippetMatchs.Count;

                    //close tags
                    transnippetRegEx = new Regex(@"</transnippet_detail>", RegexOptions.IgnoreCase);
                    transnippetMatchs = transnippetRegEx.Matches(html);
                    transCloseCount = transnippetMatchs.Count;

                    if (transOpenCount > 0)
                    {
                        if (transOpenCount != transCloseCount)
                        {
                            return -1;
                        }
                        else
                        {
                            return transOpenCount;
                        }
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string ModifyHTML(string htmlOriginal, DataTable emailProfileDataTable)
        {
            string htmlModified = string.Empty;
            System.Collections.Generic.List<string> htmlList = new System.Collections.Generic.List<string>();

            //new split the html
            while (htmlOriginal.Length > 0)
            {
                int posTransOpen = htmlOriginal.IndexOf("<transnippet ");
                int posTransClose = htmlOriginal.IndexOf("</transnippet>");
                if (posTransOpen > 0)
                {
                    htmlModified += htmlOriginal.Substring(0, posTransOpen);
                    htmlOriginal = htmlOriginal.Substring(posTransOpen, htmlOriginal.Length - posTransOpen);
                }
                else if (posTransOpen == 0)
                {
                    string sortField = "";
                    string filterField = "";
                    string filterValue = "";
                    //<transnippet filter_field="ItemStatus" filter_value="RENT" sort="Property1Name">
                    //<transnippet filter_field="ItemStatus" filter_value="PURCHASE" sort="Property3Name">
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(htmlOriginal.Substring(0, htmlOriginal.IndexOf(">") + 1) + "</transnippet>");
                    XmlElement root = doc.DocumentElement;
                    if (root.HasAttribute("sort"))
                    {
                        sortField = root.Attributes["sort"].Value;
                    }
                    if (root.HasAttribute("filter_field"))
                    {
                        filterField = root.Attributes["filter_field"].Value;
                    }
                    if (root.HasAttribute("filter_value"))
                    {
                        filterValue = root.Attributes["filter_value"].Value;
                    }

                    htmlModified += GetTransDetail(htmlOriginal.Substring(0, posTransClose + 14), emailProfileDataTable, filterField, filterValue, sortField);
                    htmlOriginal = htmlOriginal.Substring(posTransClose + 14, htmlOriginal.Length - posTransClose - 14);
                }
                else
                {
                    htmlModified += htmlOriginal;
                    htmlOriginal = string.Empty;
                }
            }
            return htmlModified;
        }

        public static string ModifyTEXT(string text, DataTable dtEmails)
        {
            string textModified = string.Empty;

            System.Collections.Generic.List<string> htmlList = new System.Collections.Generic.List<string>();

            //new split the html
            while (text.Length > 0)
            {
                int posTransOpen = text.IndexOf("<transnippet ");
                int posTransClose = text.IndexOf("</transnippet>");
                if (posTransOpen > 0)
                {
                    textModified += text.Substring(0, posTransOpen);
                    text = text.Substring(posTransOpen, text.Length - posTransOpen);
                }
                else if (posTransOpen == 0)
                {
                    string sortField = "";
                    string filterField = "";
                    string filterValue = "";
                    //<transnippet filter_field="ItemStatus" filter_value="RENT" sort="Property1Name">
                    //<transnippet filter_field="ItemStatus" filter_value="PURCHASE" sort="Property3Name">
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(text.Substring(0, text.IndexOf(">") + 1) + "</transnippet>");
                    XmlElement root = doc.DocumentElement;
                    if (root.HasAttribute("sort"))
                    {
                        sortField = root.Attributes["sort"].Value;
                    }
                    if (root.HasAttribute("filter_field"))
                    {
                        filterField = root.Attributes["filter_field"].Value;
                    }
                    if (root.HasAttribute("filter_value"))
                    {
                        filterValue = root.Attributes["filter_value"].Value;
                    }

                    textModified += GetTransDetail(text.Substring(0, posTransClose + 14), dtEmails, filterField, filterValue, sortField);
                    text = text.Substring(posTransClose + 14, text.Length - posTransClose - 14);
                }
                else
                {
                    textModified += text;
                    text = string.Empty;
                }
            }

            return textModified;
        }

        private static string GetTransDetail(string line, DataTable emailProfileDataTable, string filterField, string filterValue, string sortField)
        {
            line = line.Replace(line.Substring(0, line.IndexOf(">") + 1), "");
            line = line.Replace("</transnippet>", "");

            string lineModified = string.Empty;
            System.Collections.Generic.List<string> htmlList = new System.Collections.Generic.List<string>();
            string filter = string.Empty;
            string sort = string.Empty;
            if (filterField.Length > 0 && filterValue.Length > 0)
            {
                filter = filterField + " = '" + filterValue.Trim().ToUpper() + "'";
            }
            if (sortField.Length > 0)
            {
                sort = sortField.Trim().ToUpper() + " ASC";
            }

            //split the html
            int posTransOpen = 0;
            int posTransClose = 0;
            while (line.Length > 0)
            {
                posTransOpen = line.IndexOf("<transnippet_detail>");
                posTransClose = line.IndexOf("</transnippet_detail>");
                if (posTransOpen > 0)
                {
                    htmlList.Add(line.Substring(0, posTransOpen));
                    line = line.Substring(posTransOpen, line.Length - posTransOpen);
                }
                else if (posTransOpen == 0)
                {
                    htmlList.Add(line.Substring(0, posTransClose + 21));
                    line = line.Substring(posTransClose + 21, line.Length - posTransClose - 21);
                }
                else
                {
                    htmlList.Add(line);
                    line = string.Empty;
                }
            }

            foreach (string innerLine in htmlList)
            {
                //find out how many transnippet sections we need and then add them
                if (innerLine.IndexOf("<transnippet_detail>") == 0)
                {
                    DataRow[] dr;
                    DataView dv = emailProfileDataTable.DefaultView;
                    if (sort.Length > 0)
                    {
                        dv.Sort = sort;
                    }
                    if (filter.Length > 0)
                    {
                        dv.RowFilter = filter;
                    }
                    dr = dv.ToTable().Select();

                    if (dr.Length <= 0)
                    {
                        return string.Empty;
                    }

                    for (int i = 0; i < dr.Length; i++)
                    {
                        //replace all code snippets with corresponding value from emailprofiledataset
                        string newLine = innerLine;
                        string[] matchList = GetSnippets(newLine);
                        for (int l = 0; l < matchList.Length; l++)
                        {
                            newLine = newLine.Replace("##" + matchList[l] + "##", dr[i][matchList[l]].ToString());
                        }

                        lineModified += newLine;
                    }

                    lineModified = lineModified.Replace("<transnippet_detail>", "");
                    lineModified = lineModified.Replace("</transnippet_detail>", "");
                }
                else
                {
                    lineModified += innerLine;
                }
            }

            return lineModified;
        }

        private static string[] GetSnippets(string line)
        {
            string[] matches = null;
            MatchCollection MatchList1 = null;
            Regex regMatch = new System.Text.RegularExpressions.Regex("##", RegexOptions.IgnoreCase);
            MatchCollection MatchList = regMatch.Matches(line);
            if (MatchList.Count > 0)
            {
                if ((MatchList.Count % 2) != 0)
                {
                    return matches;
                }
                else
                {
                    Regex regMatchGood = new Regex("##[a-zA-Z0-9_]+?##", RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(line);
                    if ((MatchList.Count / 2) > MatchListGood.Count)
                    {
                        return matches;
                    }
                }
            }

            //%% and ##
            Regex reg1 = new System.Text.RegularExpressions.Regex("##.+?##", RegexOptions.IgnoreCase);
            MatchList1 = reg1.Matches(line);
            matches = new string[MatchList1.Count];
            int p = 0;
            foreach (Match m in MatchList1)
            {
                if (!string.IsNullOrEmpty(m.Value.ToString()))
                {
                    matches[p] = m.Value.ToString().Replace("##", string.Empty);
                }
                p++;
            }

            return matches;
        }

    }
}
