using System;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using ecn.common.classes;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace ecn.communicator.classes
{
    public abstract class EmailMessageProvider
    {
        private List<string> _byteSequences = new List<string>();
        private readonly Encoding UTFEncoding = Encoding.UTF8;
        private const string UTF8 = " =?utf-8?Q?";
        private const string UTF8WithQuestion = " =?utf-8?Q??=";
        private const string UTF8NewLine = "?=\r\n";
        private const string UTF8Equal = "?=";
        private const string UTF83D = "=3D";
        private const int Length21 = 21;
        private const int Length12 = 12;
        private const int Length67 = 67;
        private const int Length56 = 56;

        string _toEmailAddress, _replyTo;
        string _fromName, _fromEmailAddress;
        string _bounceAddress;

        public static EmailMessageProvider CreateInstance(string formatTypeCode, string toEmailAddress, string fromName, string fromEmailAddress, string replyTo, string bounceAddress)
        {
            switch (formatTypeCode.ToLower())
            {
                case "html":
                    return new HtmlEmailMessageProvider(toEmailAddress, fromName, fromEmailAddress, replyTo, bounceAddress);
                default:
                    return new PlainTextEmailMessageProvider(toEmailAddress, fromName, fromEmailAddress, replyTo, bounceAddress);
            }
        }

        public EmailMessageProvider(string toEmailAddress, string fromName, string fromEmailAddress, string replyTo, string bounceAddress)
        {
            _toEmailAddress = toEmailAddress;
            _fromName = fromName;
            _fromEmailAddress = fromEmailAddress;
            _replyTo = replyTo;
            _bounceAddress = bounceAddress;
        }

        public string GetSubjectUTF(string dynamicSubject)
        {
            var dynamicTemp = String.Format(@"{0}", dynamicSubject);
            var nonSurrogate = new Regex("([\\\\][u][02-3e][0-7a-bA-B][0-9a-fA-F][0-9a-fA-F])", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var unicodeSplit = new Regex("([\\\\][u][d0][08-9a-bA-B][0-9a-fA-F][0-9a-fA-F][\\\\][u][d0-9][0-9c-fC-F][a-fA-F0-9][a-fA-F0-9])", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = unicodeSplit.Matches(dynamicTemp);
            var nonSMatches = nonSurrogate.Matches(dynamicTemp);
            
            if ((matches == null || matches.Count == 0) && (nonSMatches == null || nonSMatches.Count == 0) && !QuotedPrintable.NeedsEncoding(dynamicSubject))
            {
                return " " + dynamicSubject;
            }
            else
            {
                if (QuotedPrintable.NeedsEncoding(dynamicSubject))
                {
                    dynamicSubject = QuotedPrintable.EncodeUTF(dynamicSubject, ref _byteSequences);
                }

                dynamicSubject = ReplaceMatches(dynamicSubject, matches);
                dynamicSubject = ReplaceMatches(dynamicSubject, nonSMatches);

                var subject = new StringBuilder();
                if (dynamicSubject.Contains(UTF83D) && !_byteSequences.Contains(UTF83D))
                {
                    _byteSequences.Add(UTF83D);
                }

                dynamicSubject = dynamicSubject.Replace(" ", "_").Replace("?", "=3F");
                var regexSplitByte = new Regex("(=[A-F0-9]{2})");
                string[] splitOnEncoding = regexSplitByte.Split(dynamicSubject).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                subject = GetSubject(splitOnEncoding, subject);

                if (!subject.ToString().EndsWith(UTF8Equal)) //if doesn't have closing tag, add it
                {
                    subject.Append(UTF8Equal);
                }

                if (subject.ToString().EndsWith(UTF8WithQuestion))
                {
                    subject = new StringBuilder(subject.ToString().Replace(UTF8WithQuestion, string.Empty));
                }

                return subject.ToString();
            }
        }

        private StringBuilder GetSubject(string[] splitOnEncoding, StringBuilder subject)
        {
            var startNewRow = true;
            var currentLineLength = 0;

            for (var index = 0; index < splitOnEncoding.Length; index++)
            {
                if (startNewRow)
                {
                    subject.Append(UTF8);
                    if (index == 0)
                    {
                        currentLineLength = Length21; //this is to account for "Subject: " header text
                    }
                    else
                    {
                        currentLineLength = Length12;
                    }
                    startNewRow = false;
                }

                var currentString = splitOnEncoding[index];

                //check if its a byte sequence
                var isByte = isByteString(currentString);

                if (isByte)//do a look ahead to see when the byte array stops
                {
                    var byteWhile = true;
                    var byteIndex = 1;
                    var currentByteSequence = currentString;
                    while (byteWhile)
                    {
                        if (index + byteIndex < splitOnEncoding.Length)
                        {
                            var nextByte = splitOnEncoding[index + byteIndex];
                            if (isByteString(nextByte) && !nextByte.Equals("=20") && !nextByte.Equals("=3D") && !nextByte.Equals("=3F"))//adding check for space and equals sign and question mark
                            {
                                currentByteSequence += nextByte;
                                if (_byteSequences.Contains(currentByteSequence))
                                {
                                    //we've found an existing byte sequence so stop looking ahead
                                    byteWhile = false;
                                }
                                //keep looking ahead to find more bytes
                                byteIndex++;
                            }
                            else
                            {
                                byteWhile = false;
                            }
                        }
                        else //last byte in sequence- shouldn't have to do anything
                        {
                            byteWhile = false;
                            byteIndex++; //increment incase its a single byte sequence so it will be set to current string
                        }
                    }

                    if (byteIndex > 1)
                    {
                        //we have a byte sequence
                        //currentLineLength += currentByteSequence.Length;
                        currentString = currentByteSequence;
                        index = index + byteIndex - 1;
                    }
                }

                if (currentLineLength + currentString.Length > Length67 && !isByte)//can't just append especially if it's a byte sequence
                {
                    var keepSplitting = true;
                    var splitIndexStart = 0;
                    var numberofCharsToGrab = Length67 - currentLineLength;
                    while (keepSplitting)
                    {
                        var split1 = currentString.Substring(splitIndexStart, numberofCharsToGrab);
                        if (split1.Length + currentLineLength == Length67)
                        {
                            subject.Append(split1 + UTF8NewLine);
                            subject.Append(UTF8);
                            currentLineLength = 12;
                        }
                        else
                        {
                            subject.Append(split1);
                            currentLineLength = Length12 + split1.Length;
                        }
                        if (currentString.Length - (splitIndexStart + numberofCharsToGrab) > 0)
                        {
                            //keep splitting
                            splitIndexStart += numberofCharsToGrab;
                            numberofCharsToGrab = currentString.Length - splitIndexStart > Length56 ? Length56 : currentString.Length - splitIndexStart;
                        }
                        else
                        {
                            keepSplitting = false;

                        }
                        startNewRow = false;
                    }
                }
                else if (currentLineLength + currentString.Length > Length67 && isByte)//special for byte sequence
                {
                    subject.Append(UTF8NewLine);
                    subject.Append(UTF8);
                    currentLineLength = currentString.Length + Length12;//reseting line length caues we're starting a new one
                    subject.Append(currentString);
                    startNewRow = false;
                }
                else//this means we can append 
                {
                    currentLineLength += currentString.Length;
                    subject.Append(currentString);
                }
            }

            return subject;
        }

        private string ReplaceMatches(string dynamicSubject, MatchCollection matches)
        {
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var uniValue = match.Value;
                    uniValue = uniValue.Replace("\\u", "");
                    var chars = new char[2];
                    var charIndex = 0;

                    for (var index = 0; index < uniValue.Length; index += 4)
                    {
                        chars[charIndex] = (char)Int16.Parse(uniValue.Substring(index, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        charIndex++;
                    }

                    var byteSequence = "=" + BitConverter.ToString(UTFEncoding.GetBytes(chars)).Replace("-", "=");
                    if (!_byteSequences.Contains(byteSequence))
                    {
                        _byteSequences.Add(byteSequence);
                    }

                    var checkBytes = byteSequence;
                    dynamicSubject = dynamicSubject.Replace(match.Value, checkBytes.ToString());
                }
            }

            return dynamicSubject;
        }

        private bool isByteString(string toCheck)
        {

            if (toCheck.Length == 3)
            {
                char index0 = toCheck[0];
                char index1 = toCheck[1];
                char index2 = toCheck[2];
                if (isHexChar(index1) && isHexChar(index2) && (int)index0 == 61)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool isHexChar(char character)
        {
            Regex hexChars = new Regex("[0-9A-F]");
            bool isMatch = hexChars.IsMatch(character.ToString());
            return isMatch;
        }

        private string GetMIMEEncoded(string toEncode)
        {
            string final = "";

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string base64 = Convert.ToBase64String(bytes);

            final = " =?utf-8?B?" + base64 + "=?= ";

            return final;
        }

        public string GetSubject(string dynamicSubject)
        {
            if (!QuotedPrintable.NeedsEncoding(dynamicSubject))
            {
                return " " + dynamicSubject;
            }

            StringBuilder subject = new StringBuilder();
            //   throw new Exception("dynamicSubject is |" + dynamicSubject + "|");
            dynamicSubject = QuotedPrintable.Encode(dynamicSubject, false);
            // split it into 50 character parts for RFC compliance
            for (int l = 0; l < dynamicSubject.Length; )
            {
                int how_far = 50;
                if (l + 50 > dynamicSubject.Length)
                {
                    how_far = dynamicSubject.Length - l;
                }

                // Don't split a Encoding
                if ('=' == dynamicSubject[l + how_far - 2])
                {
                    how_far += 1;
                }
                else if ('=' == dynamicSubject[l + how_far - 1])
                {
                    how_far += 2;
                }
                string little_str = dynamicSubject.Substring(l, how_far);
                l += how_far;

                // following line was messing up the emails that's sent out with a special character in the subject line 'cos of the Environment.NewLine
                // Removed this & made it just an empty string with "" see line 76
                // - Ashok 08/31/05
                //subject.Append(string.Format(" =?iso-8859-1?Q?{0}?={1}",little_str,Environment.NewLine));
                //subject.Append(string.Format(" =?iso-8859-1?Q?{0}?=",little_str));
                subject.Append(string.Format(" =?iso-8859-1?Q?{0}?={1}", little_str, ""));
            }
            return subject.ToString();
        }

        public string WriteEmailMessage(DataRow dr, string dynamicSubject, string messageID, string textBody, string boundryTag, string htmlBody, BlastConfig blastconfig)
        {
            StringBuilder sw = new StringBuilder(100000);

            //Added the Encoding for From Name 'cos customers are using special chars in the from name.
            //-ashok -4/20/06
            _fromName = GetSubject(_fromName);


            if (blastconfig.MTAName.ToUpper() == "PORT25")
            {
                if (dr["MailRoute"].ToString().Trim().Length > 0)
                {
                    sw.Append("x-MTA25-MtaID: " + dr["MailRoute"].ToString() + "\r\n");
                    sw.Append("x-MTA25-SendID: " + dr["BlastID"].ToString() + "\r\n");
                    sw.Append("x-MTA25-ListID: " + dr["GroupID"].ToString() + "\r\n");
                    sw.Append("x-virtual-mta: " + dr["MailRoute"].ToString() + "\r\n");//vmtapool1
                    sw.Append("x-job: " + dr["BlastID"].ToString() + "\r\n");
                }
                else
                {
                    Exception ex = new Exception("BlastID: " + dr["BlastID"].ToString() + " - Port25 MTA does not have a valid MailRoute");
                    throw ex;
                }
            }
            else
            {
                Exception ex = new Exception("BlastID: " + dr["BlastID"].ToString() + " -Port25 MTA is not configured");
                throw ex;
            }


            //need X-Mailer-Address for Green Arrow
            sw.Append("X-Mailer-Address: " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_toEmailAddress)) + "\r\n");

            //add the priority header for singles or test blasts
            //wgh - commented out to test speed
            //bool isPriority = false;
            //bool.TryParse(DataFunctions.ExecuteScalar("communicator", "SELECT CASE WHEN COUNT(BlastID) > 0 THEN 'TRUE' ELSE 'FALSE' END FROM Blasts WHERE BlastID = " + dr["BlastID"].ToString() + " AND (UPPER(TestBlast) = 'Y' OR EXISTS(SELECT * FROM BlastSingles where BlastID = " + dr["BlastID"].ToString() + "))").ToString(), out isPriority);
            //if (isPriority)
            //{
            //    sw.Append("x-priority: 100\r\n");
            //} 
            if (dr["IsMTAPriority"].ToString() == "1")
            {
                sw.Append("x-priority: 100\r\n");
            }

            // make the RFC 822 Style email
            sw.Append("X-Receiver: " + _toEmailAddress + "\r\n");
            sw.Append("From: \"" + _fromName.Trim() + "\" <" + _fromEmailAddress + ">" + "\r\n");
            sw.Append("To: \"" + _toEmailAddress + "\" <" + _toEmailAddress + ">" + "\r\n");
            sw.Append("List-Unsubscribe: <" + System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] +
                "/engines/Unsubscribe.aspx?e=" + _toEmailAddress + "&g=" + dr["GroupID"].ToString() + "&b=" +
                dr["BlastID"].ToString() + "&c=" + dr["CustomerID"].ToString() + "&s=U&f=html>" + "\r\n");
            sw.Append("Reply-To: " + _replyTo + "\r\n");
            //sw.WriteLine("Date: " + DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'CST'"));
            int hours_to_add = 0 - Convert.ToInt32(DateTime.Now.ToString(" z") + "\r\n");
            sw.Append("Date: " + DateTime.Now.AddHours(hours_to_add).ToString("r") + "\r\n");

            //We don't need Organization Header Tag any more - ashok - 09/04/07
            //sw.Append("Organization: Microsoft" + "\r\n");
            // RFC 1522 Email Subject
            sw.Append("Subject:" + GetSubjectUTF(dynamicSubject) + "\r\n");
            sw.Append("X-Mailer: ECN Communicator 5.1" + "\r\n");
            sw.Append("X-RCPT-TO: <" + _toEmailAddress + ">" + "\r\n");
            sw.Append("Message-ID: <" + messageID + ">" + "\r\n");

            //send on Behalf - commented by Sunil on 07/14/2010
            //sw.Append("Sender: <" + _fromEmailAddress.Replace("@", "=") + "@gdbounce.com>" + "\r\n");

            sw.Append("MIME-Version: 1.0" + "\r\n");
            WriteContentType(ref sw, boundryTag);

            WriteContent(dr, ref sw, boundryTag, textBody, htmlBody);
            return sw.ToString();
        }

        #region abstract Methods
        protected abstract void WriteContentType(ref StringBuilder sw, string boundryTag);

        protected abstract void WriteContent(DataRow dr, ref StringBuilder sw, string boundryTag, string textBody, string htmlBody);
        #endregion
    }
}