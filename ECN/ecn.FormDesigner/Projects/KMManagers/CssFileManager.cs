using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Transactions;
using System.Web.Configuration;
using ExCSS;
using KMEntities;
using KMEnums;
using KMModels;
using KMModels.PostModels;
using KMDbManagers;

using CssFile = KMEntities.CssFile;
using System.Text.RegularExpressions;

namespace KMManagers
{
    public class CssFileManager : ManagerBase
    {
        private const string Encode = "utf-8";
        private const string CssUriKey = "CssUri";
        private const string UrlToContentKey = "UrlToContent";
        private static string[] CssClasses = null;

        public CssFileManager()
        {
            if (CssClasses == null)
            {
                lock (typeof(CssFileManager))
                {
                    if (CssClasses == null)
                    {
                        CssClasses = DB.CssClassDbManager.GetAll().Select(x => x.Name).ToArray();
                    }
                }
            }
        }

        public TModel GetByFormID<TModel>(int UserID,int ChannelID, int id) where TModel : ModelBase, new()
        {
            Form form = DB.FormDbManager.GetByID(ChannelID, id);
            string data = string.Empty;
            //string name = string.Empty;
            if (form.CssFile == null)
            {
                if (form.CssUri != null)
                {
                    try
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(form.CssUri);
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(Encode));
                        data = reader.ReadToEnd();
                        reader.Close();
                        resp.Close();
                    }
                    catch
                    { }
                }
            }
            else
            {
                string fileName = "";
                switch (form.StylingType)
                {
                    case 1: // Upload
                        fileName = form.CssFile.CssFileUID.ToString() + ".css";
                        break;
                    case 2: // Custom
                        fileName = form.CssFile.Name;
                        break;
            }
                //data = form.CssFile.Read();
                string CssDir = WebConfigurationManager.AppSettings["CssDir"].TrimEnd('\\') + '\\';
                try
                {
                    CompareAndMergeFull(fileName);                    
                    string userCustomCssTemp = CssDir + "temp_" + fileName;
                    data = File.ReadAllText(userCustomCssTemp);
                    File.Delete(userCustomCssTemp);
                }
                catch
                {
                    data = string.Empty;
                    File.Delete(CssDir + "temp_" + fileName);
                }
            }

            //TModel res = null;
            //StyleSheet sheet = null;
            //if (form.StylingType == (int)StylingType.Custom && data != string.Empty)
            //{
            //    Parser parser = new Parser();
            //    sheet = parser.Parse(data);
            //}
            //if (form.StylingType != (int)StylingType.Custom || CheckIsValid(sheet))
            //{
            //    res = new TModel();
            //    res.FillData(form);
            //    if (form.StylingType == (int)StylingType.Custom)
            //    {
            //        res.FillData(sheet);
            //    }
            //}

            //comment custom css checking
            TModel res = new TModel();
            res.FillData(form);
            if (form.StylingType == (int)StylingType.Custom && data != string.Empty)
            {
                Parser parser = new Parser();
                StyleSheet sheet = parser.Parse(data);
                res.FillData(sheet);
            }

