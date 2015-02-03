using System;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;

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
				Console.Write(values[values.Count - 1]);
			}
		}
		private String readValues(String item)
		{
			Process myProcess = new Process ();
			try
			{
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.FileName = "python";
				String args = String.Format("{0} {1} {2} {3}",
					                            data["MAIN"]["script"],
					                            data ["MAIN"] ["device"],
					                            data ["MAIN"] ["devicetype"],
					                            item);
				myProcess.StartInfo.Arguments = args;
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.StartInfo.RedirectStandardOutput = true;
				myProcess.Start ();
				string output = myProcess.StandardOutput.ReadToEnd();
				myProcess.WaitForExit();
				return output;
			}
			catch (Exception e)
			{
				Console.WriteLine (e.Message);
			}
			return null;
		}
	}
}

