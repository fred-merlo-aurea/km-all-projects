using System;
using ecn.common.classes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace ecn.communicator.classes
{
	
	/// A stub class that has an ID for database access. You can also ask it for a SqlConnection if you 
	/// need several paramitized queries.
	
	public class DatabaseAccessor
	{
        int id;
        protected string connStr;
		
		/// Creates the object with id 0 and sets the connection string
		
		public DatabaseAccessor()
		{
            connStr = ConfigurationManager.AppSettings["com"];
			id = 0;
		}
		
		/// Gives a database connection for prepaired queries
		
		/// <returns>The SqlConneciton to the DB</returns>
        public SqlConnection GetDbConnection() {
            SqlConnection new_con = new SqlConnection(connStr);
            new_con.Open();
            return new_con;
        }

		public SqlConnection GetDbConnection(string productName) {			
			switch(productName) {
				case "accounts":
					return new SqlConnection(con_accounts);					
				case "collector":
					return new SqlConnection(con_collector);					
				case "creator":
					return  new SqlConnection(con_creator);					
				case "communicator":
					return  new SqlConnection(con_communicator);					
				default:
					return new SqlConnection(connStr);					
			}
		}

		
		/// Object initialization with an id of input_id
		
		/// <param name="input_id">The ID of this object</param>
        public DatabaseAccessor(int input_id):this() {
            id = input_id;
        }

		
		/// Object initialization and munging of string into int
		
		/// <param name="id_string"> A string to convert to an int </param>
        public DatabaseAccessor(string id_string): this(Convert.ToInt32(id_string)){
        }

		
		/// Gets the ID
		
		/// <returns> Object ID</returns>
        public int ID() { return id; }

		
		/// Sets the ID
		
		/// <param name="new_id"> What object ID you want it switched to</param>
        public void ID(int new_id) {
            id = new_id;
        }
		
		public static string accountsdb=ConfigurationManager.AppSettings["accountsdb"];
		public static string communicatordb	= ConfigurationManager.AppSettings["communicatordb"];
		public static string con_creator=ConfigurationManager.AppSettings["cre"];
		public static string con_collector=ConfigurationManager.AppSettings["col"];
		public static string con_communicator=ConfigurationManager.AppSettings["com"];
		public static string con_accounts=ConfigurationManager.AppSettings["act"];
		public static string product_name=ConfigurationManager.AppSettings["product_name"];

	}
}
