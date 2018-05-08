//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using ECN_Framework.Communicator.Entity;
//using ECN_Framework.Communicator.Object;
////using ECN_Framework.Common;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework.Communicator.Abstract
//{
//    public abstract class BlastAbstract : BlastNew
//    {

//        public BlastAbstract()//was protected
//        {
//            BlastID = -1;
//            CustomerID = null;
//            EmailSubject = string.Empty;
//            EmailFrom = string.Empty;
//            EmailFromName = string.Empty;
//            SendTime = null;
//            AttemptTotal = null;
//            SendTotal = null;
//            SendBytes = null;
//            StatusCode = string.Empty;
//            BlastType = string.Empty;
//            CodeID = null;
//            LayoutID = null;
//            GroupID = null;
//            FinishTime = null;
//            SuccessTotal = null;
//            BlastLog = string.Empty;
//            UserID = null;
//            FilterID = null;
//            Spinlock = "n";
//            ReplyTo = string.Empty;
//            TestBlast = string.Empty;
//            BlastFrequency = string.Empty;
//            RefBlastID = string.Empty;
//            BlastSuppression = string.Empty;
//            AddOptOuts_to_MS = null;
//            DynamicFromName = string.Empty;
//            DynamicFromEmail = string.Empty;
//            DynamicReplyToEmail = string.Empty;
//            BlastEngineID = null;
//            HasEmailPreview = false;
//            BlastScheduleID = null;
//            OverrideAmount = null;
//            OverrideIsAmount = null;
//            StartTime = null;
//            SMSOptInTotal = null;
//            CampaignItemID = null;
//            NodeID = string.Empty;
//            SmartSegmentID = null;
//            SampleID = null;
//            //Codes(ID)
//            StatusCodeID = null;
//            BlastTypeID = null;
//            //validation
//            ErrorList = new List<ValidationError>();
//            //optional
//            Group = null;
//            Layout = null;
//            BlastUser = null;
//            Filter = null;
//            Segment = null;
//            Schedule = null;
//            Fields = null;
//        }

//        //public Email GetEmails(Blast b)
//        //{
//        //    Email e = new Email();
//        //    return e;
//        //}

//        public abstract string Send();        

//        public abstract bool Save();

//        public abstract bool Save(SqlCommand command);

//        public abstract bool Validate();

//        //public abstract bool ValidateBlastContent();

//        public bool PreValidate()
//        {
//            ValidationError error = null;

//            if (BlastID == 0)
//            {
//                error = new ValidationError("BlastID", "Blast id is invalid");
//                ErrorList.Add(error);
//            }
//            if (CustomerID == null || (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(CustomerID.Value)))
//            {
//                error = new ValidationError("CustomerID", "Blast customer is invalid");
//                ErrorList.Add(error);
//            }
//            //email subject validated in implementation of abstract object
//            //email from validated in implementation of abstract object
//            //email from name validated in implementation of abstract object
//            if (SendTime == null || SendTime.Value.AddMinutes(5) <= DateTime.Now)
//            {
//                error = new ValidationError("SendTime", "Blast send time must be in the future");
//                ErrorList.Add(error);
//            }
//            //status code validated in implementation of abstract object
//            //blast type validated in implementation of abstract object
//            if (CodeID != null && (CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Code.Exists(CodeID.Value, CustomerID.Value))))
//            {
//                error = new ValidationError("CodeID", "Blast code/category is invalid");
//                ErrorList.Add(error);
//            }
//            //layout validated in implementation of abstract object
//            if (GroupID == null || CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.GroupExists(GroupID.Value, CustomerID.Value)))
//            {
//                error = new ValidationError("GroupID", "Blast group is invalid");
//                ErrorList.Add(error);
//            }
//            if (UserID == null || CustomerID == null || (!KMPlatform.BusinessLogic.User.Exists(UserID.Value, CustomerID.Value)))
//            {
//                error = new ValidationError("UserID", "Blast user id is invalid");
//                ErrorList.Add(error);
//            }
//            if ((FilterID != null && FilterID > 0) && (CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Filter.Exists(FilterID.Value, CustomerID.Value))))
//            {
//                error = new ValidationError("CustomerID", "Blast code/category is invalid");
//                ErrorList.Add(error);
//            }
//            //reply to validated in implementation of abstract object
//            //test blast validated in implementation of abstract object
//            //RefBlastID validated in implementation of abstract object
//            //BlastSuppression validated in implementation of abstract object
//            //AddOptOuts_to_MS validated in implementation of abstract object
//            //DynamicFromName validated in implementation of abstract object
//            //DynamicFromEmail validated in implementation of abstract object
//            //DynamicReplyToEmail validated in implementation of abstract object
//            if (BlastScheduleID != null && (!ECN_Framework_BusinessLayer.Communicator.BlastSchedule.Exists(BlastScheduleID.Value)))
//            {
//                error = new ValidationError("BlastScheduleID", "Blast schedule is invalid");
//                ErrorList.Add(error);
//            }
//            if (OverrideAmount != null)
//            {
//                if (OverrideIsAmount == null)
//                {
//                    error = new ValidationError("OverrideIsAmount", "Blast amount type is invalid");
//                    ErrorList.Add(error);
//                }
//            }
//            //SmartSegmentID validated in implementation of abstract object
//            //wgh - to do after campaignitem is created
//            //if (CampaignItemID  == null || CustomerID == null || (!ECN_Framework.Communicator.Entity.CampaignItem.Exists(CampaignItemID.Value, CustomerID.Value)))
//            //{
//            //    error = new ValidationError("CampaignID", "Blast campaign is invalid");
//            //    ErrorList.Add(error);
//            //}
//            //SampleID validated in implementation of abstract object
//            if (ErrorList.Count > 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }

