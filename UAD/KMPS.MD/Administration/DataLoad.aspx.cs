using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KM.Common;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using KMPS.MD.Administration.Models;
using KMPS.MD.Constants;

namespace KMPS.MD.Administration
{
    public partial class DataLoad : KMPS.MD.Main.WebPageHelper
    {
        private const int TimerInterval = 2000;
        private const int ErrorLevelThreshold = 10;
        private const string TextDisabled = "Disabled";
        private const string TextStart = "Start";
        private const string TextDone = "Done";
        private const string TextSuccess = "successfully processed";
        private const string TextAllProcessed = "100 percent processed";
        private const string StatusStarted = "Started....please wait.";
        private const string Step1Prefix = "Step 1 is<br/>";
        private const string ProcedureDataRefreshPart7 = "spDataRefreshPart7";

        private static readonly DataLoadMessages ProdStatus = new DataLoadMessages();
        private static readonly DataLoadMessages RefreshStatus = new DataLoadMessages();

        private DataLoadComponent _prodComponent;
        private DataLoadComponent _refreshComponent;

        private static string prd1Status
        {
            get { return ProdStatus.Step1Status; }
            set { ProdStatus.Step1Status = value; }
        }

        private static string prd2Status
        {
            get { return ProdStatus.Step2Status; }
            set { ProdStatus.Step2Status = value; }
        }

        private static string prd3Status
        {
            get { return ProdStatus.Step3Status; }
            set { ProdStatus.Step3Status = value; }

        }

        private static string prd4Status
        {
            get { return ProdStatus.Step4Status; }
            set { ProdStatus.Step4Status = value; }
        }

        private static string ref1Status
        {
            get { return RefreshStatus.Step1Status; }
            set { RefreshStatus.Step1Status = value; }
        }

        private static string ref2Status
        {
            get { return RefreshStatus.Step2Status; }
            set { RefreshStatus.Step2Status = value; }
        }

        private static string ref3Status
        {
            get { return RefreshStatus.Step3Status; }
            set { RefreshStatus.Step3Status = value; }
        }

        private static string ref4Status
        {
            get { return RefreshStatus.Step4Status; }
            set { RefreshStatus.Step4Status = value; }
        }

        private bool prd1Done
        {
            get { return _prodComponent.Step1Done; }
            set { _prodComponent.Step1Done = value; }
        }

        private bool prd1Continue
        {
            get { return _prodComponent.Step1Continue; }
            set { _prodComponent.Step1Continue = value; }
        }

        private bool prd2Done
        {
            get { return _prodComponent.Step2Done; }
            set { _prodComponent.Step2Done = value; }
        }

        private bool prd3Done
        {
            get { return _prodComponent.Step3Done; }
            set { _prodComponent.Step3Done = value; }
        }

        private bool prd4Done
        {
            get { return _prodComponent.Step4Done; }
            set { _prodComponent.Step4Done = value; }
        }

        private bool ref1Done
        {
            get { return _refreshComponent.Step1Done; }
            set { _refreshComponent.Step1Done = value; }
        }

        private bool ref1Continue
        {
            get { return _refreshComponent.Step1Continue; }
            set { _refreshComponent.Step1Continue = value; }
        }

        private bool ref2Done
        {
            get { return _refreshComponent.Step2Done; }
            set { _refreshComponent.Step2Done = value; }
        }

        private bool ref3Done
        {
            get { return _refreshComponent.Step3Done; }
            set { _refreshComponent.Step3Done = value; }
        }

        private bool ref4Done
        {
            get { return _refreshComponent.Step4Done; }
            set { _refreshComponent.Step4Done = value; }
        }

        private double EstimatedRestoreSeconds
        {
            get
            {
                return (double?)Session[SessionConsts.EstimatedRestoreSeconds] ?? 0;
            }
            set
            {
                Session[SessionConsts.EstimatedRestoreSeconds] = value;
            }
        }
        //private string filePath = @"\\10.10.41.250\Backups\MAF\";  moved to web.config
        private string datestamp = DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString();
        private TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Data";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!Page.IsPostBack)
            {
                LoadProdDatabase();
                LoadRereshDatabase();
            }

            InitializeDataComponents();
        }

