using System;

namespace ecn.wizard.webservice.Objects {
	public class NoSuchElementException:InvalidOperationException {
		
		public NoSuchElementException() {
			Console.WriteLine("NoSuchElementException.");
		}
		
		public NoSuchElementException(String msg) {
			Console.WriteLine(msg);
		}
	}
}
