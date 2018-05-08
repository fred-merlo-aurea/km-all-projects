using System;
using System.Collections;
using ecn.common.classes;

namespace ecn.publisher.classes
{

	public class Pages : CollectionBase 
	{
		public void Add(Page page) 
		{
			this.InnerList.Add(page);
		}

		public Page this[int index] 
		{
			get { return (Page) this.InnerList[index];}
			set { this.InnerList[index] = value;}
		}
	}
}
