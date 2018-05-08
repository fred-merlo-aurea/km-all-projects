using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using ecn.common.classes;
using System.Collections.Generic;

namespace ecn.communicator.classes {
    public class QuotedPrintable {
        private QuotedPrintable() {
        }

        // Generates random strings of alphanums
        public static Random AppRandom = new Random((int)DateTime.Now.Ticks);
        public static string RandomString(int length, bool nums) {
            const string abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                      + "abcdefghijklmnopqrstuvwxyz";
            const string abc123 = abc + "0123456789";
            string charlist;
            if (nums) {
                charlist = abc123;
            } 
            else {
                charlist = abc;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i=0;i<length;i++) {
                int r = AppRandom.Next(0, charlist.Length);
                sb.Append(charlist.Substring(r, 1));
            }
            return sb.ToString();
        }
        // determine if we are using non ascii characters;
        public static bool NeedsEncoding(string body) {
            int cur;
            for(int x = 0; x < body.Length; ++x) {
                cur = Convert.ToInt32(body[x]);
                
              //  error_msg += "n(" + body[x] + ")=" + cur + " ";
                if(!( ((cur >= 33) && (cur <= 60)) || ((cur >= 62) && (cur <= 126)) || (cur == '\r') || (cur == '\n') || (cur == '\t') || (cur == ' '))) {
                   return true;
                }
            }
            //throw new Exception("cur_str = " +error_msg);
            return false;
        }

        // Default to split 76 as RFC states
		//const int MaximumCharsPerLine = 76;
		//Changed it to 70 chars 'cos the "=" gets encoded to "=3D" which increases the length of each line
		//-ashok 5/9/07
        //Changed it to 73 chars 'cos to set the line exactly to 76 Chars incl. =3D 
        //-ashok 6/21/10
        const int MaximumCharsPerLine = 73;

        public static string Encode(string body)
        {
            //return mimelib.QuotedPrintable.Encode(body);
            return Encode(body,true);
        }
        // Encode a quoted printable string
        public static string Encode(string body, bool split)
        {
            //if (split)
            //{
            //    return mimelib.QuotedPrintable.Encode(body);
            //}
            //else
            //{
            //    return mimelib.QuotedPrintable.EncodeSmall(body);
            //}

            StringBuilder buf = new StringBuilder();
            int cur;
            Encoding my_enc = Encoding.Default;


            for (int x = 0; x < body.Length; ++x)
            {
                cur = Convert.ToInt32(body[x]);
                //is this a valid ascii character?
                if (((cur >= 33) && (cur <= 60)) || ((cur >= 62) && (cur <= 126)) || (cur == '\r') || (cur == '\n') || (cur == '\t') || (cur == ' '))
                {
                    buf.Append(body[x]);
                }
                else
                {
                    Byte[] body_encoded = my_enc.GetBytes(body[x] + "");
                    Byte b = body_encoded[0];
                    buf.Append('=');
                    buf.Append(b.ToString("X2"));
                    //                    buf.Append(((int)((cur & 0xF0) >> 4)).ToString("X"));
                    //                  buf.Append(((int) (cur & 0x0F)).ToString("X"));
                }
            }


            if (split)
            {

                //format data so that lines don't end with spaces (if so, add a trailing '='), etc.
                //for more detail see RFC 2045.
                int start = 0;
                string enc = buf.ToString();
                buf.Length = 0;
                for (int x = 0; x < enc.Length; ++x)
                {
                    cur = Convert.ToInt32(enc[x]);
                    if (cur == '\n' || cur == '\r' || x == (enc.Length - 1))
                    {
                        buf.Append(enc.Substring(start, x - start + 1));
                        start = x + 1;
                        continue;
                    }
                    if ((x - start) > MaximumCharsPerLine)
                    {
                        bool inWord = true;
                        while (inWord)
                        {
                            inWord = IsInWord(enc, x);
                            if (inWord)
                            {
                                --x;
                                cur = Convert.ToInt32(enc[x]);
                            }
                            if (x == start)
                            {
                                x = start + MaximumCharsPerLine;
                                break;
                            }
                        }
                        buf.Append(enc.Substring(start, x - start + 1));
                        buf.Append("=\r\n");
                        start = x + 1;
                    }
                }
            }
            return buf.ToString();
        }



        public static string EncodeUTF(string body,ref List<string> byteSequenceList)
        {
            StringBuilder buf = new StringBuilder();
            int cur;
            Encoding my_enc = Encoding.UTF8;


            for (int x = 0; x < body.Length; ++x)
            {
                cur = Convert.ToInt32(body[x]);
                //is this a valid ascii character?
                if (((cur >= 33) && (cur <= 60)) || ((cur >= 62) && (cur <= 126)) || (cur == '\r') || (cur == '\n') || (cur == '\t') || (cur == ' '))
                {
                    buf.Append(body[x]);
                }
                else
                {
                    Byte[] body_encoded = my_enc.GetBytes(body[x] + "");
                    string checkBytes = "=" + BitConverter.ToString(body_encoded).Replace("-", "=");
                    if (!byteSequenceList.Contains(checkBytes))
                        byteSequenceList.Add(checkBytes);

                    buf.Append(checkBytes);
                    //                    buf.Append(((int)((cur & 0xF0) >> 4)).ToString("X"));
                    //                  buf.Append(((int) (cur & 0x0F)).ToString("X"));
                }
            }

           
            return buf.ToString();
        }

		private static bool IsInWord(string enc, int index) {	
			/// It used to be: !char.IsWhiteSpace(enc, x) && enc[x-2] != '='
			if (char.IsWhiteSpace(enc, index)) {
				return false;
			}

			if (index - 2 >= 0 && enc[index-2] == '=') {
				return false;
			}

			if (enc[index] == '/') {
				return false;
			}

			return true;
		}
    }
}



