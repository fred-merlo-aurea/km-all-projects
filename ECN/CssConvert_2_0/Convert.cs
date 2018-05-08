using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CssConvert.CssParser;

namespace CssConvert_2_0
{
    public class Convert
    {
        private const string IdSelectSyntax = "//*[@id]";
        private const string ClassSelectSyntax = "//*[@class]";
        private const string AllNodesSelectorSyntax = "//*";
        private const string AllNodesSelectorSyntaxFormat = "//{0}";
        private const string ClassSelectorStart = ".";
        private const string IdSelectorStart = "#";
        private const string styleTagStart = "<style";
        private const string styleTagEnd = "</style>";

        public StringBuilder InlineCss(StringBuilder styleElementHtml)
        {
            if (styleElementHtml == null)
            {
                return new StringBuilder();
            }

            if (!styleElementHtml.ToString().ToLower().Contains(styleTagStart))
            {
                return styleElementHtml;
            }
            
            try
            {
                var html = styleElementHtml.ToString();                
                var originalImagesFromHtml = GetImageSrcsFromHtml(html);

                var cssParser = new CSSParser();
                var cssDocument = cssParser.ParseHTMLPage(html);                    
                var htmlDocument = new HtmlDocument();
                var htmlWithoutStyleSection = GetHtmlWithoutStyleSection(html);
                using (var htmlReader = new StringReader(htmlWithoutStyleSection))
                {                        
                    htmlDocument.Load(htmlReader);
                }                   
                
                SetHtmlNodesToLowerCase(htmlDocument);
                ProcessCssDocument(htmlDocument, cssDocument);

                var inlineHtml = htmlDocument.DocumentNode.WriteContentTo();
                foreach (var imageSrc in originalImagesFromHtml)
                {
                    if (inlineHtml.IndexOf(imageSrc, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        inlineHtml = inlineHtml.Replace(imageSrc.ToLower(), imageSrc);
                    }
                }

                return new StringBuilder(inlineHtml);
            }
            catch (Exception)
            {
                return styleElementHtml;
            }
        }

        private List<HtmlNode> BuildChildList(HtmlNode parent, string childToFind, List<HtmlNode> returnList)
        {
            foreach (HtmlNode cn in parent.ChildNodes)
            {
                if (cn.Name.Equals(childToFind))
                    returnList.Add(cn);

                if (cn.HasChildNodes)
                    BuildChildList(cn, childToFind, returnList);
            }

            return returnList;
        }

        private CssItem BuildCssList(CssConvert.CssParser.CSSDocument css)
        {
            List<Element> listElem = new List<Element>();
            List<Class> listClass = new List<Class>();
            List<Id> listId = new List<Id>();

            try
            {
                foreach (CssConvert.CssParser.RuleSet rs in css.RuleSets)//this will be each item in the css
                {
                    #region StyleList
                    List<Style> listStyle = new List<Style>();
                    //here I have to create the Style List
                    foreach (CssConvert.CssParser.Declaration dec in rs.Declarations)
                    {
                        string s = dec.ToString();
                        Style newStyle = new Style(s);
                        listStyle.Add(newStyle);
                    }
                    #endregion

                    foreach (CssConvert.CssParser.Selector sel in rs.Selectors)
                    {
                        foreach (CssConvert.CssParser.SimpleSelector simp in sel.SimpleSelectors)
                        {
                            #region Class
                            if (sel.ToString().Contains("."))
                            {
                                string[] _class = sel.ToString().Split('.');
                                #region starts with period
                                if (sel.ToString().StartsWith("."))
                                {
                                    //need to check if there is an element
                                    Class newClass = new Class();
                                    bool containsClassName = false;

                                    foreach (Class c in listClass)
                                    {
                                        if (c.Name.Equals(_class[1]))
                                        {
                                            newClass = c;
                                            containsClassName = true;
                                        }
                                    }
                                    #region Contains Space
                                    if (_class[1].Contains(" "))
                                    {
                                        //check for nested Element
                                        string[] nestedElement = _class[1].Split(' ');
                                        #region contains colon
                                        if (nestedElement[1].Contains(":"))
                                        {
                                            //have a psuedo on element
                                            string[] elementPsuedo = nestedElement[1].Split(':');
                                            //class
                                            if(containsClassName == false)
                                                newClass.Name = nestedElement[0];

                                            Element newElement = new Element();
                                            if (newClass.ListElement.Count > 0)
                                            {
                                                bool foundElement = false;
                                                foreach (Element e in newClass.ListElement)
                                                {
                                                    if (e.Name.Equals(elementPsuedo[0].ToString()))
                                                    {
                                                        newElement = e;
                                                        foundElement = true;
                                                    }
                                                }
                                                if(foundElement == false)
                                                    newElement.Name = elementPsuedo[0].ToString();
                                            }
                                            else
                                            {
                                                newElement.Name = elementPsuedo[0].ToString();
                                            }

                                            Psuedo p = new Psuedo();
                                            p.Name = elementPsuedo[1].ToString();
                                            p.ListStyle = listStyle;

                                            newElement.ListPsuedo.Add(p);

                                            newClass.ListElement.Add(newElement);
                                            if (containsClassName == false)
                                                listClass.Add(newClass);
                                        }
                                        #endregion
                                        #region No Colon
                                        else
                                        {
                                            Element ce = new Element();
                                            ce.Name = nestedElement[1];
                                            ce.ListElementStyle = listStyle;

                                            newClass.ListElement.Add(ce);
                                            if (containsClassName == false)
                                            {
                                                newClass.Name = nestedElement[0];
                                                listClass.Add(newClass);
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                    #region no space
                                    else
                                    {
                                        //this is just a class has no elements
                                        foreach (Class c in listClass)
                                        {
                                            if (c.Name.Equals(_class[1]))
                                                containsClassName = true;
                                        }
                                        if (containsClassName == false)
                                        {
                                            newClass.Name = _class[1];
                                            newClass.ListClassStyle = listStyle;
                                            listClass.Add(newClass);
                                        }
                                    }
                                    #endregion

                                }
                                #endregion
                                #region does not start with period
                                else
                                {
                                    #region contains colon
                                    //now deal with the 0 index which will either be an element or an element with psuedo
                                    if (_class[0].Contains(":"))
                                    {
                                        //1 index will be the class
                                        Class c = new Class();
                                        c.Name = _class[1];
                                        listClass.Add(c);

                                        Element e = new Element();
                                        List<Psuedo> listP = new List<Psuedo>();
                                        string[] ps = _class[0].Split(':');
                                        for (int i = 0; i <= ps.Length - 1; i++)
                                        {
                                            //the 0 index will always be the element, each additional item will be a psuedo
                                            if (i == 0)
                                            {
                                                e.Name = ps[0];
                                            }
                                            else
                                            {
                                                Psuedo p = new Psuedo();
                                                p.ListStyle = listStyle;
                                                p.Name = ps[i];
                                                listP.Add(p);
                                            }
                                        }
                                        e.ListPsuedo = listP;
                                        //listElem.Add(e);
                                        if (c.ListElement == null)
                                            c.ListElement = new List<Element>();
                                        c.ListElement.Add(e);
                                    }
                                    #endregion
                                    #region no colon
                                    else
                                    {
                                        //1 index will be the class
                                        Class c = new Class();
                                        c.Name = _class[1];
                                        listClass.Add(c);

                                        //we have just a plain element
                                        Element newElement = new Element();
                                        newElement.Name = _class[0].ToString();
                                        newElement.ListElementStyle = listStyle;

                                        listElem.Add(newElement);
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                            #region Id
                            else if (sel.ToString().Contains("#"))
                            {
                                string[] id = sel.ToString().Split('#');
                                if (sel.ToString().StartsWith("#"))//means id[1] is the id
                                {
                                    //split the id name to see check for an element
                                    if (id[1].Contains(" "))
                                    {
                                        string[] nestedElement = id[1].Split(' ');//nestedElement[0] is the element
                                        if (nestedElement[0].Contains(":"))
                                        {
                                            string[] _psuedo = nestedElement[0].Split(':');//_psuedo[0] is the element, _psuedo[1] is the psuedo 

                                            //we have a psuedo class
                                            Psuedo p = new Psuedo();
                                            p.Name = _psuedo[1].ToString();
                                            p.ListStyle = listStyle;

                                            Element _element = new Element();
                                            _element.Name = _psuedo[0].ToString();
                                            _element.ListPsuedo.Add(p);

                                            Id i = new Id();
                                            i.Name = id[1].ToString();
                                            i.ListElement.Add(_element);
                                            listId.Add(i);
                                        }
                                        else
                                        {
                                            //no psuedo class
                                            Element newElement = new Element();
                                            newElement.Name = nestedElement[0].ToString();
                                            newElement.ListElementStyle = listStyle;
                                            Id i = new Id();
                                            i.Name = id[1].ToString();
                                            i.ListElement.Add(newElement);
                                            listId.Add(i);
                                        }
                                    }
                                    else
                                    {
                                        //this is just a id
                                        Id i = new Id();
                                        i.Name = id[1];
                                        i.ListIdStyle = listStyle;
                                        listId.Add(i);
                                    }
                                }
                            }
                            #endregion
                            #region Element
                            else
                            {
                                if (sel.ToString().Contains(" "))//going to have multiple elements defined - possibly a nested element
                                {
                                    if (sel.ToString().Contains(","))//this is going to be multiple elements
                                    {

                                    }
                                    else//this will be nested elements - having a parent
                                    {
                                        string[] parentElement = sel.ToString().Split(' ');//this could get deep
                                        int peCounter = parentElement.Length;
                                       
                                        while (peCounter >= 0)
                                        {
                                            //have to do this backward cause we need to set the parent
                                            if (parentElement[peCounter].Contains(":"))
                                            {
                                                //we have a psuedo
                                                string[] elementPsuedo = parentElement[peCounter].Split(':');//0 will be element 1 will be psuedo
                                                Psuedo p = new Psuedo();
                                                p.Name = elementPsuedo[1].ToString();
                                                p.ListStyle = listStyle;

                                                Element newElement = new Element();
                                                newElement.Name = elementPsuedo[0].ToString();
                                                if (peCounter != parentElement.Length)
                                                {
                                                    newElement.Parent = listElem[listElem.Count - 1];// (listElem.Count - 1);
                                                }
                                                newElement.ListPsuedo.Add(p);
                                                //increment to the next element
                                                peCounter--;
                                            }
                                            else
                                            {
                                                Element newElement = new Element();
                                                newElement.Name = parentElement[peCounter].ToString();
                                                if (peCounter != parentElement.Length)
                                                {
                                                    newElement.Parent = listElem[listElem.Count - 1];// (listElem.Count - 1);
                                                }
                                                if (peCounter == parentElement.Length)
                                                {
                                                    newElement.ListElementStyle = listStyle;
                                                }

                                                listElem.Add(newElement);
                                                //increment to the next element
                                                peCounter--;
                                            }
                                        }

                                    }
                                }
                                else if (sel.ToString().Contains(":"))
                                {
                                    Element e = new Element();
                                    List<Psuedo> listP = new List<Psuedo>();
                                    string[] ps = sel.ToString().Split(':');
                                    for (int i = 0; i <= ps.Length - 1; i++)
                                    {
                                        //the 0 index will always be the element, each additional item will be a psuedo
                                        if (i == 0)
                                        {

                                            e.Name = ps[0];
                                        }
                                        else
                                        {
                                            Psuedo p = new Psuedo();
                                            p.ListStyle = listStyle;
                                            p.Name = ps[i];
                                            listP.Add(p);
                                        }
                                    }
                                    e.ListPsuedo = listP;
                                    listElem.Add(e);
                                }
                                else
                                {
                                    //we have just a plain element
                                    Element newElement = new Element();
                                    newElement.Name = sel.ToString();
                                    newElement.ListElementStyle = listStyle;

                                    listElem.Add(newElement);
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string except = ex.ToString();

            }

            //create our new CssItem and add the list objects
            CssItem ci = new CssItem();

            ci.ListElement = listElem;
            ci.ListClass = listClass;
            ci.ListId = listId;
            ci.HasElement = listElem.Count > 0 ? true : false;
            ci.HasClass = listClass.Count > 0 ? true : false;
            ci.HasId = listId.Count > 0 ? true : false;

            return ci;
        }

        private IEnumerable<string> GetImageSrcsFromHtml(string htmlString)
        {
            const string imageRegexPattern = @"src=""http://+.*\.jpg""|src=""http://+.*\.gif""|src=""http://+.*\.png""|src=""http://+.*\.ico""|src=""http://+.*\.bmp""";

            if (String.IsNullOrWhiteSpace(htmlString))
            {
                yield break;
            }

            var imageRegex = new Regex(imageRegexPattern, RegexOptions.IgnoreCase);
            var matches = imageRegex.Matches(htmlString);
            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }

        private string GetHtmlWithoutStyleSection(string htmlString)
        {
            htmlString = htmlString.ToLower();
            if (htmlString.Contains(styleTagStart))
            {
                var start = htmlString.IndexOf(styleTagStart);
                var last = htmlString.IndexOf(styleTagEnd, start) + styleTagEnd.Length;

                htmlString = htmlString.Remove(start, last - start);
            }

            return htmlString;
        }

        private void SetHtmlNodesToLowerCase(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null)
            {
                return;
            }

            const string hrefTag = "href";

            var htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(AllNodesSelectorSyntax);
            foreach (var node in htmlNodeCollection)
            {
                node.Name = node.Name.ToLower();
                foreach (var nodeAttribute in node.Attributes)
                {
                    if (!nodeAttribute.Name.ToLower().Equals(hrefTag))
                    {
                        nodeAttribute.Name = nodeAttribute.Name.ToLower();
                        nodeAttribute.Value = nodeAttribute.Value.ToLower();
                    }
                }
            }
        }

        private void ProcessCssDocument(HtmlDocument htmlDocument, CSSDocument cssDocument)
        {
            if (htmlDocument == null || cssDocument == null)
            {
                return;
            }

            foreach (var ruleSet in cssDocument.RuleSets)
            {
                foreach (var selector in ruleSet.Selectors)
                {
                    var isSingleSelector = selector.SimpleSelectors.Count == 1 ||
                            (selector.SimpleSelectors.Count == 2 &&
                             !String.IsNullOrWhiteSpace(selector.SimpleSelectors[0].Class));
                    if (isSingleSelector)
                    {
                        ProcessSingleSelector(selector, htmlDocument, cssDocument, ruleSet);
                    }
                    else
                    {
                        ProcessMultipleSelectors(selector, htmlDocument, ruleSet);
                    }
                }
            }            
        }

        private void ProcessSingleSelector(Selector selector, HtmlDocument htmlDocument,
            CSSDocument cssDocument, RuleSet ruleSet)
        {
            if (selector == null)
            {
                return;
            }
            
            var colonName = String.Empty;
            var selectorName = GetCleanedUpSelector(selector.ToString(), out colonName);

            var identifiedNodes = new List<HtmlNode>();
            var idNodesIdentified = false;
            var regularNodesIdentified = false;

            var classNodesIdentified = IdentifyNodesMatchingSelector(selectorName, ClassSelectSyntax,
                ClassSelectorStart, htmlDocument, (node) => identifiedNodes.Add(node), true);
            if (!classNodesIdentified)
            {
                idNodesIdentified = IdentifyNodesMatchingSelector(selectorName, IdSelectSyntax,
                    IdSelectorStart, htmlDocument, (node) => identifiedNodes.Add(node));
                if (!idNodesIdentified)
                {
                    regularNodesIdentified = true;

                    var tempAllNodes = htmlDocument.DocumentNode.SelectNodes(String.Format(AllNodesSelectorSyntaxFormat, selectorName));
                    var tempClassNodes = htmlDocument.DocumentNode.SelectNodes(ClassSelectSyntax);
                    var tempIdNodes = htmlDocument.DocumentNode.SelectNodes(IdSelectSyntax);

                    if (tempAllNodes != null)
                    {
                        foreach (var node in tempAllNodes)
                        {
                            if ((tempClassNodes != null && tempClassNodes.Any(classNode => classNode == node)) ||
                                (tempIdNodes != null && tempIdNodes.Any(idNode => idNode == node)))
                            {
                                identifiedNodes.Add(node);
                            }
                        }
                    }
                }
            }
            
            foreach (var node in identifiedNodes)
            {
                var hasAncestor = NodeHasAncestors(node, cssDocument);
                if (classNodesIdentified && !hasAncestor)
                {
                    ProcessNode(node, ruleSet.Declarations, colonName);
                }
                if (idNodesIdentified && !hasAncestor)
                {
                    ProcessNode(node, ruleSet.Declarations, colonName);
                }
                if (regularNodesIdentified && !hasAncestor && 
                    selectorName.Equals(node.Name, StringComparison.OrdinalIgnoreCase))
                {
                    ProcessNode(node, ruleSet.Declarations, colonName);
                }
            }
        }

        private void ProcessMultipleSelectors(Selector selector, HtmlDocument htmlDocument, RuleSet ruleSet)
        {
            if (selector == null)
            {
                return;
            }

            var parent = selector.SimpleSelectors[0].ToString().ToLower();
            var child = selector.SimpleSelectors[1].ToString().ToLower();
            var colonName = String.Empty;
            var selectorName = GetCleanedUpSelector(child, out colonName);

            var identifiedNodes = new List<HtmlNode>();
            var listChild = new List<HtmlNode>();

            var classList = IdentifyNodesMatchingSelector(parent, ClassSelectSyntax,
                ClassSelectorStart, htmlDocument, (node) => identifiedNodes.Add(node));
            if (!classList)
            {
                var idList = IdentifyNodesMatchingSelector(parent, IdSelectSyntax,
                    IdSelectorStart, htmlDocument, (node) => identifiedNodes.Add(node));
                if (!idList)
                {
                    var nodes = htmlDocument.DocumentNode.SelectNodes(String.Format(AllNodesSelectorSyntaxFormat, parent));
                    if (nodes != null)
                    {
                        foreach (var node in nodes)
                        {
                            identifiedNodes.Add(node);
                        }
                    }
                }
            }

            foreach (var parentNode in identifiedNodes)
            {
                listChild = BuildChildList(parentNode, child, listChild);
            }

            //now add the style or psuedo to the elements in the listChild
            foreach (var node in listChild)
            {
                ProcessNode(node, ruleSet.Declarations, colonName);
            }
        }

        private string GetCleanedUpSelector(string selectorName, out string colonName)
        {
            colonName = String.Empty;
            if (String.IsNullOrWhiteSpace(selectorName))
            {
                return String.Empty;
            }

            selectorName = selectorName.ToLower();
            if (selectorName.Contains(":"))//we have some psuedo class to add
            {
                //need to find the base name of the element so remove : and everything after it
                var totalLength = selectorName.Length;
                var colonIndex = selectorName.IndexOf(":");
                colonName = selectorName.Substring(colonIndex + 1, totalLength - (colonIndex + 1));
                selectorName = selectorName.Remove(colonIndex, totalLength - colonIndex);
            }

            return selectorName;
        }

        private bool IdentifyNodesMatchingSelector(string selectorName, string selectSyntax, string stringSelectorStart,
            HtmlDocument htmlDocument, Action<HtmlNode> processNode, bool removeEmptySpace = false)
        {
            if (!selectorName.StartsWith(stringSelectorStart))
            {
                return false;
            }

            if (removeEmptySpace)
            {
                const string whiteSpace = " ";
                if (selectorName.Contains(whiteSpace))
                {
                    var totalLength = selectorName.Length;
                    var spaceIndex = selectorName.IndexOf(whiteSpace);
                    selectorName = selectorName.Remove(spaceIndex, totalLength - spaceIndex);
                }
            }

            var className = selectorName.Remove(0, 1);
            var nodes = htmlDocument.DocumentNode.SelectNodes(selectSyntax);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.Attributes.Any(
                        attribute => attribute.Value.Equals(className, StringComparison.OrdinalIgnoreCase)))
                    {
                        processNode(node);
                    }
                }
            }

            return true;
        }

        private bool NodeHasAncestors(HtmlNode node, CSSDocument cssDocument)
        {
            if (node == null || node.ParentNode == null)
            {
                return false;
            }

            var ancestorNodesNames = new List<string>();
            var parentNode = node.ParentNode;
            var hasParent = true;
            while (hasParent)
            {
                if (parentNode.ParentNode != null)
                {
                    ancestorNodesNames.Add(parentNode.Name);
                    parentNode = parentNode.ParentNode;
                }
                else
                {
                    hasParent = false;
                }
            }

            if (!ancestorNodesNames.Any())
            {
                return false;
            }

            foreach (var ruleSet in cssDocument.RuleSets)
            {
                foreach (var selector in ruleSet.Selectors)
                {
                    if (selector.SimpleSelectors.Count > 1)
                    {
                        if (ancestorNodesNames.Any(x => 
                            x.Equals(selector.SimpleSelectors[0].ToString(), StringComparison.OrdinalIgnoreCase) &&
                            selector.SimpleSelectors[1].ToString().Equals(node.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void ProcessNode(HtmlNode node, IList<Declaration> declarations, string colonName)
        {
            if (node == null)
            {
                return;
            }

            const string styleTagName = "style";
            const string semiColon = ";";

            var inlineStyle = new StringBuilder();            
            if (!String.IsNullOrWhiteSpace(colonName))
            {
                const string colon = ":";
                const string leftCurlyBracket = "{";
                const string rightCurlyBracket = "}";

                inlineStyle.AppendFormat("{0}{1} {2}", colon, colonName, leftCurlyBracket);
                AppendDeclarations(declarations, inlineStyle, semiColon);
                inlineStyle.Append(rightCurlyBracket);
            }
            else
            {
                AppendDeclarations(declarations, inlineStyle, semiColon);
            }

            var styleAttribute = node.Attributes[styleTagName];
            if (styleAttribute == null)
            {
                node.Attributes.Append(styleTagName, inlineStyle.ToString());
            }
            else
            {
                styleAttribute.Value += inlineStyle.ToString();
            }
        }

        private void AppendDeclarations(IList<Declaration> declarations, StringBuilder builder, string separator)
        {
            if (declarations == null)
            {
                return;
            }

            if (builder == null)
            {
                builder = new StringBuilder();
            }
            foreach (var declaration in declarations)
            {
                builder.Append(declaration).Append(separator);
            }
        }
    }
}
