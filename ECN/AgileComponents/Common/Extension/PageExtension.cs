#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.Diagnostics;
using System.Text;
using System.Web.UI;

namespace ActiveUp.WebControls.Common.Extension
{
    public static class PageExtension
    {
        private const string ScriptKeyPostFix = "_startup";
        private const string ActiveToolbarJsResource = "ActiveUp.WebControls._resources.ActiveToolbar.js";

        public static void TestAndRegisterScriptBlock(
            this Page page,
            string scriptKey,
            string scriptDirectory,
            string scriptPresentFuncName)
        {
            if (!page.IsClientScriptBlockRegistered(string.Concat(scriptKey, ScriptKeyPostFix)))
            {
                var directoryDescription = scriptDirectory == string.Empty
                    ? string.Empty
                    : string.Concat(" [", scriptDirectory, "] directory or change the");

                var startupStringBuilder = new StringBuilder();
                startupStringBuilder.Append("<script>\n");
                startupStringBuilder.Append("// Test if the client script is present.\n");
                startupStringBuilder.Append("try\n{\n");
                startupStringBuilder.Append(scriptPresentFuncName).Append("\n");
                startupStringBuilder.Append("}\n catch (e)\n {\n")
                    .Append(" alert('Could not find script file. ")
                    .Append("Please ensure that the Javascript files are deployed in the ")
                    .Append(directoryDescription)
                    .Append("ScriptDirectory and/or ExternalScript properties to point to the directory ")
                    .Append("where the files are.'); \n}\n");
                startupStringBuilder.Append("\n</script>\n");

                page.RegisterClientScriptBlock(string.Concat(scriptKey, ScriptKeyPostFix),
                    startupStringBuilder.ToString());
            }
        }

        public static void TestAndRegisterScriptInclude(
            this Page page, 
            string activeToolBarScriptKey, 
            string clientSideApi, 
            Type type)
        {
            if (!page.IsClientScriptBlockRegistered(activeToolBarScriptKey))
            {
                page.RegisterActiveToolbarClientScriptInclude(activeToolBarScriptKey, type);
                page.RegisterActiveToolBarScript(clientSideApi, activeToolBarScriptKey);
            }
        }

        [Conditional("NOT_FX1_1")]
        public static void RegisterActiveToolbarClientScriptInclude(
            this Page page, 
            string activeToolBarScriptKey,
            Type type)
        {
            page.ClientScript.RegisterClientScriptInclude(
                activeToolBarScriptKey,
                page.ClientScript.GetWebResourceUrl(type, ActiveToolbarJsResource));
        }

        [Conditional("FX1_1")]
        public static void RegisterActiveToolBarScript(
            this Page page, 
            string clientSideApi, 
            string activeToolBarScriptKey)
        {
            if (clientSideApi == null)
            {
                clientSideApi = Utils.GetResource(ActiveToolbarJsResource);
            }

            if (!clientSideApi.StartsWith("<script"))
            {
                clientSideApi = "<script language=\"javascript\">\n" + clientSideApi + "\n";
            }

            clientSideApi += "\n</script>\n";

            page.RegisterClientScriptBlock(activeToolBarScriptKey, clientSideApi);
        }
    }
}