        private void InitializeDataComponents()
        {
            _prodComponent = new DataLoadComponent(Session, "prd")
            {
                ActionButton = btnPrd,
                DbDropDownList = ddlPrd,
                Messages = ProdStatus
            };
            _refreshComponent = new DataLoadComponent(Session, "ref")
            {
                ActionButton = btnRefresh,
                DbDropDownList = ddlRefresh,
                Messages = RefreshStatus
            };
        }

        #region Prod to Refresh
        private void LoadProdDatabase()
        {
            ddlPrd.Items.Clear();
            string prodDB = "SELECT name FROM master..sysdatabases WHERE name like '%MasterDB%' AND Name not like '%_Refresh'";
            List<string> listDB = new List<string>();
            try
            {
                using (SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections))
                {
                    SqlCommand cmd = new SqlCommand(prodDB, conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listDB.Add(rdr.GetString(0));
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
            listDB = listDB.OrderBy(x => x).ToList();
            ddlPrd.DataSource = listDB;
            ddlPrd.DataBind();

            ddlPrd.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void btnPrd_Click(object sender, EventArgs e)
        {
            StartProcess(_prodComponent, false);
        }

        private void Prd1_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed") || e.Message.Equals("Warning: Null value is eliminated by an aggregate or other SET operation."))
                    {
                        prd1Status = "Done";
                        prd1Done = true;
                    }
                    else
                    {
                        prd1Status = "working..."; //e.Message;
                        prd1Done = false;
                    }
                }
            }
        }
        private void Prd2_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed"))
                    {
                        prd2Status = "Done";
                        prd2Done = true;
                    }
                    else
                    {
                        prd2Status = e.Message;
                        prd2Done = false;
                    }
                }
            }
        }
        private void Prd3_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed"))
                    {
                        prd3Status = "Done";
                        prd3Done = true;
                    }
                    else
                    {
                        prd3Status = e.Message;
                        prd3Done = false;
                    }
                }
            }
        }
        private void Prd4_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            ProcessStep4InfoMessage(_prodComponent, e);
        }
        #endregion

        private void RunCmd(TaskScheduler uiScheduler, SqlConnection conn, string sproc)
        {
            Task.Factory.StartNew(() =>
            {
                SqlCommand myCmd = new SqlCommand(sproc, conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 0;
                myCmd.ExecuteNonQuery();
                conn.Close();
                //conn.InfoMessage -= Prd_OnInfoMessage;
                //conn.FireInfoMessageEventOnUserErrors = false;

            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
        }
        private void RunCmd(TaskScheduler uiScheduler, SqlConnection conn, string sproc, string dbName, string file)
        {
            Task.Factory.StartNew(() =>
            {
                SqlCommand myCmd = new SqlCommand(sproc, conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 0;
                myCmd.Parameters.AddWithValue("@DbName", dbName);
                myCmd.Parameters.AddWithValue("@File", file);

                myCmd.ExecuteNonQuery();
                conn.Close();
                //conn.InfoMessage -= Prd_OnInfoMessage;
                //conn.FireInfoMessageEventOnUserErrors = false;

            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
        }
        private void RunRestoreCmd(TaskScheduler uiScheduler, SqlConnection conn, string sproc, string dbName, string file, string backupMDF, string backupLDF, string savePath)
        {
            Task.Factory.StartNew(() =>
            {
                SqlCommand myCmd = new SqlCommand(sproc, conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 0;
                myCmd.Parameters.AddWithValue("@DbName", dbName);
                myCmd.Parameters.AddWithValue("@File", file);
                myCmd.Parameters.AddWithValue("@BackupMDF", backupMDF);
                myCmd.Parameters.AddWithValue("@BackupLDF", backupLDF);
                myCmd.Parameters.AddWithValue("@SavePath", savePath);

                myCmd.ExecuteNonQuery();
                conn.Close();
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
        }
        private string SqlError(SqlError ex)
        {
            System.Text.StringBuilder sbEx = new System.Text.StringBuilder();
            try
            {
                sbEx.AppendLine("**********************");

                if (ex.Server != null)
                {
                    sbEx.AppendLine("-- Server --");
                    sbEx.AppendLine("Server: " + ex.Server.ToString());
                }

                if (ex.Procedure != null)
                {
                    sbEx.AppendLine("-- Procedure --");
                    sbEx.AppendLine("Procedure: " + ex.Procedure.ToString());
                }

                //if (ex.Class != null)
                {
                    sbEx.AppendLine("-- Class --");
                    sbEx.AppendLine("Class: " + ex.Class.ToString());
                }

               //if (ex.LineNumber != null)
                {
                    sbEx.AppendLine("-- LineNumber --");
                    sbEx.AppendLine("LineNumber: " + ex.LineNumber.ToString());
                }

                if (ex.Source != null)
                {
                    sbEx.AppendLine("-- Source --");
                    sbEx.AppendLine(ex.Source);
                }

                if (ex.Message != null)
                {
                    sbEx.AppendLine("-- Message --");
                    sbEx.AppendLine(ex.Message);
                }

                //if (ex.Number != null)
                {
                    sbEx.AppendLine("-- Number --");
                    sbEx.AppendLine("Number: " + ex.Number.ToString());
                }

                //if (ex.State != null)
                {
                    sbEx.AppendLine("-- State --");
                    sbEx.AppendLine(ex.State.ToString());
                }
                sbEx.AppendLine("**********************");
            }
            catch
            {
                sbEx.Clear();
                sbEx.Append(ex.ToString());
            }
            return sbEx.ToString();
        }
        private string GetConnection(string selectedDB)
        {
            string connString = string.Empty;
            foreach (ConnectionStringSettings s in ConfigurationManager.ConnectionStrings)
            {
                if (s.Name.Equals(selectedDB.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    connString = s.ConnectionString;
            }
            return connString;
        }
        #region Refresh to Prod
        private void LoadRereshDatabase()
        {
            ddlRefresh.Items.Clear();
            string refreshDB = "SELECT name FROM master..sysdatabases WHERE name like '%_Refresh'";
            List<string> listDB = new List<string>();
            try
            {
                using (SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections))
                {
                    SqlCommand cmd = new SqlCommand(refreshDB, conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listDB.Add(rdr.GetString(0));
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
            listDB = listDB.OrderBy(x => x).ToList();
            ddlRefresh.DataSource = listDB;
            ddlRefresh.DataBind();
            ddlRefresh.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            StartProcess(_refreshComponent, true);
        }

        private void Ref1_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed") || e.Message.Equals("Warning: Null value is eliminated by an aggregate or other SET operation."))
                    {
                        ref1Status = "Done";
                        ref1Done = true;
                    }
                    else
                    {
                        ref1Status = "working...";// e.Message;
                        ref1Done = false;
                    }
                }
            }
        }
        private void Ref2_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed"))
                    {
                        ref2Status = "Done";
                        ref2Done = true;
                    }
                    else
                    {
                        ref2Status = e.Message;
                        ref2Done = false;
                    }
                }
            }
        }
        private void Ref3_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError info in e.Errors)
            {
                if (info.Class > 10)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (e.Message.Contains("successfully processed"))
                    {
                        ref3Status = "Done";
                        ref3Done = true;
                    }
                    else
                    {
                        ref3Status = e.Message;
                        ref3Done = false;
                    }
                }
            }
        }
        private void Ref4_OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            ProcessStep4InfoMessage(_refreshComponent, e);
        }
        #endregion

        protected void timer_Tick(object sender, EventArgs e)
        {
            string filePath = ConfigurationManager.AppSettings["BackupSQL_FilePath"].ToString();

            #region Prod to Refresh
            if (!string.IsNullOrEmpty(prd1Status))
                litPrd1.Text = "Step 1 is<br/>" + prd1Status;
            if (!string.IsNullOrEmpty(prd2Status))
                litPrd2.Text = "Step 2 is<br/>" + prd2Status;
            if (!string.IsNullOrEmpty(prd3Status))
                litPrd3.Text = "Step 3 is<br/>" + prd3Status;
            if (!string.IsNullOrEmpty(prd4Status))
                litPrd4.Text = "Step 4 is<br/>" + prd4Status;

            if (prd1Done == true && prd1Continue == false)
            {
                prd1Status = "Done";

                prd1Continue = true;

                string prdDB = ddlPrd.SelectedItem.ToString();
                string refreshDB = ddlPrd.SelectedItem.ToString() + "_Refresh";
                string prdFile = filePath + prdDB + "_" + datestamp + ".bak";
                string refreshFile = filePath + refreshDB + "_" + datestamp + ".bak";

                //backup Prd database
                SqlConnection conStep2 = new SqlConnection(GetConnection(prdDB));
                conStep2.FireInfoMessageEventOnUserErrors = true;
                conStep2.InfoMessage += Prd2_OnInfoMessage;
                conStep2.Open();
                Task.Factory.StartNew(() => RunCmd(uiScheduler, conStep2, "job_MasterDB_Backup", prdDB, prdFile));
                prd2Status = "Started....please wait.";
                //prd2Done = true;

                //backup Refresh database
                SqlConnection conStep3 = new SqlConnection(GetConnection(prdDB));
                conStep3.FireInfoMessageEventOnUserErrors = true;
                conStep3.InfoMessage += Prd3_OnInfoMessage;
                conStep3.Open();
                Task.Factory.StartNew(() => RunCmd(uiScheduler, conStep3, "job_RefreshDB_Backup", refreshDB, refreshFile));
                prd3Status = "Started....please wait.";
                //prd3Done = true;
            }

            if (prd1Done == true && prd2Done == true && prd3Done == true)
            {
                prd1Done = false;
                prd2Done = false;
                prd3Done = false;

                string prdDB = ddlPrd.SelectedItem.ToString();
                string refreshDB = ddlPrd.SelectedItem.ToString() + "_Refresh";
                string prdFile = filePath + prdDB + "_" + datestamp + ".bak";
                string refreshFile = filePath + refreshDB + "_" + datestamp + ".bak";

                //get Logical File names from backup file
                List<SqlBakInfo> list = SqlBakInfo.GetFileInfo(prdFile, GetConnection(prdDB)).ToList();
                EstimatedRestoreSeconds = (list.First().BackupSizeInBytes / 1048576) / 10;

                string backupMDF = list.First().LogicalName;
                string backupLDF = list.Last().LogicalName;
                string physName = list.First().PhysicalName.ToString();
                string savePath = physName.Substring(0, physName.LastIndexOf(@"\"));
                if (!savePath.EndsWith(@"\"))
                    savePath += @"\";

                SqlConnection conStep4 = new SqlConnection(GetConnection(prdDB));
                conStep4.FireInfoMessageEventOnUserErrors = true;
                conStep4.InfoMessage += Prd4_OnInfoMessage;
                conStep4.Open();
                Task.Factory.StartNew(() => RunRestoreCmd(uiScheduler, conStep4, "job_RefreshDB_Restore", refreshDB, prdFile, backupMDF, backupLDF, savePath));

                litPrd4.Text = "Step 4 <br/>approx. seconds remaining<br/>" + EstimatedRestoreSeconds.ToString();
            }

            if (EstimatedRestoreSeconds > 0 && prd3Status.Equals("Done"))
            {
                EstimatedRestoreSeconds = EstimatedRestoreSeconds - 2;
                litPrd4.Text = "Step 4 <br/>approx. seconds remaining<br/>" + EstimatedRestoreSeconds.ToString();
            }
            else if (EstimatedRestoreSeconds < 0 && prd3Status.Equals("Done"))
            {
                EstimatedRestoreSeconds = EstimatedRestoreSeconds - 2;
                litPrd4.Text = "Step 4 <br/>finishing up";
            }

            if (prd4Done == true)
            {
                prd4Done = false;
                timer.Enabled = false;

                ddlPrd.Enabled = true;
                ddlRefresh.Enabled = true;
                btnPrd.Enabled = true;
                btnRefresh.Enabled = true;
                btnRefresh.Text = "Start";
                btnPrd.Text = "Start";
                divError.Visible = false;
                lblErrorMessage.Text = string.Empty;
            }

            if (EstimatedRestoreSeconds < -120)
            {
                prd4Done = true;
                prd4Status = "Done";
            }
            #endregion

            #region Refresh to Prod
            if (!string.IsNullOrEmpty(ref1Status))
                litRef1.Text = "Step 1 is<br/>" + ref1Status;
            if (!string.IsNullOrEmpty(ref2Status))
                litRef2.Text = "Step 2 is<br/>" + ref2Status;
            if (!string.IsNullOrEmpty(ref3Status))
                litRef3.Text = "Step 3 is<br/>" + ref3Status;
            if (!string.IsNullOrEmpty(ref4Status))
                litRef4.Text = "Step 4 is<br/>" + ref4Status;

            if (ref1Done == true && ref1Continue == false)
            {
                ref1Continue = true;

                string prdDB = ddlRefresh.SelectedItem.ToString().Replace("_Refresh", "");
                string refreshDB = ddlRefresh.SelectedItem.ToString();
                string prdFile = filePath + prdDB + "_" + datestamp + ".bak";
                string refreshFile = filePath + refreshDB + "_" + datestamp + ".bak";

                //backup Prd database
                SqlConnection conStep2 = new SqlConnection(GetConnection(prdDB));
                conStep2.FireInfoMessageEventOnUserErrors = true;
                conStep2.InfoMessage += Ref2_OnInfoMessage;
                conStep2.Open();
                Task.Factory.StartNew(() => RunCmd(uiScheduler, conStep2, "job_MasterDB_Backup", prdDB, prdFile));

                //backup Refresh database
                SqlConnection conStep3 = new SqlConnection(GetConnection(prdDB));
                conStep3.FireInfoMessageEventOnUserErrors = true;
                conStep3.InfoMessage += Ref3_OnInfoMessage;
                conStep3.Open();
                Task.Factory.StartNew(() => RunCmd(uiScheduler, conStep3, "job_RefreshDB_Backup", refreshDB, refreshFile));
            }
            if (ref1Done == true && ref2Done == true && ref3Done == true)
            {
                ref1Done = false;
                ref2Done = false;
                ref3Done = false;

                string prdDB = ddlRefresh.SelectedItem.ToString().Replace("_Refresh", "");
                string refreshDB = ddlRefresh.SelectedItem.ToString();
                string prdFile = filePath + prdDB + "_" + datestamp + ".bak";
                string refreshFile = filePath + refreshDB + "_" + datestamp + ".bak";

                //get Logical File names from backup file
                List<SqlBakInfo> list = SqlBakInfo.GetFileInfo(refreshFile, GetConnection(prdDB)).ToList();
                EstimatedRestoreSeconds = (list.First().BackupSizeInBytes / 1048576) / 10;
                string backupMDF = list.First().LogicalName;
                string backupLDF = list.Last().LogicalName;
                string physName = list.First().PhysicalName.ToString();
                string savePath = physName.Substring(0, physName.LastIndexOf(@"\"));
                if (!savePath.EndsWith(@"\"))
                    savePath += @"\";

                SqlConnection conStep4 = new SqlConnection(GetConnection(prdDB));
                conStep4.FireInfoMessageEventOnUserErrors = true;
                conStep4.InfoMessage += Prd4_OnInfoMessage;
                conStep4.Open();
                Task.Factory.StartNew(() => RunRestoreCmd(uiScheduler, conStep4, "job_MasterDB_Restore", prdDB, refreshFile, backupMDF, backupLDF, savePath));

                litRef4.Text = "Step 4 <br/>approx. seconds remaining<br/>" + EstimatedRestoreSeconds.ToString();
            }

            if (EstimatedRestoreSeconds > 0 && ref3Status.Equals("Done"))
            {
                EstimatedRestoreSeconds = EstimatedRestoreSeconds - 2;
                litRef4.Text = "Step 4 <br/>approx. seconds remaining<br/>" + EstimatedRestoreSeconds.ToString();
            }
            else if (EstimatedRestoreSeconds < 0 && ref3Status.Equals("Done"))
            {
                EstimatedRestoreSeconds = EstimatedRestoreSeconds - 2;
                litRef4.Text = "Step 4 <br/>finishing up";
            }

            if (ref4Done == true)
            {
                ref4Done = false;
                timer.Enabled = false;

                ddlPrd.Enabled = true;
                ddlRefresh.Enabled = true;
                btnPrd.Enabled = true;
                btnRefresh.Enabled = true;
                btnRefresh.Text = "Start";
                btnPrd.Text = "Start";
                divError.Visible = false;
                lblErrorMessage.Text = string.Empty;
            }

            if (EstimatedRestoreSeconds < -120)
            {
                ref4Done = true;
                ref4Status = "Done";
            }
            #endregion
        }

        private void StartProcess(DataLoadComponent component, bool isRefresh)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            SetMessage(string.Empty);

            if (component.DbDropDownList.SelectedIndex > 0)
            {
                SetMessage("Page controls will be disabled until all 4 steps are complete.");
                component.Reset();
                DisableControls();
                timer.Interval = TimerInterval;
                timer.Enabled = true;

                var selectedDbItem = ddlPrd.SelectedItem.ToString();
                var connectionString = GetConnection(selectedDbItem);

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    if (isRefresh)
                    {
                        //step one
                        var connection = new SqlConnection(connectionString);
                        connection.FireInfoMessageEventOnUserErrors = true;
                        connection.InfoMessage += Ref1_OnInfoMessage;
                        connection.Open();
                        Task.Factory.StartNew(() => RunCmd(uiScheduler, connection, ProcedureDataRefreshPart7));

                        component.Messages.Step1Status = StatusStarted;
                        litRef1.Text = $"{Step1Prefix}{ref1Status}";
                    }
                    else
                    {
                        //don't have to run the step 7 sproc so there is no step one
                        component.Step1Done = true;
                        component.Messages.Step1Status = StatusStarted;
                    }
                }
                else
                {
                    SetMessage("Database connection string could not be set");

                    if (isRefresh)
                    {
                        btnRefresh.Text = TextStart;
                        btnPrd.Text = TextStart;
                    }
                }
            }
            else
            {
                SetMessage("You must select a database");
            }
        }

        private void DisableControls()
        {
            ddlPrd.Enabled = false;
            ddlRefresh.Enabled = false;
            btnPrd.Enabled = false;
            btnRefresh.Enabled = false;
            btnRefresh.Text = TextDisabled;
            btnPrd.Text = TextDisabled;
        }

        private void SetMessage(string message)
        {
            divError.Visible = !string.IsNullOrWhiteSpace(message);
            lblErrorMessage.Text = message;
        }

        private void ProcessStep4InfoMessage(DataLoadComponent component, SqlInfoMessageEventArgs eventArgs)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            foreach (var info in eventArgs.Errors.OfType<SqlError>())
            {
                if (info.Class > ErrorLevelThreshold)
                {
                    // TODO: treat this as a genuine error
                    divError.Visible = true;
                    lblErrorMessage.Text = SqlError(info);
                }
                else
                {
                    // TODO: treat this as a progress message
                    if (eventArgs.Message.IndexOf(TextSuccess, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        eventArgs.Message.IndexOf(TextAllProcessed, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        component.Messages.Step4Status = TextDone;
                        component.Step4Done = true;
                    }
                    else
                    {
                        component.Messages.Step4Status = eventArgs.Message;
                        component.Step4Done = false;
                    }
                }
            }
        }
    }

    public class SqlBakInfo
    {
        public SqlBakInfo() { }

        #region Properties
        public string LogicalName { get; set; }
        public string PhysicalName { get; set; }
        public string Type { get; set; }
        public string FileGroupName { get; set; }
        public int Size { get; set; }
        public int MaxSize { get; set; }
        public int FileId { get; set; }
        public bool CreateLSN { get; set; }
        public bool DropLSN { get; set; }
        public System.Guid UniqueId { get; set; }
        public bool ReadOnlyLSN { get; set; }
        public bool ReadWriteLSN { get; set; }
        public int BackupSizeInBytes { get; set; }
        public int SourceBlockSize { get; set; }
        public int FileGroupId { get; set; }
        public System.Guid LogGroupGUID { get; set; }
        public int DifferentialBaseLSN { get; set; }
        public System.Guid DifferentialBaseGUID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPresent { get; set; }
        public string TDEThumbprint { get; set; }

        #endregion

        public static List<SqlBakInfo> GetFileInfo(string file, string sqlConnection)
        {
            string sql = "RESTORE FILELISTONLY FROM DISK = '" + file + "'";
            List<SqlBakInfo> retList = new List<SqlBakInfo>();
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SqlBakInfo> builder = DynamicBuilder<SqlBakInfo>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    SqlBakInfo x = builder.Build(rdr);
                    
                    retList.Add(x);
                }
                rdr.Close();
                rdr.Dispose();
                cmd.Connection.Close();
            }
            return retList;
        }
    }
}