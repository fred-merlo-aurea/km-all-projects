using System;
using System.Collections;

namespace ecn.communicator.blastsmanager
 {
    public class HobbyTownPrice 
    {
		public int LowerRange;
		public int UppderRange;
		public double Rate;

		public HobbyTownPrice(int lowerRange, int upperRange, double rate) 
        {
			LowerRange = lowerRange;
			UppderRange = upperRange;
			Rate = rate;

		}

		public static ArrayList PriceList
        {
			get
            {
				ArrayList list = new ArrayList();
				list.Add(new HobbyTownPrice(1, 1000, 0.03));
				list.Add(new HobbyTownPrice(1001,2500, 0.025));
				list.Add(new HobbyTownPrice(2501,15000, 0.0225));
				list.Add(new HobbyTownPrice(15001,25000, 0.02));
				list.Add(new HobbyTownPrice(25001,50000, 0.0185));
				list.Add(new HobbyTownPrice(50000,100000, 0.0175));
				return list;
			}
		}
	}

}