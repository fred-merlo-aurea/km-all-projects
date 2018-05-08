//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using System.Configuration;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Entity;
////using ECN_Framework.Common;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework.Communicator.Object
//{
//    public class BlastChampion : BlastAbstract
//    {
//        public BlastNew _Blast;

//        public BlastChampion()
//            : base()
//        {
//            BlastTypeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion;
//            BlastType = "champion";
//            StatusCodeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending;
//            StatusCode = "pending";
//            TestBlast = "n";
//            LayoutID = 0;
//        }

//        public override string Send()
//        {
//            return "This is a champion Blast";
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
//            if (BlastType.ToLower() != "champion")
//            {
//                error = new ValidationError("BlastType", "Blast type is invalid");
//                ErrorList.Add(error);
//            }
//            if (BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion)
//            {
//                error = new ValidationError("BlastTypeID", "Blast type id is invalid");
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

//            //validate content will not happen as content and subject are established when blast is sent.

//            if (ErrorList.Count > 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }
       
//        //public override bool Save()
//        //{
//        //    if (ValidateBlast())
//        //    {

//        //        //save the blast
//        //        if (BlastID > 0)
//        //        {
//        //            try
//        //            {
//        //                if (Blast.BlastExists(BlastID, this.CustomerID.Value))
//        //                {
//        //                    Blast.Update(this);
//        //                    return true;
//        //                }
//        //                else
//        //                {
//        //                    this.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
//        //                    return false;
//        //                }
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                this.ErrorList.Add(new ValidationError("Update", ex.ToString()));
//        //                return false;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            try
//        //            {
//        //                this.BlastID = Blast.Insert(this);
//        //                return true;
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                this.ErrorList.Add(new ValidationError("Insert", ex.ToString()));
//        //                return false;
//        //            }
//        //        }
//        //    }
//        //    else
//        //    {
//        //        return false;
//        //    }
//        //}

//        public override bool Save()
//        {
//            bool success = false;
//            if (Validate())
//            {
//                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
//                {
//                    connection.Open();
//                    SqlCommand command = connection.CreateCommand();
//                    SqlTransaction transaction = connection.BeginTransaction("InsertChampionBlastTransaction");
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
//                            }
//                            else
//                                success = true;

//                            success = ECN_Framework_BusinessLayer.Communicator.BlastFields.Save(ref fields, ref command);
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
//            throw new NotImplementedException();
//        }
//    }
//}
