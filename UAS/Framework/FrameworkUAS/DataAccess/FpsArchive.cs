using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
	public class FpsArchive
	{
		public static int Save(Entity.FpsArchive fpsArchive)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "e_FpsArchive_Save";
			cmd.Parameters.Add(new SqlParameter("@FpsArchiveId", fpsArchive.FpsArchiveId));
			cmd.Parameters.Add(new SqlParameter("@ClientId", fpsArchive.ClientId));
			cmd.Parameters.Add(new SqlParameter("@SourceFileId", fpsArchive.SourceFileId));
			cmd.Parameters.Add(new SqlParameter("@ObjectType", fpsArchive.ObjectType));
			cmd.Parameters.Add(new SqlParameter("@ObjectJson", fpsArchive.ObjectJson));
			cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)fpsArchive.DateCreated ?? DBNull.Value));
			cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)fpsArchive.DateUpdated ?? DBNull.Value));
			cmd.Parameters.Add(new SqlParameter("@CreatedByUserId", fpsArchive.CreatedByUserId));
			cmd.Parameters.Add(new SqlParameter("@UpdatedByUserId", fpsArchive.UpdatedByUserId));

			return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
		}

		public static void DeleteAll()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Truncate FpsArchive";
			KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString());
		}
	}
}