            return res;
        }

        public CustomStyles GetDefaultStyles()
        {
            string data = DB.CssFileDbManager.ReadDefault();
            Parser parser = new Parser();
            FormStylesPostModel defStyle = new FormStylesPostModel();
            defStyle.FillData(parser.Parse(data));

            return defStyle.CustomStyles;
        }

        public void Update(KMPlatform.Entity.User User,int ChannelID, FormStylesPostModel model)
        {
            //int newId = model.Id;
            Form form = DB.FormDbManager.GetByID(ChannelID, model.Id);
            form.UpdatedBy = User.UserName;
            form.LastUpdated = DateTime.Now;
            //if (form.Status != (int)FormStatus.Saved && form.Form1.Count == 0)
            //{
            //    newId = FullCopy(model, form);
            //}
            //else
            //{
            bool wasCustom = form.StylingType == (int)StylingType.Custom;
            form.StylingType = (int)model.StylingType;
            switch (model.StylingType)
            {
                case StylingType.External:
                    if (!string.IsNullOrEmpty(model.ExternalUrl))// && CheckUrl(model.ExternalUrl))
                    {
                        form.CssUri = model.ExternalUrl;
                        int? cssId = form.CssFile_Seq_ID;
                        form.CssFile_Seq_ID = null;
                        using (TransactionScope t = new TransactionScope())
                        {
                            DB.FormDbManager.SaveChanges();
                            if (cssId.HasValue)
                            {
                                DB.CssFileDbManager.DeleteByID(cssId.Value);
                                DB.CssFileDbManager.SaveChanges();
                            }

                            t.Complete();
                        }
                    }
                    break;
                case StylingType.Custom:
                    form.CssUri = null;
                    using (TransactionScope t = new TransactionScope())
                    {
                        if (wasCustom)
                        {
                            if (form.CssFile == null)
                            {
                                CssFile css = new CssFile();
                                DB.CssFileDbManager.Add(css);
                                DB.CssFileDbManager.SaveChanges();
                                form.CssFile_Seq_ID = css.CssFile_Seq_ID;
                                //FM.Add(form);
                                //FM.SaveChanges();
                            }
                            else
                            form.CssFile.Name = form.CssFile.RealFileName();
                        }
                        else
                        {
                            CssFile css = new CssFile();
                            DB.CssFileDbManager.Add(css);
                            DB.CssFileDbManager.SaveChanges();
                            form.CssFile_Seq_ID = css.CssFile_Seq_ID;
                        }

                        DB.FormDbManager.SaveChanges();
                        string data = string.Empty;
                        CompareAndMergeFull(form.CssFile.Name);
                        string CssDir = WebConfigurationManager.AppSettings["CssDir"].TrimEnd('\\') + '\\';
                        string userCustomCssTemp = CssDir + "temp_" + form.CssFile.Name;
                        data = File.ReadAllText(userCustomCssTemp);
                        form.CssFile.Write(model.RewriteCss(data));
                        File.Delete(userCustomCssTemp);
                        CompareAndSplitMin(form.CssFile.Name);

                        t.Complete();
                    }
                    break;
                case StylingType.Upload:
                    form.CssUri = null;
                    using (TransactionScope t = new TransactionScope())
                    {
                        bool isTheSame = false;
                        if (form.CssFile != null)
                        {
                            isTheSame = form.CssFile.CssFileUID == model.File.UID;
                            if (!isTheSame)
                            {
                                int? cssId = form.CssFile_Seq_ID;
                                form.CssFile_Seq_ID = null;
                                DB.FormDbManager.SaveChanges();
                                if (cssId.HasValue)
                                {
                                    DB.CssFileDbManager.DeleteByID(cssId.Value);
                                    DB.CssFileDbManager.SaveChanges();
                                }
                            }
                        }
                        if (!isTheSame)
                        {
                            CssFile css = new CssFile();
                            css.Name = model.File.Name;
                            css.CssFileUID = model.File.UID;
                            DB.CssFileDbManager.AddOnly(css);
                            DB.CssFileDbManager.SaveChanges();
                            form.CssFile_Seq_ID = css.CssFile_Seq_ID;
                        }
                        DB.FormDbManager.SaveChanges();

                        t.Complete();
                    }
                    break;
            }
            //}

            //return newId;
        }

        private bool CheckUrl(string uri)
        {
            string data = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(Encode));
                data = reader.ReadToEnd();
                reader.Close();
                resp.Close();
            }
            catch
            { }

            StyleSheet sheet = null;
            if (data != string.Empty)
            {
                Parser parser = new Parser();
                sheet = parser.Parse(data);
            }

            return CheckIsValid(sheet);
        }

        private bool CheckIsValid(StyleSheet sheet)
        {
            bool res = sheet != null && sheet.Errors.Count == 0;
            if (res)
            {
                List<string> toCheck = CssClasses.ToList();
                foreach (var rules in sheet.StyleRules.Select(x => x.Value))
                {
                    foreach (var rule in rules.Split(','))
                    {
                        string matchingData = null;
                        foreach (var data in toCheck)
                        {
                            if (RuleIsMatch(rule, data))
                            {
                                matchingData = data;
                                break;
                            }
                        }
                        if (matchingData != null)
                        {
                            toCheck.Remove(matchingData);
                        }
                    }
                }
                res = toCheck.Count == 0;
            }

            return res;
        }

        private bool RuleIsMatch(string rule, string data)
        {
            bool res = false;
            rule = rule.Trim();
            if (rule != string.Empty)
            {
                res = true;
                string[] classes = data.Split(' ');
                int index = 0;
                if (classes.Length == 1)
                {
                    res = rule == classes[0];
                }
                else
                {
                    foreach (var cl in classes)
                    {
                        int newIndex = -1;
                        if (index < rule.Length)
                        {
                            newIndex = rule.IndexOf(cl + ' ', index);
                            if (newIndex == -1 && rule.Substring(index).EndsWith(cl))
                            {
                                newIndex = rule.Length;
                            }
                        }
                        if (newIndex == -1)
                        {
                            res = false;
                            break;
                        }
                        else
                        {
                            index = newIndex + cl.Length + 1;
                        }
                    }
                }
            }

            return res;
        }

        internal string GetURLByForm(Form form)
        {
            string res = string.Empty;
            switch (form.StylingType)
            {
                case 0: //External
                    res = form.CssUri;
                    break;
                case 1: //Upload
                    res = WebConfigurationManager.AppSettings[CssUriKey].TrimEnd('/') + '/' + form.CssFile.RealFileName();
                    break;
                case 2: //Custom
                    string customCssFile = string.Empty;
                    if (form.CssFile != null)
                        customCssFile = WebConfigurationManager.AppSettings[CssUriKey].TrimEnd('/') + '/' + form.CssFile.RealFileName();
                    res = WebConfigurationManager.AppSettings[UrlToContentKey].TrimEnd('/') + "/KM_styles.css" +
                        "\" rel=\"stylesheet\"><link href=\"" + customCssFile;
                    break;
            }

            return res;
        }

        private static readonly Regex sWhitespace = new Regex(@"\s+"); 
        public static string ReplaceWhitespace(string input, string replacement) { return sWhitespace.Replace(input, replacement); }

        public void CompareAndSplitMin(string cssName)
        {
            string defaultKMcss = WebConfigurationManager.AppSettings["DefaultCSSPath"];
            string CssDir = WebConfigurationManager.AppSettings["CssDir"].TrimEnd('\\') + '\\';
            string userCustomCss = CssDir + cssName;
            string userCustomCssTemp = CssDir + "temp_" + cssName;
            if (!File.Exists(userCustomCssTemp))
                File.Copy(userCustomCss, userCustomCssTemp);

            using(FileStream fs = File.Open(defaultKMcss, FileMode.Open, FileAccess.Read))
            using(BufferedStream bs = new BufferedStream(fs))
            using(StreamReader sr = new StreamReader(bs))
            using (StreamWriter writer = new StreamWriter(userCustomCss, false, System.Text.Encoding.Default))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "/*  KM Form Builder Sample stylesheet  */" && line.Trim().Length != 0)
                    {
                        while (new string(line.ToCharArray(line.Trim().Length - 1, 1)) != "}")
                        {
                            string bodyLine = sr.ReadLine().Trim();
                            if (bodyLine.Length != 0) 
                                if (bodyLine[0] != '/')
                                    line += bodyLine;
                        }

                        string[] defClassElement = line.Split('{');
                        string[] defElement = defClassElement[1].Split(';');

                        using (FileStream fscust = File.Open(userCustomCssTemp, FileMode.Open, FileAccess.Read))
                        using (BufferedStream bscust = new BufferedStream(fscust))
                        using (StreamReader srcust = new StreamReader(bscust))
                        {
                            string lineCustom = string.Empty;
                            while ((lineCustom = srcust.ReadLine()) != null)
                            {
                                if (lineCustom != "/*  KM Form Builder Sample stylesheet  */" && lineCustom.Trim().Length != 0)
                                {
                                    while (new string(lineCustom.ToCharArray(lineCustom.Trim().Length - 1, 1)) != "}")
                                    {
                                        string bodyLine = srcust.ReadLine().Trim();
                                        if (bodyLine.Length != 0) 
                                            if (bodyLine[0] != '/')
                                                lineCustom += bodyLine;
                                    }

                                    string[] cusClassElement = lineCustom.Split('{');
                                    string defClassEle0 = ReplaceWhitespace(defClassElement[0].Trim(), "");
                                    string cusClassEle0 = ReplaceWhitespace(cusClassElement[0].Trim(), "");
                                    if (defClassEle0.Equals(cusClassEle0))
                                    {
                                        string row = defClassElement[0] + "{";
                                        writer.WriteLine(row);
                                        string[] cusElement = cusClassElement[1].Split(';');
                                        for (int i = 0; i < defElement.Length-1; i++)
                                        {
                                            string[] defItem = defElement[i].Split(':');
                                            for (int j = 0; j < cusElement.Length-1; j++)
                                            {
                                                string[] cusItem = cusElement[j].Split(':');
                                                if (defItem[0].Trim() == cusItem[0].Trim())
                                                {
                                                    if (defItem[1].Trim() != cusItem[1].Trim())
                                                    {
                                                        row = cusElement[j] + ";";
                                                        writer.WriteLine(row);
                                                    }
                                                }
                                            }
                                        }
                                        row = "}";
                                        writer.WriteLine(row);
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            File.Delete(userCustomCssTemp);
        }

        public void CompareAndSplitMinReleaseScript(string cssName, string cssDir)
        {
            string defaultKMcss = WebConfigurationManager.AppSettings["DefaultCSSPath"];
            string CssDir = cssDir;
            string userCustomCss = CssDir + cssName;
            string userCustomCssTemp = CssDir + "temp_" + cssName;
            if (!File.Exists(userCustomCssTemp))
                File.Copy(userCustomCss, userCustomCssTemp);

            using (FileStream fs = File.Open(defaultKMcss, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            using (StreamWriter writer = new StreamWriter(userCustomCss, false, System.Text.Encoding.Default))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "/*  KM Form Builder Sample stylesheet  */" && line.Trim().Length != 0)
                    {
                        while (new string(line.ToCharArray(line.Trim().Length - 1, 1)) != "}")
                        {
                            string bodyLine = sr.ReadLine().Trim();
                            if (bodyLine.Length != 0)
                                if (bodyLine[0] != '/')
                                    line += bodyLine;
                        }

                        string[] defClassElement = line.Split('{');
                        string[] defElement = defClassElement[1].Split(';');

                        using (FileStream fscust = File.Open(userCustomCssTemp, FileMode.Open, FileAccess.Read))
                        using (BufferedStream bscust = new BufferedStream(fscust))
                        using (StreamReader srcust = new StreamReader(bscust))
                        {
                            string lineCustom = string.Empty;
                            while ((lineCustom = srcust.ReadLine()) != null)
                            {
                                if (lineCustom != "/*  KM Form Builder Sample stylesheet  */" && lineCustom.Trim().Length != 0)
                                {
                                    while (new string(lineCustom.ToCharArray(lineCustom.Trim().Length - 1, 1)) != "}")
                                    {
                                        string bodyLine = srcust.ReadLine().Trim();
                                        if (bodyLine.Length != 0)
                                            if (bodyLine[0] != '/')
                                                lineCustom += bodyLine;
                                    }

                                    string[] cusClassElement = lineCustom.Split('{');
                                    string defClassEle0 = ReplaceWhitespace(defClassElement[0].Trim(), "");
                                    string cusClassEle0 = ReplaceWhitespace(cusClassElement[0].Trim(), "");
                                    if (defClassEle0.Equals(cusClassEle0))
                                    {
                                        string row = defClassElement[0] + "{";
                                        writer.WriteLine(row);
                                        string[] cusElement = cusClassElement[1].Split(';');
                                        for (int i = 0; i < defElement.Length - 1; i++)
                                        {
                                            string[] defItem = defElement[i].Split(':');
                                            for (int j = 0; j < cusElement.Length - 1; j++)
                                            {
                                                string[] cusItem = cusElement[j].Split(':');
                                                if (defItem[0].Trim() == cusItem[0].Trim())
                                                {
                                                    if (defItem[1].Trim() != cusItem[1].Trim())
                                                    {
                                                        row = cusElement[j] + ";";
                                                        writer.WriteLine(row);
                                                    }
                                                }
                                            }
                                        }
                                        row = "}";
                                        writer.WriteLine(row);
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            File.Delete(userCustomCssTemp);
        }

        public void CompareAndMergeFull(string cssName)
        {
            string defaultKMcss = WebConfigurationManager.AppSettings["DefaultCSSPath"];
            string CssDir = WebConfigurationManager.AppSettings["CssDir"].TrimEnd('\\') + '\\';
            string userCustomCss = CssDir + cssName;
            string userCustomCssTemp = CssDir + "temp_" + cssName;
            if (!File.Exists(userCustomCssTemp))
                File.Copy(defaultKMcss, userCustomCssTemp);

            using (FileStream fs = File.Open(defaultKMcss, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            using (StreamWriter writer = new StreamWriter(userCustomCssTemp, false, System.Text.Encoding.Default))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "/*  KM Form Builder Sample stylesheet  */" && line.Trim().Length != 0)
                    {
                        while (new string(line.ToCharArray(line.Trim().Length - 1, 1)) != "}")
                        {
                            string bodyLine = sr.ReadLine().Trim();
                            if (bodyLine.Length != 0) 
                                if (bodyLine[0] != '/')
                                    line += bodyLine;
                        }

                        string[] defClassElement = line.Split('{');
                        string[] defElement = defClassElement[1].Split(';');
                        bool isNewCss = true;

                        using (FileStream fscust = File.Open(userCustomCss, FileMode.Open, FileAccess.Read))
                        using (BufferedStream bscust = new BufferedStream(fscust))
                        using (StreamReader srcust = new StreamReader(bscust))
                        {
                            string lineCustom = string.Empty;
                            while ((lineCustom = srcust.ReadLine()) != null)
                            {
                                if (lineCustom != "/*  KM Form Builder Sample stylesheet  */" && lineCustom.Trim().Length != 0)
                                {
                                    while (new string(lineCustom.ToCharArray(lineCustom.Trim().Length - 1, 1)) != "}")
                                    {
                                        string bodyLine = srcust.ReadLine().Trim();
                                        if (bodyLine.Length != 0)
                                            if (bodyLine[0] != '/')
                                                lineCustom += bodyLine;
                                    }

                                    string[] cusClassElement = lineCustom.Split('{');
                                    string defClassEle0 = ReplaceWhitespace(defClassElement[0].Trim(), "");
                                    string cusClassEle0 = ReplaceWhitespace(cusClassElement[0].Trim(), "");
                                    
                                    if (defClassEle0.Equals(cusClassEle0))
                                    {
                                        isNewCss = false;
                                        string row = defClassElement[0] + "{";
                                        writer.WriteLine(row);
                                        string[] cusElement = cusClassElement[1].Split(';');
                                        
                                        for (int i = 0; i < defElement.Length - 1; i++)
                                        {
                                            string[] defItem = defElement[i].Split(':');
                                            if (cusElement.Length > 1)
                                            {
                                                row = string.Empty;
                                                for (int j = 0; j < cusElement.Length - 1; j++)
                                                {
                                                    string[] cusItem = cusElement[j].Split(':');
                                                    if (defItem[0].Trim() == cusItem[0].Trim())
                                                    {
                                                        if (defItem[1].Trim() != cusItem[1].Trim())
                                                        {
                                                            row = cusElement[j] + ";";
                                                            j = cusElement.Length;
                                                        }
                                                    }
                                                }
                                                if(!string.IsNullOrEmpty(row))
                                                {
                                                    writer.WriteLine(row);
                                                }
                                                else
                                                {
                                                    row = defElement[i] + ";";
                                                        writer.WriteLine(row);
                                                    }
                                                }
                                            else
                                            {
                                                row = defElement[i] + ";";
                                                writer.WriteLine(row);
                                            }
                                        }

                                        row = "}";
                                        writer.WriteLine(row);
                                        writer.WriteLine();
                                    }                                    
                                }
                            }
                        }

                        if (isNewCss)
                        {
                            string row = defClassElement[0] + "{";
                            writer.WriteLine(row);
                            for (int i = 0; i < defElement.Length - 1; i++)
                            {
                                row = string.Empty;
                                row = defElement[i] + ";";
                                if(defElement[i] != "}")
                                    writer.WriteLine(row);
                            }
                            row = "}";
                            writer.WriteLine(row);
                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        public bool MatchKMStandardCSS(string cssName, string type)
        {
            bool result = false;
            string CssDir = WebConfigurationManager.AppSettings["CssDir"].TrimEnd('\\') + '\\';
            string cssFileName = Guid.NewGuid() + ".css";
            try
            {
                if (type == "Upload")
                {
                    CompareAndMergeFull(cssName + ".css");
                    result = true;
                    File.Delete(CssDir + "temp_" + cssName + ".css");
                }
                else
                {                    
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(cssName, CssDir + cssFileName);
                    }
                    CompareAndMergeFull(cssFileName);
                    result = true;
                    File.Delete(CssDir + cssFileName);
                    File.Delete(CssDir + "temp_" + cssFileName);
                }
            }
            catch
            {
                if (type == "Upload")
                {
                    File.Delete(CssDir + "temp_" + cssName + ".css");
                }
                else
                {
                    File.Delete(CssDir + cssFileName);
                    File.Delete(CssDir + "temp_" + cssFileName);
                }
            }
            return result;
        }
    }
}