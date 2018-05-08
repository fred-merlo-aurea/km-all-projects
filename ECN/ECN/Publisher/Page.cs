using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace ecn.publisher.classes
{

	public class Page
	{
		private int _id = 0;
		private int _editionID;
		private Links _links;
		private int _pageno;
		private string _displayno;
		private int _width;
		private int _height;
		private string _textcontent = string.Empty;

		public Page() 
		{
			_links = new Links();
		}


		public int ID 
		{
			get 
			{
				return (_id);
			}
			set 
			{
				_id = value;
			}
		}


		public int EditionID
		{
			get 
			{
				return (_editionID);
			}
			set 
			{
				_editionID = value;
			}
		}		


		public string ImagePath 
		{
			get { return string.Format("{0}","");}
		}


		public int PageNo
		{
			get 
			{
				return (_pageno);
			}
			set 
			{
				_pageno = value;
			}
		}		


		public string DisplayNo
		{
			get 
			{
				return (_displayno);
			}
			set 
			{
				_displayno = value;
			}
		}


		public int Width
		{
			get 
			{
				return (_width);
			}
			set 
			{
				_width = value;
			}
		}		


		public int Height
		{
			get 
			{
				return (_height);
			}
			set 
			{
				_height = value;
			}
		}


		public string TextContent
		{
			get 
			{
				return (_textcontent);
			}
			set 
			{
				_textcontent = value;
			}
		}


		public Links LinkCollection
		{
			get 
			{
				return (this._links);
			}
		}


		public void AddLink(Link item) 
		{
			_links.Add(item);
		}

		public int Save()
		{
			StringBuilder sbLinks = new StringBuilder("");

			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["pub"]);

			SqlCommand cmd = new SqlCommand("dbo.sp_SavePage", conn);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@PageID", SqlDbType.Int));
			cmd.Parameters["@PageID"].Value = _id;		
			
			cmd.Parameters.Add(new SqlParameter("@EditionID", SqlDbType.Int));
			cmd.Parameters["@EditionID"].Value = _editionID;	

			cmd.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int));
			cmd.Parameters["@PageNumber"].Value = _pageno;	

			cmd.Parameters.Add(new SqlParameter("@DisplayNumber", SqlDbType.VarChar));
			cmd.Parameters["@DisplayNumber"].Value = _displayno;	

			cmd.Parameters.Add(new SqlParameter("@width", SqlDbType.Int));
			cmd.Parameters["@width"].Value = _width;	

			cmd.Parameters.Add(new SqlParameter("@height", SqlDbType.Int));
			cmd.Parameters["@height"].Value = _height;
	
			cmd.Parameters.Add(new SqlParameter("@TextContent", SqlDbType.Text));
			cmd.Parameters["@TextContent"].Value = _textcontent;				

			foreach (Link lk in this.LinkCollection)
			{
				sbLinks.Append("<Link type='" + lk.LinkType + "' x1='" + lk.x1 + "' y1='" + lk.y1 + "' x2='" + lk.x2 + "' y2='" + lk.y2 + "'><![CDATA[" + lk.LinkURL + "]]></Link>");
			}

			cmd.Parameters.Add(new SqlParameter("@xmlLinks", SqlDbType.Text));
			cmd.Parameters["@xmlLinks"].Value = "<Links>" + sbLinks.ToString() + "</Links>";	

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
	
	}
	
}
