//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data.SqlClient;
//using System.Data;
//using System.Configuration;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Entity;
////using ECN_Framework.Common;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework.Communicator.Object
//{
//    public class BlastABMaster
//    {
//        #region Properties
//        public ECN_Framework_Entities.Communicator.Samples Sample;
//        public BlastAbstract BlastA;
//        public BlastAbstract BlastB;
//        //validation
//        public List<ValidationError> ErrorList { get; set; }
//        #endregion

//        public BlastABMaster()
//        {
//            Sample = null;
//            BlastA = null;
//            BlastB = null;
//            ErrorList = new List<ValidationError>();
//        }

//        public bool ValidateObjects()
//        {
//            //validate
//            ECN_Framework_BusinessLayer.Communicator.Samples.Validate(ref Sample);
//            BlastA.Validate();
//            BlastB.Validate();

//            foreach (ValidationError error in Sample.ErrorList)
//            {
//                ErrorList.Add(error);
//            }
//            foreach (ValidationError error in BlastA.ErrorList)
//            {
//                ErrorList.Add(error);
//            }
//            foreach (ValidationError error in BlastB.ErrorList)
//            {
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

//        public bool Save()
//        {
//            bool success = false;
//            if (ValidateObjects())
//            {
//                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
//                {
//                    connection.Open();
//                    SqlCommand command = connection.CreateCommand();
//                    SqlTransaction transaction = connection.BeginTransaction("InsertABBlastTransaction");
//                    command.Connection = connection;
//                    command.Transaction = transaction;
//                    command.CommandTimeout = 0;
//                    try
//                    {
//                        success = ECN_Framework_BusinessLayer.Communicator.Samples.Save(ref Sample, ref command);
//                        if (success)
//                        {
//                            BlastA.SampleID = Sample.SampleID;
//                            BlastB.SampleID = Sample.SampleID;
//                            success = BlastA.Save(command);
//                        }
//                        else
//                        {
//                            foreach (ValidationError saveError in Sample.ErrorList)
//                            {
//                                ErrorList.Add(saveError);
//                            }
//                        }
//                        if (success)
//                        {
//                            success = BlastB.Save(command);
//                        }
//                        else
//                        {
//                            foreach (ValidationError saveError in BlastA.ErrorList)
//                            {
//                                ErrorList.Add(saveError);
//                            }
//                        }
//                        if (success)
//                        {
//                            transaction.Commit();
//                        }
//                        else
//                        {
//                            foreach (ValidationError saveError in BlastB.ErrorList)
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
//            else
//                success = false;

//            return success;           
//        }
//    }
//}
