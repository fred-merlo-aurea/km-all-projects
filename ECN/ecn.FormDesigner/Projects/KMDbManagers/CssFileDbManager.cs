using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using KMEntities;

namespace KMDbManagers
{
    public class CssFileDbManager : DbManagerBase
    {
        private const string CssDirKey = "CssDir";
        public const string CssExt = ".css";
        private const string DefaultCSSPathKey = "DefaultCSSPath";

        private readonly static string CssDir = null;

        static CssFileDbManager()
        {
            CssDir = WebConfigurationManager.AppSettings[CssDirKey].TrimEnd('\\') + '\\';
        }
        
        public static string GetFullName(CssFile css)
        {
            return CssDir + css.CssFileUID + CssExt;
        }

        public string ReadDefault()
        {
            string data = string.Empty;
            try
            {
                data = File.ReadAllText
                (
                    WebConfigUtils.KMDesignerRoot() +
                    WebConfigurationManager.AppSettings[DefaultCSSPathKey]
                );
            }
            catch
            { }

            return data;
        }

        public void Add(CssFile css)
        {
            Add(css, -1);
        }

        public void Add(CssFile css, int copyId)
        {
            css.CssFileUID = Guid.NewGuid();
            css.Name = css.CssFileUID + CssExt;
            string fromName =
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[DefaultCSSPathKey];
            if (copyId > 0)
            {
                CssFile copy = KM.CssFiles.Single(x => x.CssFile_Seq_ID == copyId);
                fromName = CssDir + copy.CssFileUID + CssExt;
            }
            File.Copy(fromName, CssDir + css.Name);
            KM.CssFiles.Add(css);
        }

        public void AddOnly(CssFile css)
        {
            KM.CssFiles.Add(css);
        }

        public void DeleteByID(int id)
        {
            CssFile file = KM.CssFiles.Single(x => x.CssFile_Seq_ID == id);
            try
            {
                File.Delete(CssDir + file.CssFileUID + CssExt);
            }
            catch
            { }
            KM.CssFiles.Remove(file);
        }
    }
}