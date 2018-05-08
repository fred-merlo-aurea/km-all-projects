using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Ecn.Framework.DataLayer.Helpers;
using Ecn.Framework.DataLayer.Interfaces;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastSchedule
    {
        private const string InsertBlastScheduleStoredProcedure = "spInsertBlastSchedule";
        private const string InsertBlastScheduleDaysStoredProcedure = "spInsertBlastScheduleDays";
        private const string InsertBlastScheduleHistoryStoredProcedure = "spInsertBlastScheduleHistory";
        private const string UpdateBlastScheduleStoredProcedure = "spUpdateBlastSchedule";
        private static IDatabaseAdapter _databaseAdapter;
        
        static BlastSchedule()
        {
            _databaseAdapter = new SqlDatabaseAdapter();
        }

        public static void Initialize(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        public static bool Exists(int blastScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(BlastScheduleID) from BlastSchedule where BlastScheduleID = @BlastScheduleID";
            cmd.Parameters.AddWithValue("@BlastScheduleID", blastScheduleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.BlastSchedule GetByBlastScheduleID(int blastScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastSchedule where BlastScheduleID = @BlastScheduleID";
            cmd.Parameters.AddWithValue("@BlastScheduleID", blastScheduleID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.BlastSchedule GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bs.* from BlastSchedule bs join Blast b on bs.BlastScheduleID = b.BlastScheduleID where b.BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.BlastSchedule Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastSchedule retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastSchedule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastSchedule>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.BlastSchedule schedule)
        {
            int blastScheduleID = 0;
            //need to validate objects first

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
            {
                connection.Open();
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var cmd = DataFunctions.BuildCommand
                    (
                        connection,
                        InsertBlastScheduleStoredProcedure,
                        new Dictionary<string, object>()
                        {
                            { "@SchedTime", schedule.SchedTime },
                            { "@SchedStartDate", schedule.SchedStartDate },
                            { "@SchedEndDate", schedule.SchedEndDate },
                            { "@Period", schedule.Period },
                            { "@CreatedBy", schedule.CreatedBy },
                            { "@SplitType", schedule.SplitType }
                        }
                    );

                    var exec = DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                    blastScheduleID = Convert.ToInt32(exec.ToString());

                    if (schedule.DaysList == null || schedule.DaysList.Count == 0)
                    {
                        scope.Complete();
                        return 0;
                    }

                    foreach (var days in schedule.DaysList)
                    {
                        cmd = DataFunctions.BuildCommand
                        (
                            connection,
                            InsertBlastScheduleDaysStoredProcedure,
                            new Dictionary<string, object>()
                            {
                                { "@BlastScheduleID", blastScheduleID },
                                { "@DayToSend", days.DayToSend },
                                { "@IsAmount", days.IsAmount },
                                { "@Total", days.Total },
                                { "@Weeks", days.Weeks },
                            }
                        );
                        cmd.ExecuteNonQuery();
                    }

                    scope.Complete();
                }
            }

            return blastScheduleID;
        }

        public static int Update(ECN_Framework_Entities.Communicator.BlastSchedule schedule, int blastID)
        {
            var blastScheduleID = 0;
            //need to validate objects first
            var parameters = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("@BlastID", blastID),
                new KeyValuePair<string, object>("@Action", "UPDATE")
            };

            return RunDeleteOrUpdateTransaction(blastScheduleID, "UpdateBlastTransaction", parameters, true, schedule);
        }

        public static void Delete(int blastScheduleID)
        {
            var parameters = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("@BlastScheduleID", blastScheduleID),
                new KeyValuePair<string, object>("@Action", "DELETE")
            };

            RunDeleteOrUpdateTransaction(blastScheduleID, "DeleteBlastTransaction", parameters, false, null);
        }

        private static int RunDeleteOrUpdateTransaction(
            int blastScheduleID, 
            string transactionName, 
            IList<KeyValuePair<string, object>> parameters, 
            bool isUpdate,
            ECN_Framework_Entities.Communicator.BlastSchedule schedule)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString()))
            {
                _databaseAdapter.OpenConnection(connection);
                var command = _databaseAdapter.CreateCommand(connection);
                var transaction = _databaseAdapter.BeginTransaction(connection, transactionName);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandTimeout = 0;

                //archive history
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spInsertBlastScheduleHistory";
                foreach (var parameter in parameters)
                {
                    _databaseAdapter.AddParameterWithValue(command, parameter.Key, parameter.Value);
                }

                try
                {
                    if (isUpdate)
                    {
                        blastScheduleID = Convert.ToInt32(_databaseAdapter.ExecuteScalar(command).ToString());
                        if (blastScheduleID <= 0)
                        {
                            _databaseAdapter.RollbackTransaction(transaction);
                            return blastScheduleID;
                        }
                    }
                    else
                    {
                        _databaseAdapter.ExecuteNonQuery(command);
                    }

                    DeleteScheduleDays(blastScheduleID, command);

                    if (isUpdate)
                    {
                        UpdateBlastSchedule(blastScheduleID, schedule, command);
                        InsertDays(blastScheduleID, schedule, command);
                    }
                    else
                    {
                        DeleteSchedule(blastScheduleID, command);
                    }
                    
                    _databaseAdapter.CommitTransaction(transaction);
                }
                catch (Exception)
                {
                    blastScheduleID = 0;
                    AttemptToRollback(transaction);
                }
            }
            return blastScheduleID;
        }

        private static void AttemptToRollback(IDbTransaction transaction)
        {
            try
            {
                //send ex in email
                _databaseAdapter.RollbackTransaction(transaction);

            }
            catch
            {
                //send ex2 (rollback issue) in email
            }
        }

        private static void InsertDays(int blastScheduleID, ECN_Framework_Entities.Communicator.BlastSchedule schedule, IDbCommand command)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException("schedule");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "spInsertBlastScheduleDays";
            if (schedule.DaysList.Any())
            {
                foreach (ECN_Framework_Entities.Communicator.BlastScheduleDays days in schedule.DaysList)
                {
                    command.Parameters.Clear();
                    _databaseAdapter.AddParameterWithValue(command, "@BlastScheduleID", blastScheduleID);
                    _databaseAdapter.AddParameterWithValue(command, "@DayToSend", days.DayToSend);
                    _databaseAdapter.AddParameterWithValue(command, "@IsAmount", days.IsAmount);
                    _databaseAdapter.AddParameterWithValue(command, "@Total", days.Total);
                    _databaseAdapter.AddParameterWithValue(command, "@Weeks", days.Weeks);
                    _databaseAdapter.ExecuteNonQuery(command);
                }
            }
        }

        private static void UpdateBlastSchedule(int blastScheduleID, ECN_Framework_Entities.Communicator.BlastSchedule schedule, IDbCommand command)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException("schedule");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "spUpdateBlastSchedule";
            command.Parameters.Clear();
            _databaseAdapter.AddParameterWithValue(command, "@BlastScheduleID", blastScheduleID);
            _databaseAdapter.AddParameterWithValue(command, "@SchedTime", schedule.SchedTime);
            _databaseAdapter.AddParameterWithValue(command, "@SchedStartDate", schedule.SchedStartDate);
            _databaseAdapter.AddParameterWithValue(command, "@SchedEndDate", schedule.SchedEndDate);
            _databaseAdapter.AddParameterWithValue(command, "@Period", schedule.Period);
            _databaseAdapter.AddParameterWithValue(command, "@UpdatedBy", schedule.UpdatedBy);
            _databaseAdapter.AddParameterWithValue(command, "@SplitType", schedule.SplitType);
            _databaseAdapter.ExecuteNonQuery(command);
        }

        private static void DeleteScheduleDays(int blastScheduleID, IDbCommand command)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = "delete BlastScheduleDays where BlastScheduleID = @BlastScheduleID";
            command.Parameters.Clear();
            _databaseAdapter.AddParameterWithValue(command, "@BlastScheduleID", blastScheduleID);
            _databaseAdapter.ExecuteNonQuery(command);
        }

        private static void DeleteSchedule(int blastScheduleID, IDbCommand command)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = "delete BlastSchedule where BlastScheduleID = @BlastScheduleID";
            command.Parameters.Clear();
            _databaseAdapter.AddParameterWithValue(command, "@BlastScheduleID", blastScheduleID);
            _databaseAdapter.ExecuteNonQuery(command);
        }
    }
}
