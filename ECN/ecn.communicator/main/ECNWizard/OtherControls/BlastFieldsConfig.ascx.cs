using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class BlastFieldsConfig : System.Web.UI.UserControl
    {
        private DataTable BlastFieldsValue_DT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["BlastFieldsValue_DT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["BlastFieldsValue_DT"] = value;
            }
        }

        public int BlastFieldID
        {
            get
            {
                try
                {
                    return (int)ViewState["BlastFieldID"];
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                ViewState["BlastFieldID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void loadData()
        {
            lblBlastFieldsNameMessage.Text = "";
            lblBlastFieldsNameMessage.Visible = false;
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(BlastFieldID, currentUser);
            if (blastFieldsName != null)
            {
                txtBlastFieldsCustomName.Text = blastFieldsName.Name;
            }
            BlastFieldsValue_DT = ECN_Framework_BusinessLayer.Communicator.BlastFieldsValue.GetByBlastFieldID(BlastFieldID, currentUser);
            loadGrid();
        }

        private void loadGrid()
        {
            var result = (from src in BlastFieldsValue_DT.AsEnumerable()
                          where src.Field<bool>("IsDeleted") == false
                          select new
                          {
                              BlastFieldsValueID = src.Field<string>("BlastFieldsValueID"),
                              Value = src.Field<string>("Value"),
                              IsDeleted = src.Field<bool>("IsDeleted")
                          }).ToList();
            gvBlastFieldsValue.DataSource = result;
            gvBlastFieldsValue.DataBind();
            if (result.Count > 0)
                gvBlastFieldsValue.Visible = true;
            else
                gvBlastFieldsValue.Visible = false;
        }

        protected void gvBlastFieldsValue_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string BlastFieldsValueID = e.CommandArgument.ToString();
            if (e.CommandName == "ValueDelete")
            {
                foreach (DataRow dr in BlastFieldsValue_DT.AsEnumerable())
                {
                    if (dr["BlastFieldsValueID"].Equals(BlastFieldsValueID))
                    {
                        dr["IsDeleted"] = true;
                    }
                }
                loadGrid();
            }
        }

        protected void btnAddBlastFieldsValue_Click(object sender, EventArgs e)
        {
            DataTable dt = BlastFieldsValue_DT;
            DataRow dr = dt.NewRow();
            dr["BlastFieldsValueID"] = Guid.NewGuid();
            dr["Value"] = txtBlastFieldsValue.Text;
            dr["IsDeleted"] = false;
            dt.Rows.Add(dr);
            txtBlastFieldsValue.Text = "";
            BlastFieldsValue_DT = dt;
            loadGrid();
        }

        public void save()
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName;
            blastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(BlastFieldID, currentUser);
            if (blastFieldsName != null)
            {
                blastFieldsName.UpdatedUserID = currentUser.UserID;
            }
            else
            {
                blastFieldsName = new ECN_Framework_Entities.Communicator.BlastFieldsName();
                blastFieldsName.BlastFieldID = BlastFieldID;
                blastFieldsName.CreatedUserID = currentUser.UserID;
            }
            blastFieldsName.CustomerID = currentUser.CustomerID;
            blastFieldsName.Name = txtBlastFieldsCustomName.Text;

            ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.Save(blastFieldsName, currentUser);


            if (BlastFieldsValue_DT != null && BlastFieldsValue_DT.Rows.Count > 0)
            {
                foreach (DataRow dr in BlastFieldsValue_DT.AsEnumerable())
                {
                    string isDeleted = dr["IsDeleted"].ToString();
                    if (dr["BlastFieldsValueID"].ToString().Contains("-") && isDeleted.Equals("False"))
                    {
                        ECN_Framework_Entities.Communicator.BlastFieldsValue blastFieldsValue = new ECN_Framework_Entities.Communicator.BlastFieldsValue();
                        blastFieldsValue.BlastFieldID = BlastFieldID;
                        blastFieldsValue.CreatedUserID = currentUser.UserID;
                        blastFieldsValue.Value = dr["Value"].ToString();
                        blastFieldsValue.CustomerID = currentUser.CustomerID;
                        ECN_Framework_BusinessLayer.Communicator.BlastFieldsValue.Save(blastFieldsValue, currentUser);
                    }
                    if (isDeleted.Equals("True") && !dr["BlastFieldsValueID"].ToString().Contains("-"))
                    {
                        ECN_Framework_BusinessLayer.Communicator.BlastFieldsValue.Delete(Convert.ToInt32(dr["BlastFieldsValueID"].ToString()), currentUser);
                    }
                }
            }
        }

        internal void Reset()
        {
            txtBlastFieldsCustomName.Text = "";
            BlastFieldsValue_DT = new DataTable();
            BlastFieldID = -1;
            loadGrid();
        }
    }
}