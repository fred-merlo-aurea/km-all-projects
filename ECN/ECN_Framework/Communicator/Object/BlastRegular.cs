//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Configuration;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Entity;
////using ECN_Framework.Common;
//using ECN_Framework_Common.Objects;
//using System.Data;
//using System.Data.SqlClient;

//namespace ECN_Framework.Communicator.Object
//{
//    public class BlastRegular : BlastAbstract
//    {
//        public BlastNew _Blast;

//        public BlastRegular()
//            : base()
//        {
//            BlastTypeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular;
//            BlastType = "html";
//            StatusCodeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending;
//            StatusCode = "pending";
//        }

//        public override string Send()
//        {
//            return "This is a regular Blast";
//        }

//        public override bool Validate()
//        {
//            //validate common fields
//            PreValidate();

//            //do blast type specific validation            
//            ValidationError error = null;
//            if (EmailFrom.Trim() == string.Empty)
//            {
//                error = new ValidationError("EmailFrom", "Blast email from is empty");
//                ErrorList.Add(error);
//            }
//            if (EmailFromName.Trim() == string.Empty)
//            {
//                error = new ValidationError("EmailFromName", "Blast email from name is empty");
//                ErrorList.Add(error);
//            }
//            if (StatusCode.ToLower() != "pending")
//            {
//                error = new ValidationError("StatusCode", "Blast status code is invalid");
//                ErrorList.Add(error);
//            }
//            if (StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending)
//            {
//                error = new ValidationError("StatusCodeID", "Blast status code id is invalid");
//                ErrorList.Add(error);
//            }
//            if (BlastType.ToLower() != "html" && BlastType.ToLower() != "text")
//            {
//                error = new ValidationError("BlastType", "Blast type is invalid");
//                ErrorList.Add(error);
//            }
//            if (BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular)
//            {
//                error = new ValidationError("BlastTypeID", "Blast type id is invalid");
//                ErrorList.Add(error);
//            }
//            if (LayoutID == null || CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(LayoutID.Value, CustomerID.Value)))
//            {
//                error = new ValidationError("LayoutID", "Blast layout is invalid");
//                ErrorList.Add(error);
//            }
//            if (ReplyTo.Trim() == string.Empty)
//            {
//                error = new ValidationError("ReplyTo", "Blast email reply to is empty");
//                ErrorList.Add(error);
//            }
//            if (TestBlast.Trim() == string.Empty)
//            {
//                error = new ValidationError("TestBlast", "Blast test blast is empty");
//                ErrorList.Add(error);
//            }
//            if (((RefBlastID.Trim() != string.Empty && RefBlastID.Trim() != "-1")) && (CustomerID == null || SendTime == null || (!ECN_Framework.Communicator.Entity.BlastNew.RefBlastsExists(RefBlastID.Trim(), CustomerID.Value, SendTime.Value))))
//            {
//                error = new ValidationError("RefBlastID", "Blast smart segment ref blast is invalid");
//                ErrorList.Add(error);
//            }
//            if (((BlastSuppression.Trim() != string.Empty)) && (CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(BlastSuppression.Trim(), CustomerID.Value))))
//            {
//                error = new ValidationError("BlastSuppression", "Blast suppression group is invalid");
//                ErrorList.Add(error);
//            }
//            if (AddOptOuts_to_MS == null)
//            {
//                error = new ValidationError("AddOptOuts_to_MS", "Blast ms opt outs is invalid");
//                ErrorList.Add(error);
//            }
//            if (DynamicFromName.Trim().Length > 0 && !(ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Exists(DynamicFromName.Trim(), GroupID.Value)))
//            {
//                error = new ValidationError("DynamicFromName", "Blast dynamic from name is invalid");
//                ErrorList.Add(error);
//            }
//            if (DynamicFromEmail.Trim().Length > 0 && !(ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Exists(DynamicFromEmail.Trim(), GroupID.Value)))
//            {
//                error = new ValidationError("DynamicFromEmail", "Blast dynamic from email is invalid");
//                ErrorList.Add(error);
//            }
//            if (DynamicReplyToEmail.Trim().Length > 0 && !(ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Exists(DynamicReplyToEmail.Trim(), GroupID.Value)))
//            {
//                error = new ValidationError("DynamicReplyToEmail", "Blast dynamic reply to email is invalid");
//                ErrorList.Add(error);
//            }
//            if (SmartSegmentID != null && (CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.SmartSegment.SmartSegmentExists(SmartSegmentID.Value))))
//            {
//                error = new ValidationError("SmartSegmentID", "Blast smart segment id is invalid");
//                ErrorList.Add(error);
//            }

