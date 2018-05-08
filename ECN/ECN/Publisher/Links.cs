using System;
using System.Collections;

namespace ecn.publisher.classes
{

	public class Links : CollectionBase 
	{
		public void Add(Link link) 
		{
			this.InnerList.Add(link);
		}

		public Link this[int index] 
		{
			get { return (Link) this.InnerList[index];}
			set { this.InnerList[index] = value;}
		}
	}
	
}
