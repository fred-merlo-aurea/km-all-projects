using System;
using System.Collections;

namespace ecn.publisher.classes
{

	public enum LinkTypeEnum {GoTo, URI}

	public class Link
	{
		private int _id = 0;
		private int _pageID;
		private string _linkType = string.Empty;
		private string _linkURL = string.Empty;
		private int _x1 = 0;
		private int _y1 = 0;
		private int _x2 = 0;
		private int _y2 = 0;

		public Link() : this(string.Empty, string.Empty)
		{ }

        public Link(string linkType, string linkUrl) : this(linkType, linkUrl, 0, 0, 0, 0)
        { }

		public Link(string linkType, string linkUrl, int x1, int y1, int x2, int y2) 
		{
			_linkType = linkType;
			_linkURL = linkUrl;
			_x1 = x1;
			_y1 = y1;
			_x2 = x2;
			_y2 = y2;
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


		public int PageID 
		{
			get 
			{
				return (_pageID);
			}
			set 
			{
				_pageID = value;
			}
		}


		public string LinkURL 
		{
			get 
			{
				return (_linkURL);
			}
			set 
			{
				_linkURL = value;
			}
		}


		public string LinkType 
		{
			get 
			{
				return (_linkType);
			}
			set 
			{
				_linkType = value;
			}
		}


		public int x1
		{
			get 
			{
				return (_x1);
			}
			set 
			{
				_x1 = value;
			}
		}		


		public int y1
		{
			get 
			{
				return (_y1);
			}
			set 
			{
				_y1 = value;
			}
		}	


		public int x2
		{
			get 
			{
				return (_x2);
			}
			set 
			{
				_x2 = value;
			}
		}


		public int y2
		{
			get 
			{
				return (_y2);
			}
			set 
			{
				_y2 = value;
			}
		}

	}
}
