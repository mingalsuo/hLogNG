using System;
using System.Data;
using System.Timers;
using Npgsql;

namespace hLogNG
{
	class MainClass
	{
		public static void doStuff (object source, ElapsedEventArgs e)
		{
			Console.WriteLine (DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fffzz"));
		}

		public static void Main (string[] args)
		{
			var parser = new IniParser.FileIniDataParser ();
			IniParser.Model.IniData data = parser.ReadFile ("Config.ini");

			var interval = Convert.ToInt32 (data ["MAIN"] ["interval"]);
			String objects = data ["MAIN"] ["objects"];
			char[] delim = {','};
			String[] objList = objects.Split (delim);

			foreach (string s in objList)
			{
				System.Console.WriteLine (s);
			}

			interval = interval * 1000;
			// Create a timer with a two second interval.
			System.Timers.Timer aTimer = new System.Timers.Timer (interval);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += doStuff;

			while (DateTime.Now.Second != 0)
			{
				System.Threading.Thread.Sleep (100);
			}

			aTimer.Enabled = true;

			Console.WriteLine ("Press the Enter key to exit the program... ");
			Console.ReadLine ();
			Console.WriteLine ("Terminating the application...");

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
