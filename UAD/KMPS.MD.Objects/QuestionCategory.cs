using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class QuestionCategory
    {
        private const string CacheKeyQuestionCategory = "QUESTIONCATEGORY";
        private const string ExistsByCategoryNameCommandText = "e_QuestionCategory_Exists_ByCategoryName";
        private const string ExistsByParentIdNameCommandText = "e_QuestionCategory_Exists_ByParentID";
        private const string CategoryNameParameterName = "@CategoryName";
        private const string CategoryIdParameterName = "@QuestionCategoryID"; 
        private const string ParentIdParameterName = "@ParentID"; 

         public QuestionCategory() { }

        #region Properties
        [DataMember]
        public int QuestionCategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        #endregion

        #region Data
        public static List<QuestionCategory> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<QuestionCategory> questioncategory = (List<QuestionCategory>)CacheUtil.GetFromCache("QUESTIONCATEGORY", DatabaseName);

                if (questioncategory == null)
                {
                    questioncategory = GetData(clientconnection);

                    CacheUtil.AddToCache("QUESTIONCATEGORY", questioncategory, DatabaseName);
                }

                return questioncategory;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<QuestionCategory> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<QuestionCategory> retList = new List<QuestionCategory>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_QuestionCategory_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<QuestionCategory> builder = DynamicBuilder<QuestionCategory>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    QuestionCategory x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }
        public static QuestionCategory GetByID(KMPlatform.Object.ClientConnections clientconnection, int questionCategoryID)
        {
            QuestionCategory fc = GetAll(clientconnection).Find(x => x.QuestionCategoryID == questionCategoryID);
            return fc;
        }

        public static bool ExistsByCategoryName(ClientConnections clientConnections, int questionCategoryId, string categoryName)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter(CategoryNameParameterName, categoryName),
                new SqlParameter(CategoryIdParameterName, questionCategoryId)
            };

            return DataFunctions.ExecuteScalar(clientConnections, ExistsByCategoryNameCommandText, sqlParameters) > 0;
        }

        public static bool ExistsByParentID(ClientConnections clientConnections, int parentId)
        {
            var sqlParameters = new List<SqlParameter> { new SqlParameter(ParentIdParameterName, parentId) };

            return DataFunctions.ExecuteScalar(clientConnections, ExistsByParentIdNameCommandText, sqlParameters) > 0;
        }

        public static void DeleteCache(ClientConnections clientConnections)
        {
            if (!CacheUtil.IsCacheEnabled())
            {
                return;
            }

            var databaseName = DataFunctions.GetDBName(clientConnections);

            CacheUtil.SafeRemoveFromCache(CacheKeyQuestionCategory, databaseName);
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, QuestionCategory questionCategory)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_QuestionCategory_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionCategoryID", questionCategory.QuestionCategoryID));
            cmd.Parameters.Add(new SqlParameter("@CategoryName", questionCategory.CategoryName));
            cmd.Parameters.Add(new SqlParameter("@ParentID", questionCategory.ParentID));
            if (questionCategory.QuestionCategoryID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", questionCategory.UpdatedUserID));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", questionCategory.CreatedUserID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int QuestionCategoryID, int UserID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_QuestionCategory_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionCategoryID", QuestionCategoryID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
