using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ecn.common.classes;
using System.Collections;

namespace ecn.publisher.classes
{
	public class Edition: StatusItem
	{
		private int _id = 0;
		private int _Publicationid = 0;
        private string _PublicationType = string.Empty;
        private int _Customerid = 0;
		private string _editionname = string.Empty;
		private string _filename = string.Empty;
		private string _toc = string.Empty;
        private string _theme = string.Empty;
		private string _activateDate = string.Empty;
		private string _deactivatedate = string.Empty;
		private bool _IsSearchEnabled = false;
		private bool _IsLoginRequired = false;
		

		private Pages _pages;
	
		public Edition()
		{
			_pages = new Pages();
		}

		public int ID 
		{
			get {return (_id);}
			set {_id = value;}
		}


		public int PublicationID 
		{
			get {return (_Publicationid);}
			set {_Publicationid = value;}
		}


        public int CustomerID
		{
            get { return (_Customerid); }
            set { _Customerid = value; }
		}

        public string PublicationType
        {
            get { return (_PublicationType); }
            set { _PublicationType = value; }
        }

		public string EditionName
		{
			get {return (_editionname);}
			set {_editionname = value;}
		}

        public string theme
		{
			get {return (_theme);}
			set {_theme = value;}
		}

		public string FileName
		{
			get {return (_filename);}
			set {_filename = value;}
		}


		public string ActivationDate
		{
			get {return (_activateDate);}
			set {_activateDate = value;}
		}


		public string DeActivationDate
		{
			get {return (_deactivatedate);}
			set {_deactivatedate = value;}
		}
		
		public bool IsSearchEnabled
		{
			get {return (_IsSearchEnabled);}
			set {_IsSearchEnabled = value;}
		}

		public bool IsLoginRequired
		{
			get {return (_IsLoginRequired);}
			set {_IsLoginRequired = value;}
		}

		public string TableofContents
		{
			get {return (_toc);}
			set {_toc = value;}
		}
		

		public Pages PageCollection
		{
			get 
			{
				return (this._pages);
			}
		}

		public void AddPage(Page page) 
		{
			_pages.Add(page);
		}

		public bool Exists(string EditionName)
		{
			return Exists(this.ID, EditionName);
		}

		public bool Exists(int PublicationID, string EditionName)
		{
			//return Convert.ToBoolean(DataFunctions.ExecuteScalar("if exists(select EditionID from Edition where PublicationID = " + PublicationID + " and EditionName = '" +  EditionName.Replace("'","''") + "') 1 else 0 "));
			return false;
		}

		public int Save()
		{
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["pub"].ToString());