//            //validate content
//            if (GroupID != null && EmailSubject.Trim().Length > 0 && LayoutID != null)
//            {
//                ValidateBlastContent();
//            }
//            else
//            {
//                error = new ValidationError("ContentValidation", "Could not validate blast content");
//                ErrorList.Add(error);
//            }

//            if (ErrorList.Count > 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }

//        private static bool ValidateList(ref List<BlastRegular> blastList)
//        {
//            bool success = true;
//            //validate
//            foreach (BlastRegular blast in blastList)
//            {
//                if(!blast.Validate())
//                {
//                    success = false;
//                }
//            }

//            return success;
//        }

//        #region Moved to Abstract
//        //public override bool ValidateBlastContent()
//        //{
//        //    bool bSuccess = true;
//        //    ValidationError error = null;

//        //    try
//        //    {
//        //        System.Collections.Generic.List<string> listCS = new System.Collections.Generic.List<string>();
//        //        StringBuilder sbCS = new StringBuilder();
//        //        sbCS.Append(" select lower(c.name) as shortname from sysobjects o join syscolumns c on o.id = c.id where o.name = 'Emails' ");
//        //        sbCS.Append(" union ");
//        //        sbCS.Append(" select shortname as shortname from groupdatafields where groupID = " + GroupID.Value);

//        //        DataTable dtCS = new DataTable();
//        //        dtCS = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sbCS.ToString(), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//        //        foreach (DataRow dr in dtCS.Rows)
//        //        {
//        //            listCS.Add(dr["shortname"].ToString().ToLower());
//        //        }


//        //        System.Collections.Generic.List<string> listLY = new System.Collections.Generic.List<string>();
//        //        StringBuilder sbLY = new StringBuilder();
//        //        sbLY.Append(" select ContentSource as Content from Layouts ");
//        //        sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9  ");
//        //        sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ") union all ");
//        //        sbLY.Append(" select ContentText as Content from Layouts ");
//        //        sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9 ");
//        //        sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ") union all ");
//        //        sbLY.Append(" select ContentMobile as Content from Layouts ");
//        //        sbLY.Append(" join Content on  Content.ContentID = Layouts.ContentSlot1 or Content.ContentID = Layouts.ContentSlot2 or  Content.ContentID = Layouts.ContentSlot3 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot4 or Content.ContentID = Layouts.ContentSlot5 or Content.ContentID = Layouts.ContentSlot6 or ");
//        //        sbLY.Append(" Content.ContentID = Layouts.ContentSlot7 or Content.ContentID = Layouts.ContentSlot8 or Content.ContentID = Layouts.ContentSlot9 ");
//        //        sbLY.Append(" where layouts.layoutID in (" + LayoutID.Value + ")");

//        //        DataTable dtLY = new DataTable();
//        //        dtLY = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sbLY.ToString(), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());

//        //        foreach (DataRow dr in dtLY.Rows)
//        //        {
//        //            listLY.Add(dr["Content"].ToString().ToLower());
//        //        }
//        //        listLY.Add(EmailSubject.Trim().ToLower());

