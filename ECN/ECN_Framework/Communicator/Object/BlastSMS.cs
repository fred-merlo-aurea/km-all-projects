//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Entity;
////using ECN_Framework.Common;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework.Communicator.Object
//{
//    public class BlastSMS : BlastAbstract
//    {
//        public BlastNew _Blast;

//        public BlastSMS()
//            : base()
//        {
//            BlastTypeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS; 
//            BlastType = "sms";
//            StatusCodeID = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed;
//            StatusCode = "deployed";
//            TestBlast = "n";
//        }

//        public override string Send()
//        {
//            return "This is an SMS Blast";
//        }

//        public override bool Validate()
//        {
//            //validate common fields
//            PreValidate();

//            //do blast type specific validation            
//            ValidationError error = null;
//            if (StatusCode.ToLower() != "deployed")
//            {
//                error = new ValidationError("StatusCode", "Blast status code is invalid");
//                ErrorList.Add(error);
//            }
//            if (StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed)
//            {
//                error = new ValidationError("StatusCodeID", "Blast status code id is invalid");
//                ErrorList.Add(error);
//            }
//            if (BlastType.ToLower() != "sms")
//            {
//                error = new ValidationError("BlastType", "Blast type is invalid");
//                ErrorList.Add(error);
//            }
//            if (BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS)
//            {
//                error = new ValidationError("BlastTypeID", "Blast type id is invalid");
//                ErrorList.Add(error);
//            }
//            if (LayoutID == null || CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(LayoutID.Value, CustomerID.Value)))
//            {
//                error = new ValidationError("LayoutID", "Blast layout is invalid");
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
//            if (SmartSegmentID != null && (CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.SmartSegment.SmartSegmentExists(SmartSegmentID.Value))))
//            {
//                error = new ValidationError("SmartSegmentID", "Blast smart segment id is invalid");
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
        
//        public override bool Save()
//        {
//            if (Validate())
//            {

//                //save the blast
//                if (BlastID > 0)
//                {
//                    try
//                    {
//                        if (BlastNew.Exists(BlastID, this.CustomerID.Value))
//                        {
//                            BlastNew.Update(this);
//                            return true;
//                        }
//                        else
//                        {
//                            this.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
//                            return false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Update", ex.ToString()));
//                        return false;
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        this.BlastID = BlastNew.Insert(this);
//                        return true;
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Insert", ex.ToString()));
//                        return false;
//                    }
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public override bool Save(SqlCommand command)
//        {
//            if (Validate())
//            {
//                //save the blast
//                if (BlastID > 0)
//                {
//                    try
//                    {
//                        if (BlastNew.Exists(BlastID, this.CustomerID.Value))
//                        {
//                            BlastNew.Update(this, command);
//                            return true;
//                        }
//                        else
//                        {
//                            this.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
//                            return false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Update", ex.ToString()));
//                        return false;
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        this.BlastID = BlastNew.Insert(this, command);
//                        return true;
//                    }
//                    catch (Exception ex)
//                    {
//                        this.ErrorList.Add(new ValidationError("Insert", ex.ToString()));
//                        return false;
//                    }
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
