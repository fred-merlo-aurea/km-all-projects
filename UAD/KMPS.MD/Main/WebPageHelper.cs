using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Data;

namespace KMPS.MD.Main
{
        public class WebPageHelper : System.Web.UI.Page
        {
            //protected override void SavePageStateToPersistenceMedium(object viewState)
            //{
            //    List<string> lVSExPages = new List<string>(ConfigurationManager.AppSettings["ViewStateExPages"].ToString().Split(','));

            //    string VSKey = null;
            //    //String that will hold the Unique Key 
            //    //used to reference this ViewState data

            //    //Create the key based on the SessionID, on the Request.RawUrl 
            //    //and on the Ticks representated by the exact time 
            //    //while the page is being saved
            //    VSKey = "VIEWSTATE_" + base.Session.SessionID + "_" + Request.RawUrl + "_" + System.DateTime.Now.Ticks.ToString();

            //    //Check if the ServerSideViewState is Activated

            //    string currentPage = Request.ServerVariables["Path_Info"].ToLower().Replace((Request.ApplicationPath.ToLower() == "/" ? " " : Request.ApplicationPath.ToLower()), "").ToString();

            //    if (ConfigurationManager.AppSettings["ServerSideViewState"].ToString().ToUpper() == "TRUE" && !lVSExPages.Exists(delegate(string s) { return s == currentPage; }))
            //    {
            //        //Check were we will save the ViewState Data

            //        if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "CACHE")
            //        {
            //            #region STORED IN CACHE
            //            //Store the ViewState on Cache
            //            Cache.Add(VSKey, viewState, null, System.DateTime.Now.AddMinutes(Session.Timeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            //            #endregion
            //            //The ViewStateData will be Saved on the SESSION

            //        }
            //        else if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "SESSION")
            //        {
            //            #region STORED IN SESSION
            //            DataTable VsDataTable = null;
            //            DataRow DbRow = null;

            //            //Check if the ViewState DataTable are on the Session

            //            if (Session["__VSDataTable"] == null)
            //            {
            //                //No, it's not. Create it...
            //                DataColumn[] PkColumn = new DataColumn[1]; ;
            //                DataColumn DbColumn = null;
            //                VsDataTable = new DataTable("VState");
            //                //Create the DataTable

            //                //Column 1 - Name: VSKey - PrimaryKey
            //                DbColumn = new DataColumn("VSKey", typeof(string));
            //                VsDataTable.Columns.Add(DbColumn);
            //                PkColumn[0] = DbColumn;
            //                VsDataTable.PrimaryKey = PkColumn;

            //                //Column 2 - Name: ViewStateData
            //                DbColumn = new DataColumn("VSData", typeof(object));
            //                VsDataTable.Columns.Add(DbColumn);

            //                //Column 3 - Name: DateTime
            //                DbColumn = new DataColumn("DateTime", typeof(System.DateTime));
            //                VsDataTable.Columns.Add(DbColumn);

            //            }
            //            else
            //            {
            //                //The ViewState DataTable is already on the UserSession
            //                VsDataTable = (DataTable)Session["__VSDataTable"];

            //            }

            //            //Check if we already have a ViewState saved with the same key. 
            //            //If yes, update it instead of creating a new row. 
            //            //(This is very dificult to happen)
            //            DbRow = VsDataTable.Rows.Find(VSKey);

            //            if ((DbRow != null))
            //            {
            //                //Row found!!! Update instead of creating a new one...
            //                DbRow["VsData"] = viewState;
            //            }
            //            else
            //            {
            //                //Create a new row...
            //                DbRow = VsDataTable.NewRow();
            //                DbRow["VSKey"] = VSKey;
            //                DbRow["VsData"] = viewState;
            //                DbRow["DateTime"] = System.DateTime.Now;
            //                VsDataTable.Rows.Add(DbRow);
            //            }

            //            //Check if our DataTable is OverSized...
            //            if (Convert.ToInt16(ConfigurationManager.AppSettings["ViewStateTableSize"]) < VsDataTable.Rows.Count)
            //            {
            //                VsDataTable.Rows[0].Delete();
            //                //Delete the 1st line.
            //            }

            //            //Store the DataTable on the Session.
            //            Session["__VSDataTable"] = VsDataTable;
            //            #endregion
            //        }
            //        else if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "FILE")
            //        {
            //            #region STORED IN FILE

            //            string url = Request.ServerVariables["Path_Info"];
            //            url = url.Replace("/", "_");

            //            // Place the file in a temp folder (with write permissions) 
            //            string fileName = "~/{0}/{1}_{2}_{3}.viewstate";
            //            fileName = Server.MapPath(String.Format(fileName, "ViewStateTemp", Session.SessionID, url, System.DateTime.Now.Ticks.ToString()));

            //            VSKey = fileName;

            //            StreamWriter sw = new StreamWriter(fileName);
            //            sw.Write(VSSerialize(viewState));
            //            sw.Close();

            //            #endregion

            //        }
            //        //Register a HiddenField on the Page, 
            //        //that contains ONLY the UniqueKey generated. 
            //        //With this, we'll be able to find with ViewState 
            //        //is from this page, by retrieving these value.
            //        ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", VSKey);