//        //        System.Collections.Generic.List<string> subLY = new System.Collections.Generic.List<string>();
//        //        foreach (string s in listLY)
//        //        {
//        //            #region Badly Formed Snippets
//        //            //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
//        //            System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//        //            System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
//        //            if (MatchList.Count > 0)
//        //            {
//        //                if ((MatchList.Count % 2) != 0)
//        //                {
//        //                    //return error
//        //                    error = new ValidationError("ContentValidation", "Incorrectly formed code snippet");
//        //                    ErrorList.Add(error);
//        //                    return false;
//        //                }
//        //                else
//        //                {
//        //                    System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//        //                    System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
//        //                    if ((MatchList.Count / 2) > MatchListGood.Count)
//        //                    {
//        //                        //return error
//        //                        error = new ValidationError("ContentValidation", "Incorrectly formed code snippet");
//        //                        ErrorList.Add(error);
//        //                        return false;
//        //                    }
//        //                }
//        //            }
//        //            #endregion

//        //            //%% and ##
//        //            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//        //            System.Text.RegularExpressions.MatchCollection MatchList1 = reg1.Matches(s);

//        //            foreach (System.Text.RegularExpressions.Match m in MatchList1)
//        //            {
//        //                if (!string.IsNullOrEmpty(m.Value.ToString()))
//        //                {
//        //                    if (!subLY.Contains(m.Value.ToString().Replace("%%", string.Empty)))
//        //                        subLY.Add(m.Value.ToString().Replace("%%", string.Empty));
//        //                }
//        //            }
//        //        }
//        //        System.Collections.Generic.List<string> listNoExist = new System.Collections.Generic.List<string>();
//        //        foreach (string s in subLY)
//        //        {
//        //            if (!listCS.Contains(s))
//        //                listNoExist.Add(s);
//        //        }

//        //        listNoExist.Remove("blastid");
//        //        listNoExist.Remove("groupid");
//        //        listNoExist.Remove("groupname");
//        //        listNoExist.Remove("emailtofriend");
//        //        listNoExist.Remove("conversiontrkcde");
//        //        listNoExist.Remove("unsubscribelink");
//        //        listNoExist.Remove("lastchanged");
//        //        listNoExist.Remove("createdon");
//        //        listNoExist.Remove("publicview");
//        //        listNoExist.Remove("company_address");
//        //        listNoExist.Remove("surveytitle");
//        //        listNoExist.Remove("surveylink");
//        //        listNoExist.Remove("currdate");

//        //        listNoExist.Remove("customer_name");
//        //        listNoExist.Remove("customer_address");
//        //        listNoExist.Remove("customer_webaddress");

//        //        listNoExist.Remove("customer_udf1");
//        //        listNoExist.Remove("customer_udf2");
//        //        listNoExist.Remove("customer_udf3");
//        //        listNoExist.Remove("customer_udf4");
//        //        listNoExist.Remove("customer_udf5");

//        //        if (listNoExist.Count > 0)
//        //        {
//        //            StringBuilder errormsg = new StringBuilder();
//        //            errormsg.Append("CodeSnippet(s) do not exist in the list/group. Please review the content and subject.");

//        //            foreach (string s in listNoExist)
//        //                errormsg.AppendLine("<br /> %%" + s + "%%");

//        //            error = new ValidationError("ContentValidation", errormsg.ToString());
//        //            ErrorList.Add(error);
//        //            bSuccess = false;
//        //        }
//        //        else
//        //            bSuccess = true;

//        //    }
//        //    catch (SqlException ex)
//        //    {
//        //        bSuccess = false;
//        //        error = new ValidationError("ContentValidation", ex.Message.ToString());
//        //        ErrorList.Add(error);
//        //    }
//        //    return bSuccess;
//        //}
//        #endregion