//        public bool ValidateBlastContent()
//        {
//            bool bSuccess = true;
//            if (ErrorList == null)
//            {
//                ErrorList = new List<ValidationError>();
//            }
//            ValidationError error = null;

//            try
//            {
//                System.Collections.Generic.List<string> listCS = new System.Collections.Generic.List<string>();
//                StringBuilder sbCS = new StringBuilder();
//                sbCS.Append(" select lower(c.name) as shortname from sysobjects o join syscolumns c on o.id = c.id where o.name = 'Emails' ");
//                sbCS.Append(" union ");
//                sbCS.Append(" select shortname as shortname from groupdatafields where groupID = " + GroupID.Value);

//                DataTable dtCS = new DataTable();
//                dtCS = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sbCS.ToString(), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//                foreach (DataRow dr in dtCS.Rows)
//                {
//                    listCS.Add(dr["shortname"].ToString().ToLower());
//                }


//                System.Collections.Generic.List<string> listLY = new System.Collections.Generic.List<string>();
//                StringBuilder sbLY = new StringBuilder();
//                sbLY.Append(" select ContentSource as Content from Layouts ");
//                sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9  ");
//                sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ") union all ");
//                sbLY.Append(" select ContentText as Content from Layouts ");
//                sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9 ");
//                sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ") union all ");
//                sbLY.Append(" select ContentMobile as Content from Layouts ");
//                sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//                sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9 ");
//                sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ")");

//                DataTable dtLY = new DataTable();
//                dtLY = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sbLY.ToString(), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());

//                foreach (DataRow dr in dtLY.Rows)
//                {
//                    listLY.Add(dr["Content"].ToString().ToLower());
//                }
//                listLY.Add(EmailSubject.Trim().ToLower());

//                System.Collections.Generic.List<string> subLY = new System.Collections.Generic.List<string>();
//                foreach (string s in listLY)
//                {
//                    #region Badly Formed Snippets
//                    //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
//                    System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//                    System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
//                    if (MatchList.Count > 0)
//                    {
//                        if ((MatchList.Count % 2) != 0)
//                        {
//                            //return error
//                            error = new ValidationError("ContentValidation", "Incorrectly formed code snippet");
//                            ErrorList.Add(error);
//                            return false;
//                        }
//                        else
//                        {
//                            System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//                            System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
//                            if ((MatchList.Count / 2) > MatchListGood.Count)
//                            {
//                                //return error
//                                error = new ValidationError("ContentValidation", "Incorrectly formed code snippet");
//                                ErrorList.Add(error);
//                                return false;
//                            }
//                        }
//                    }
//                    #endregion

//                    //%% and ##
//                    System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//                    System.Text.RegularExpressions.MatchCollection MatchList1 = reg1.Matches(s);

//                    foreach (System.Text.RegularExpressions.Match m in MatchList1)
//                    {
//                        if (!string.IsNullOrEmpty(m.Value.ToString()))
//                        {
//                            if (!subLY.Contains(m.Value.ToString().Replace("%%", string.Empty)))
//                                subLY.Add(m.Value.ToString().Replace("%%", string.Empty));
//                        }
//                    }
//                }
//                System.Collections.Generic.List<string> listNoExist = new System.Collections.Generic.List<string>();
//                foreach (string s in subLY)
//                {
//                    if (!listCS.Contains(s))
//                        listNoExist.Add(s);
//                }

//                listNoExist.Remove("blastid");
//                listNoExist.Remove("groupid");
//                listNoExist.Remove("groupname");
//                listNoExist.Remove("emailtofriend");
//                listNoExist.Remove("conversiontrkcde");
//                listNoExist.Remove("unsubscribelink");
//                listNoExist.Remove("lastchanged");
//                listNoExist.Remove("createdon");
//                listNoExist.Remove("publicview");
//                listNoExist.Remove("company_address");
//                listNoExist.Remove("surveytitle");
//                listNoExist.Remove("surveylink");
//                listNoExist.Remove("currdate");

//                listNoExist.Remove("customer_name");
//                listNoExist.Remove("customer_address");
//                listNoExist.Remove("customer_webaddress");

//                listNoExist.Remove("customer_udf1");
//                listNoExist.Remove("customer_udf2");
//                listNoExist.Remove("customer_udf3");
//                listNoExist.Remove("customer_udf4");
//                listNoExist.Remove("customer_udf5");

//                if (listNoExist.Count > 0)
//                {
//                    StringBuilder errormsg = new StringBuilder();
//                    errormsg.Append("CodeSnippet(s) do not exist in the list/group. Please review the content and subject.");

//                    foreach (string s in listNoExist)
//                        errormsg.AppendLine("<br /> %%" + s + "%%");

//                    error = new ValidationError("ContentValidation", errormsg.ToString());
//                    ErrorList.Add(error);
//                    bSuccess = false;
//                }
//                else
//                    bSuccess = true;

//            }
//            catch (SqlException ex)
//            {
//                bSuccess = false;
//                error = new ValidationError("ContentValidation", ex.Message.ToString());
//                ErrorList.Add(error);
//            }
//            return bSuccess;
//        }
//    }
//}