            //    }
            //    else
            //    {
            //        //Call the normal process.
            //        base.SavePageStateToPersistenceMedium(viewState);

            //    }

            //}

            //protected override object LoadPageStateFromPersistenceMedium()
            //{

            //    List<string> lVSExPages = new List<string>(ConfigurationManager.AppSettings["ViewStateExPages"].ToString().Split(','));

            //    string currentPage = Request.ServerVariables["Path_Info"].ToLower().Replace((Request.ApplicationPath.ToLower() == "/" ? " " : Request.ApplicationPath.ToLower()), "").ToString();

            //    if (ConfigurationManager.AppSettings["ServerSideViewState"].ToString().ToUpper() == "TRUE" && !lVSExPages.Exists(delegate(string s) { return s == currentPage; }))
            //    {
            //        #region Retrieve ViewState
            //        //ViewState UniqueKey
            //        string VSKey = string.Empty;
            //        VSKey = Request.Form["__VIEWSTATE_KEY"];

            //        //Request the Key from the page and validade it.

            //        if (!VSKey.StartsWith("VIEWSTATE_") && !VSKey.ToUpper().EndsWith(".VIEWSTATE"))
            //            throw new Exception("Invalid VIEWSTATE Key: " + VSKey);

            //        //Verify which <SPAN id=BABID_Results7>modality was used to save ViewState
            //        if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "CACHE")
            //        {
            //            #region STORED IN CACHE
            //            return Cache[VSKey];
            //            #endregion
            //        }
            //        else if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "SESSION")
            //        {
            //            #region STORED IN SESSION
            //            DataTable VsDataTable;
            //            DataRow DbRow;
            //            VsDataTable = (DataTable)Session["__VSDataTable"];

            //            DbRow = VsDataTable.Rows.Find(VSKey);

            //            if (DbRow == null)
            //            {
            //                throw new Exception("VIEWStateKey not Found. " + "Consider increasing the ViewStateTableSize" + " parameter on Web.Config file.");
            //            }

            //            return DbRow["VsData"];
            //            #endregion
            //        }
            //        else if (ConfigurationManager.AppSettings["ViewStateStore"].ToString().ToUpper() == "FILE")
            //        {
            //            #region STORED IN FILE

            //            string m_viewState;

            //            object viewStateBag;

            //            StreamReader sr = new StreamReader(VSKey);
            //            m_viewState = sr.ReadToEnd();
            //            sr.Close();

            //            try
            //            {
            //                viewStateBag = VSDeserialize(m_viewState);
            //            }
            //            catch
            //            {
            //                throw new HttpException("The View State is invalid or corrupted");
            //            }

            //            return viewStateBag;

            //            #endregion
            //        }
            //        else
            //        {
            //            throw new HttpException("ViewStateStore is invalid");
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        //Return the ViewState using the Norma Method
            //        return base.LoadPageStateFromPersistenceMedium();

            //    }
            //}

            //private object VSSerialize(object vw)
            //{
            //    LosFormatter m_formatter = new LosFormatter();
            //    StringWriter writer = new StringWriter();
            //    m_formatter.Serialize(writer, vw);

            //    return writer;
            //}
            //private object VSDeserialize(string vw)
            //{
            //    LosFormatter m_formatter = new LosFormatter();
            //    return m_formatter.Deserialize(vw);
            //}

        }

        //// ***********************************************************************
        //// Loads any saved view-state information into the Page object
        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    string VSKey = string.Empty;
        //    string m_viewState;
        //    LosFormatter m_formatter;

        //    object viewStateBag;

        //    VSKey = "VIEWSTATE_" + Session.SessionID + "_" + Request.RawUrl + "_" + DateTime.Now.Ticks.ToString();


        //    string file = GetFileName();
        //    StreamReader sr = new StreamReader(file);
        //    m_viewState = sr.ReadToEnd();
        //    sr.Close();

        //    m_formatter = new LosFormatter();

        //    try
        //    {
        //        viewStateBag = m_formatter.Deserialize(m_viewState);
        //    }
        //    catch
        //    {
        //        throw new HttpException("The View State is invalid or corrupted");
        //    }

        //    return viewStateBag;
        //}

        //// ***********************************************************************

        //// ***********************************************************************
        //// Saves any view-state information for the page
        //protected override void SavePageStateToPersistenceMedium(object viewStateBag)
        //{
        //    string file = GetFileName();
        //    StreamWriter sw = new StreamWriter(file);
        //    LosFormatter m_formatter = new LosFormatter();
        //    m_formatter.Serialize(sw, viewStateBag);
        //    sw.Close();
        //    return;
        //}

        //// ***********************************************************************

        // ***********************************************************************
        // Determines the file name based on Session ID and URL
        //private string GetFileName()
        //{
        //    string url = Request.ServerVariables["Path_Info"];
        //    url = url.Replace("/", "_");

        //    // Place the file in a temp folder (with write permissions) 
        //    string fileName = "~/{0}/{1}_{2}.viewstate";
        //    fileName = String.Format(fileName, "ViewStateTemp", Session.SessionID, url);
        //    return Server.MapPath(fileName);
        //}
        // ***********************************************************************

}

