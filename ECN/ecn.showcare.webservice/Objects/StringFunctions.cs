using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ecn.showcare.webservice.Objects {
	public class StringFunctions {
		public static String Lower(String strParam) {
			return strParam.ToLower();
		}

		//Convert String to UpperCase
		public static String Upper(String strParam) {
			return strParam.ToUpper();
		}

		//Convert String to ProperCase
		public static String PCase(String strParam) {
			String strProper=strParam.Substring(0,1).ToUpper();
			strParam=strParam.Substring(1).ToLower();
			String strPrev="";

			for(int iIndex=0;iIndex<strParam.Length;iIndex++) {
				if(iIndex>1) {
					strPrev=strParam.Substring(iIndex-1,1);
				}
				if( strPrev.Equals(" ") ||
					strPrev.Equals("\t") ||
					strPrev.Equals("\n") ||
					strPrev.Equals(".")) {
					strProper+=strParam.Substring(iIndex,1).ToUpper();
				}
				else {
					strProper+=strParam.Substring(iIndex,1);
				}
			}
			return strProper;
		}

		// Function to Reverse the String
		public static String Reverse(String strParam) {
			if(strParam.Length==1) {
				return strParam;
			}
			else {
				return Reverse(strParam.Substring(1)) + strParam.Substring(0,1);
			}
		}

		// Function to Test for Palindrome
		public static bool IsPalindrome(String strParam) {
			int iLength,iHalfLen;
			iLength=strParam.Length-1;
			iHalfLen=iLength/2;
			for(int iIndex=0;iIndex<=iHalfLen;iIndex++) {
				if(strParam.Substring(iIndex,1)!=strParam.Substring(iLength-iIndex,1)) {
					return false;
				}
			}
			return true;
		}

		// Function to get string from beginning.
		public static String Left(String strParam,int iLen) {
			if(iLen>0)
				return strParam.Substring(0,iLen);
			else
				return strParam;      
		}

		// Function to get string from beginning.
		public static String TrimQuotes(String strParam) {
			if(Left(strParam,1).Equals("'") || Left(strParam,1).Equals("\"")){
				strParam = Right(strParam, strParam.Length - 1);
			}
			if(Right(strParam,1).Equals("'") || Right(strParam,1).Equals("\"")){
				strParam = Left(strParam, strParam.Length - 1);
			}
			return strParam;      
		}

		//Function to get string from end
		public static String Right(String strParam,int iLen) {
			if(iLen>0)
				return strParam.Substring(strParam.Length-iLen,iLen);
			else
				return strParam;
		}

		//Function to count no.of occurences of Substring in Main string
		public static int CharCount(String strSource,String strToCount) {
			int iCount=0;
			int iPos=strSource.IndexOf(strToCount);
			while(iPos!=-1) {
				iCount++;
				strSource=strSource.Substring(iPos+1);
				iPos=strSource.IndexOf(strToCount);
			}
			return iCount;
		}

		//Function to count no.of occurences of Substring in Main string
		public static int CharCount(String strSource,String strToCount,bool IgnoreCase) {
			if(IgnoreCase) {
				return CharCount(strSource.ToLower(),strToCount.ToLower());
			}
			else {
				return CharCount(strSource,strToCount);      
			}
		}

		//Function Trim the string to contain Single between words
		public static String ToSingleSpace(String strParam) {
			int iPosition=strParam.IndexOf("  ");
			if(iPosition==-1) {
				return strParam;
			}
			else {
				return ToSingleSpace(strParam.Substring(0,iPosition) + strParam.Substring(iPosition+1));
			}
		}

		//Function Replace string function.
		public static String Replace(String strText,String strFind,String strReplace) {
			int iPos=strText.IndexOf(strFind);
			String strReturn="";
			while(iPos!=-1) {
				strReturn+=strText.Substring(0,iPos) + strReplace;
				strText=strText.Substring(iPos+strFind.Length);
				iPos=strText.IndexOf(strFind);
			}
			if(strText.Length>0)
				strReturn+=strText;
			return strReturn;
		}

		//Convert object returned from SQL to string
		public static string DBString(object obj, string tacit) {
			if ((tacit == null) || (tacit == String.Empty) || (tacit.Length <= 0))
				tacit = " ";
			if ((obj.Equals(System.DBNull.Value)) || (obj == null))
				return tacit;
			try {
				if (Convert.ToString(obj)=="")
					return tacit;
				else
					return (Convert.ToString(obj));
			} 
			catch (Exception) {
				return tacit;
			}
		}

		public static String Remove(string strText, string removeSet) {
			string tmp, strOutput = "";

			for (int j=0; j<strText.Length; j++){
				tmp = strText.Substring(j,1);
				for (int i=0; i<removeSet.Length; i++){
					tmp = Replace( tmp, removeSet.Substring(i,1), "");
					if (tmp.Length==0) {
						break;
					}
				}
				strOutput+=tmp;
			}
			return strOutput;
		}

		public static String NonAlphas() {
			string thebadones="";
			for (int i=0; i<48; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			for (int i=58; i<65; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			for (int i=91; i<97; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			for (int i=123; i<128; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			return thebadones;
		}

		public static String NonDomain() {
			string thebadones="";
			for (int i=0; i<43; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			thebadones+=(char)44;
			thebadones+=(char)47;
			for (int i=58; i<64; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			for (int i=91; i<95; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			thebadones+=(char)96;
			for (int i=123; i<128; i++) {
				char cs=(char)i;
				thebadones+=cs;
			}
			return thebadones;
		}

		public static String RemoveHTML(string strText){
			strText = Replace(strText, "<br>", System.Environment.NewLine);
			strText = Replace(strText, "<BR>", System.Environment.NewLine);
			strText = Replace(strText, "<p>", System.Environment.NewLine);
			strText = Replace(strText, "<P>", System.Environment.NewLine);
			strText = Replace(strText, "&nbsp;", " ");
			strText = Replace(strText, "&copy;", "");
			strText = Replace(strText, "&reg;", "");
			strText = Replace(strText, "&trade;", "");
			Regex cr = new Regex("<[^>]*>");
			return cr.Replace(strText, "");
		}
		
		public static string GetNumberInWord(int number) {
			return new NumberConverter().GetNumberInWord(number);
		}
	}
	
	public class NumberConverter {
		public string GetNumberInWord(int n) {
			string number = n.ToString();
			string s = "";

			bool andflag = false;
			if(number.Length > 6) {
				s += DoTriplet((n%1000000000) / 1000000, andflag) + " million ";
				andflag = true;
			}
			if(number.Length > 3) {
				s += DoTriplet((n%1000000) / 1000, andflag) + " thousand ";
				andflag = true;
			}
			else
				andflag = false;
			s += DoTriplet(n % 1000, andflag);

			return s;
		}		

		string DoTriplet(int n, bool andflag) {
			int o = n%10;
			int t = (n%100/10);
			int h = n/100;

			string s = "";
			if(h != 0) {
				s += GetOnes(h) + " hundred ";
			}
			if(t != 0 || o != 0) {
				if(h != 0 || andflag)
					s += "and ";
			}
			if(t == 1 && o != 0) {
				s += GetFirst(t*10 + o);
			}
			else {
				s += GetTens(t) + " " + GetOnes(o);
			}
			return s;
		}		

		string GetOnes(int n) {
			switch(n) {
				case 1:
					return "one";
				case 2:
					return "two";
				case 3:
					return "three";
				case 4:
					return "four";
				case 5:
					return "five";
				case 6:
					return "six";
				case 7:
					return "seven";
				case 8:
					return "eight";
				case 9:
					return "nine";
				default:
					return "";
			}
		}

		string GetTens(int n) {
			switch(n) {
				case 1:
					return "ten";
				case 2:
					return "twenty";
				case 3:
					return "thirty";
				case 4:
					return "fourty";
				case 5:
					return "fifty";
				case 6:
					return "sixty";
				case 7:
					return "seventy";
				case 8:
					return "eighty";
				case 9:
					return "ninety";
				default:
					return "";
			}
		}

		string GetFirst(int n) {
			switch(n) {
				case 11:
					return "eleven";
				case 12:
					return "twelve";
				case 13:
					return "thirteen";
				case 14:
					return "fourteen";
				case 15:
					return "fifteen";
				case 16:
					return "sixteen";
				case 17:
					return "seventeen";
				case 18:
					return "eighteen";
				case 19:
					return "nineteen";
				default:
					return "";
			}
		}
	}
}
