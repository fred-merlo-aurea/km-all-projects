using System;

namespace ecn.common.classes {
	public class NoSuchElementException:InvalidOperationException {
		
		public NoSuchElementException() {
			Console.WriteLine("NoSuchElementException.");
		}
		
		public NoSuchElementException(String msg) {
			Console.WriteLine(msg);
		}
	}
}
