using System;
using System.Collections.Generic;
using System.Data;
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

			sqlQuery ("CREATE TABLE IF NOT EXISTS log (id SERIAL PRIMARY KEY, timestamp TIMESTAMP WITH TIME ZONE, object TEXT, value REAL);");
		}

		private String sqlQuery (String sql)
		{
			string connectionString = String.Format ("Server={0};Database={1};User ID={2};Password={3};",
			                                        data ["MAIN"] ["dbaddr"],
			                                        data ["MAIN"] ["dbname"],
			                                        data ["MAIN"] ["dbuser"],
			                                        data ["MAIN"] ["dbpass"]);
			Console.WriteLine ("connectionString: {0}", connectionString);
			Console.WriteLine (sql);

			using (IDbConnection dbcon = new NpgsqlConnection (connectionString)) {
				dbcon.Open ();
				IDbCommand dbcmd = dbcon.CreateCommand ();
				dbcmd.CommandText = sql;

				Int32 rowsaffected = dbcmd.ExecuteNonQuery ();
				Console.WriteLine ("Rows: {0}", rowsaffected);

				dbcmd.Dispose ();
				dbcmd = null;
				dbcon.Close ();

				return connectionString;
			}
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
				storeValues(objList, values, e.SignalTime);
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
		private void storeValues(String[] objList, List<String> values, DateTime timeStamp)
		{
			Console.WriteLine ("Values count {0}", values.Count);

			for (int i = 0; i < values.Count; i++)
			{
				Console.WriteLine ("i: {0}", i);
				sqlQuery (String.Format("INSERT INTO log (timestamp, object, value) " +
				                        "VALUES ('{0}','{1}',{2});",
				                        timeStamp.ToString ("yyyy-MM-dd HH:mm:ss.fffzz"),
				                        objList[i],
				                        values[i]));
			}
		}
	}
}

