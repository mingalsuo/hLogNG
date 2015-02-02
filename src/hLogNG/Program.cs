using System;
using System.Data;
using Npgsql;

namespace hLogNG
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			int interval = 60;
			while(true)
			{
				if (DateTime.Now.ToString("ss") == "00")
				{
					Console.WriteLine("Debug, Second 00");
					break;
				}
				else
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			while (true)
			{
				Console.WriteLine("Debug, " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzz"));
				System.Threading.Thread.Sleep (1000);
			}
			//2004-10-19 10:23:54+02
			//Console.WriteLine(DateTime.Now() + "\n" + DateTime.Now.ToString("HH:mm:ss") + "\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzz"));
//			string connectionString =
//				"Server=127.0.0.1;" +
//					"Database=DATABASE;" +
//					"User ID=USER;" +
//					"Password=PASSWORD;";
//			IDbConnection dbcon;
//			dbcon = new NpgsqlConnection(connectionString);
//			dbcon.Open();
//			IDbCommand dbcmd = dbcon.CreateCommand();
//			// requires a table to be created named employee
//			// with columns firstname and lastname
//			// such as,
//			//        CREATE TABLE employee (
//			//           firstname varchar(32),
//			//           lastname varchar(32));
//			string sql =
//				"SELECT firstname, lastname " +
//					"FROM employee";
//			dbcmd.CommandText = sql;
//			IDataReader reader = dbcmd.ExecuteReader();
//			while(reader.Read()) {
//				string FirstName = reader.GetString(reader.GetOrdinal("firstname"));
//				string LastName = reader.GetString(reader.GetOrdinal("lastname"));
//				Console.WriteLine("Name: " +
//				                  FirstName + " " + LastName);
//			}
//			// clean up
//			reader.Close();
//			reader = null;
//			dbcmd.Dispose();
//			dbcmd = null;
//			dbcon.Close();
//			dbcon = null;
		}
	}
}