			SqlCommand cmd = new SqlCommand("dbo.sp_SaveEdition", conn);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@EditionID", SqlDbType.Int));
			cmd.Parameters["@EditionID"].Value = _id;		
			cmd.Parameters.Add(new SqlParameter("@EditionName", SqlDbType.VarChar));
			cmd.Parameters["@EditionName"].Value = _editionname;	
			cmd.Parameters.Add(new SqlParameter("@PublicationID", SqlDbType.Int));
			cmd.Parameters["@PublicationID"].Value = _Publicationid;	
			cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
			cmd.Parameters["@status"].Value = Status;	
			cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.VarChar));
			cmd.Parameters["@filename"].Value = _filename;	
			cmd.Parameters.Add(new SqlParameter("@Totalpages", SqlDbType.Int));
			cmd.Parameters["@Totalpages"].Value = _pages.Count;	
			cmd.Parameters.Add(new SqlParameter("@ActivateDate", SqlDbType.VarChar));
			cmd.Parameters["@ActivateDate"].Value = _activateDate;	
			cmd.Parameters.Add(new SqlParameter("@DeactivateDate", SqlDbType.VarChar));
			cmd.Parameters["@DeactivateDate"].Value = _deactivatedate;				
			cmd.Parameters.Add(new SqlParameter("@IsSearchEnabled", SqlDbType.Bit));
			cmd.Parameters["@IsSearchEnabled"].Value = _IsSearchEnabled?1:0;	
			cmd.Parameters.Add(new SqlParameter("@IsLoginRequired", SqlDbType.Bit));
			cmd.Parameters["@IsLoginRequired"].Value = _IsLoginRequired?1:0;	
			cmd.Parameters.Add(new SqlParameter("@TOC", SqlDbType.Text));
			cmd.Parameters["@TOC"].Value =_toc;	
			try
			{
				conn.Open();
				this._id = Convert.ToInt32(cmd.ExecuteScalar());	
				conn.Close();
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
			return _id;		
		}

		public int ReUpload()
		{
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["pub"].ToString());

			SqlCommand cmd = new SqlCommand("dbo.sp_UpdateEdition", conn);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@EditionID", SqlDbType.Int));
			cmd.Parameters["@EditionID"].Value = _id;		
			cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.VarChar));
			cmd.Parameters["@filename"].Value = _filename;	
			cmd.Parameters.Add(new SqlParameter("@Totalpages", SqlDbType.Int));
			cmd.Parameters["@Totalpages"].Value = _pages.Count;	
			cmd.Parameters.Add(new SqlParameter("@TOC", SqlDbType.Text));
			cmd.Parameters["@TOC"].Value =_toc;	
			try
			{
				conn.Open();
				this._id = Convert.ToInt32(cmd.ExecuteScalar());	
				conn.Close();
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
			return _id;		
		}

		public void Delete(int EditionID) 
		{
            SqlCommand cmd = new SqlCommand("Delete from editionhistory where EditionID = " + EditionID + ";Delete from editionactivitylog where EditionID = " + EditionID + ";Delete from link where PageID in (select pageID from Page where EditionID = " + EditionID + ");Delete from Page where EditionID = " + EditionID + ";Delete from Edition where EditionID = " + EditionID);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.Text;

			DataFunctions.Execute("publisher",cmd);
			
		}

        public void Remove(int EditionID)
        {
            SqlCommand cmd = new SqlCommand("Delete from editionactivitylog where EditionID = " + EditionID + ";Delete from link where PageID in (select pageID from Page where EditionID = " + EditionID + ");Delete from Page where EditionID = " + EditionID);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            DataFunctions.Execute("publisher", cmd);

        }

		public static Edition GetEditionbyID(int editionID)
		{
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["pub"].ToString());

                SqlCommand cmd = new SqlCommand("select e.* , p.customerID, p.PublicationType from edition e join publication p on e.publicationID = p.publicationID where EditionID = " + editionID + ";select * from page where editionID = " + editionID + " order by PageNumber; select * from link where pageID in (select pageID from page where editionID = " + editionID + ");", conn);
				cmd.CommandTimeout = 0;
				cmd.CommandType = CommandType.Text;

				SqlDataAdapter da	= new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();			
				conn.Open();
				da.Fill(ds);
				conn.Close();

				if (ds.Tables[0].Rows.Count != 0) 
				{
					Edition ed = new Edition();

					ed.ID = editionID;
					ed.EditionName= ds.Tables[0].Rows[0]["EditionName"].ToString();
                    ed.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[0]["customerID"]);
					ed.FileName = ds.Tables[0].Rows[0]["FileName"].ToString();
					ed.PublicationID = Convert.ToInt32(ds.Tables[0].Rows[0]["PublicationID"]);
                    ed.PublicationType = ds.Tables[0].Rows[0]["PublicationType"].ToString();
					ed.Status = ds.Tables[0].Rows[0]["Status"].ToString();
					ed.TableofContents = ds.Tables[0].Rows[0].IsNull("xmlTOC")?"":ds.Tables[0].Rows[0]["xmlTOC"].ToString();
					ed.IsSearchEnabled = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSearchable"]);
					ed.ActivationDate = ds.Tables[0].Rows[0].IsNull("EnableDate")?"":ds.Tables[0].Rows[0]["EnableDate"].ToString();
					ed.DeActivationDate = ds.Tables[0].Rows[0].IsNull("DisableDate")?"":ds.Tables[0].Rows[0]["DisableDate"].ToString();
                    ed.theme = ds.Tables[0].Rows[0]["theme"].ToString();

                    try
                    {
                        ed.IsLoginRequired = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLoginRequired"]);
                    }
                    catch {
                        ed.IsLoginRequired = false;
                    }



					if (ds.Tables[1].Rows.Count != 0)
					{
						for(int i=0;i<ds.Tables[1].Rows.Count;i++)
						{
							ecn.publisher.classes.Page page = new ecn.publisher.classes.Page();

							page.ID = Convert.ToInt32(ds.Tables[1].Rows[i]["PageID"]);
							page.EditionID = editionID;
							page.PageNo = Convert.ToInt32(ds.Tables[1].Rows[i]["PageNumber"]);
							page.DisplayNo = ds.Tables[1].Rows[i]["DisplayNumber"].ToString();
							page.Width = Convert.ToInt32(ds.Tables[1].Rows[i]["Width"]);
							page.Height = Convert.ToInt32(ds.Tables[1].Rows[i]["Height"]);
							page.TextContent = ds.Tables[1].Rows[i].IsNull("TextContent")?"":ds.Tables[1].Rows[i]["TextContent"].ToString();

							if (ds.Tables[2].Rows.Count != 0)
							{
								DataView dv = ds.Tables[2].DefaultView;
								dv.RowFilter = "PageID = '" + ds.Tables[1].Rows[i]["PageID"].ToString() + "'";

								IEnumerator iterator = dv.GetEnumerator();
								DataRowView drv;

								while(iterator.MoveNext())
								{      
									drv = (DataRowView)iterator.Current;

									ecn.publisher.classes.Link lk = new ecn.publisher.classes.Link();
								
									lk.PageID = Convert.ToInt32(ds.Tables[1].Rows[i]["PageID"]);
									lk.LinkType = drv.Row["LinkType"].ToString();
									lk.LinkURL = drv.Row["LinkURL"].ToString();
									lk.x1 = Convert.ToInt32(drv.Row["x1"]);
									lk.y1 = Convert.ToInt32(drv.Row["y1"]);
									lk.x2 = Convert.ToInt32(drv.Row["x2"]);
									lk.y2 = Convert.ToInt32(drv.Row["y2"]);

									page.AddLink(lk);

								}
							}
							ed.AddPage(page);
						}
					
					}


					return ed;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public static void CreateActivity(int EditionID, int EmailID, int BlastID, int Pageno, int LinkID, string ActionTypeCode, string ActionValue, string IP, string SessionID)
		{
			SqlCommand cmd = new SqlCommand("dbo.sp_InsertActivityLog");
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@EditionID", SqlDbType.Int));
			cmd.Parameters["@EditionID"].Value = EditionID;		
			cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
			cmd.Parameters["@EmailID"].Value = EmailID;	
			cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
			cmd.Parameters["@BlastID"].Value = BlastID;		
			cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.VarChar));
			cmd.Parameters["@PageNo"].Value = Pageno;	
			cmd.Parameters.Add(new SqlParameter("@LinkID", SqlDbType.Int));
			cmd.Parameters["@LinkID"].Value = LinkID;	
			cmd.Parameters.Add(new SqlParameter("@ActionTypeCode", SqlDbType.VarChar));
			cmd.Parameters["@ActionTypeCode"].Value = ActionTypeCode;	
			cmd.Parameters.Add(new SqlParameter("@ActionValue", SqlDbType.VarChar));
			cmd.Parameters["@ActionValue"].Value = ActionValue;	
			cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar));
			cmd.Parameters["@IP"].Value = IP;	
			cmd.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.VarChar));
			cmd.Parameters["@SessionID"].Value = SessionID;	
	
			DataFunctions.Execute(cmd);			
		}

	}
}
