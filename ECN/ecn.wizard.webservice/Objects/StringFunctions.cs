using System;

namespace ecn.wizard.webservice.Objects
{
	public class StringFunctions
	{
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
	}
}
