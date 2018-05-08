using System;

namespace ecn.common.classes.billing {	
	public interface ICreditCardProcessor {
		bool IsCreditCardValid(CreditCard creditCard);
	}
}
