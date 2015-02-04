using System;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;
using Npgsql;

namespace hLogNG
{
	public class Logger
	{
		public int interval;
		private IniParser.Model.IniData data;

		public Logger (String IniFile)
		{
			var parser = new IniParser.FileIniDataParser ();
			data = parser.ReadFile (IniFile);

			interval = Convert.ToInt32 (data ["MAIN"] ["interval"]);
			String devicetype = data ["MAIN"] ["devicetype"];
			String device = data ["MAIN"] ["device"];
			String script = data ["MAIN"] ["script"];
		}

		public void onTimerEvent (object source, ElapsedEventArgs e)
		{
			Console.WriteLine ("{0}", e.SignalTime.ToString ("yyyy-MM-dd HH:mm:ss.fffzz"));
			String objects = data ["MAIN"] ["objects"];
			char[] delim = {','};
			String[] objList = objects.Split (delim);

			List<String> values = new List<String>();
			foreach (var item in objList)
			{
				values.Add(readValues(item));
				Console.WriteLine(values[values.Count - 1]);
			}
			if (values.Count == objList.Length)
			{
				storeValues(objList, values);
			}
		}
		private String readValues(String item)
		{
			try
			{
				using (Process myProcess = new Process ())
				{
					myProcess.StartInfo.UseShellExecute = false;
					myProcess.StartInfo.FileName = "python";
					String args = String.Format("{0} {1} {2} \"{3}\"",
						                            data["MAIN"]["script"],
						                            data ["MAIN"] ["device"],
						                            data ["MAIN"] ["devicetype"],
						                            item);
					myProcess.StartInfo.Arguments = args;
					myProcess.StartInfo.CreateNoWindow = true;
					myProcess.StartInfo.RedirectStandardOutput = true;
					myProcess.Start ();
					string output = myProcess.StandardOutput.ReadToEnd();
					output = output.Replace("\n", "");
					myProcess.WaitForExit();
					return output;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine (e.Message);
			}
			return null;
		}
		private void storeValues(String[] objList, List<String> values)
		{
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