//        public override bool Save()
//        {
//            bool success = false;
//            if (Validate())
//            {
//                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
//                {
//                    connection.Open();
//                    SqlCommand command = connection.CreateCommand();
//                    SqlTransaction transaction = connection.BeginTransaction("InsertRegularBlastTransaction");
//                    command.Connection = connection;
//                    command.Transaction = transaction;
//                    command.CommandTimeout = 0;
//                    try
//                    {
//                        ECN_Framework_Entities.Communicator.BlastFields fields = Fields;
//                        //save the blast
//                        if (BlastID > 0)
//                        {
//                            if (BlastNew.Exists(BlastID, this.CustomerID.Value))
//                            {
//                                BlastNew.Update(this, command);
//                                if (Fields != null)
//                                {
//                                    Fields.BlastID = BlastID;
//                                    Fields.CustomerID = CustomerID;
//                                    success = ECN_Framework_BusinessLayer.Communicator.BlastFields.Save(ref fields, ref command);
//                                }
//                                else
//                                    success = true;
//                            }
//                            else
//                            {
//                                this.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
//                                return false;
//                            }
//                        }
//                        else
//                        {
//                            BlastID = BlastNew.Insert(this, command);
//                            if (Fields != null)
//                            {
//                                Fields.BlastID = BlastID;
//                                Fields.CustomerID = CustomerID;
//                                success = ECN_Framework_BusinessLayer.Communicator.BlastFields.Save(ref fields, ref command);
//                            }
//                            else
//                                success = true;
//                        }
//                        if (success)
//                        {
//                            transaction.Commit();
//                        }
//                        else
//                        {
//                            foreach (ValidationError saveError in Fields.ErrorList)
//                            {
//                                ErrorList.Add(saveError);
//                            }
//                            transaction.Rollback();
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        ErrorList.Add(new ValidationError("Save", ex.ToString()));
//                        success = false;
//                    }
//                }
//            }
//            return success;
//        }

//        public override bool Save(SqlCommand command)
//        {
//            bool success = false;
//            if (Validate())
//            {
//                ECN_Framework_Entities.Communicator.BlastFields fields = Fields;
//                //save the blast
//                if (BlastID > 0)
//                {
//                    try
//                    {
//                        if (BlastNew.Exists(BlastID, this.CustomerID.Value))
//                        {
//                            BlastNew.Update(this, command);
//                            if (Fields != null)
//                            {
//                                Fields.BlastID = BlastID;
//                                Fields.CustomerID = CustomerID;
//                                success = ECN_Framework_BusinessLayer.Communicator.BlastFields.Save(ref fields, ref command);
//                            }
//                            else
//                            {
//                                success = true;
//                            }
//                        }
//                        else
//                        {
//                            this.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
//                            success = false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Update", ex.ToString()));
//                        success = false;
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        this.BlastID = BlastNew.Insert(this, command);
//                        if (Fields != null)
//                        {
//                            Fields.BlastID = BlastID;
//                            Fields.CustomerID = CustomerID;
//                            success = ECN_Framework_BusinessLayer.Communicator.BlastFields.Save(ref fields, ref command);
//                        }
//                        else
//                        {
//                            success = true;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Insert", ex.ToString()));
//                        return false;
//                    }
//                }
//                if (!success)
//                {
//                    foreach (ValidationError saveError in Fields.ErrorList)
//                    {
//                        ErrorList.Add(saveError);
//                    }
//                }
//            }
//            else
//            {
//                success = false;
//            }
//            return success;
//        }

//        public static bool SaveList(ref List<BlastRegular> blastList)
//        {
//            bool success = false;

//            if (ValidateList(ref blastList))
//            {
//                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
//                {
//                    connection.Open();
//                    SqlCommand command = connection.CreateCommand();
//                    SqlTransaction transaction = connection.BeginTransaction("InsertRegularBlastListTransaction");
//                    command.Connection = connection;
//                    command.Transaction = transaction;
//                    command.CommandTimeout = 0;
//                    try
//                    {
//                        foreach (ECN_Framework.Communicator.Object.BlastRegular blast in blastList)
//                        {
//                            success = blast.Save(command);
//                            if (!success)
//                            {
//                                transaction.Rollback();
//                                break;
//                            }
//                        }
//                        if (success)
//                        {
//                            transaction.Commit();
//                        }
//                    }
//                    catch (Exception)
//                    {
//                        transaction.Rollback();
//                        success = false;
//                    }
//                }
//            }            
            
//            return success;
//        }
//    }
//}
