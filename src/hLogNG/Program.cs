using System;
using System.Data;
using System.Timers;

namespace hLogNG
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Logger MyLogger = new Logger ("Config.ini");

			int interval = MyLogger.interval * 1000;
			// Create a timer with a two second interval.
			System.Timers.Timer aTimer = new System.Timers.Timer (interval);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += MyLogger.onTimerEvent;

			while (DateTime.Now.Second != 0)
			{
				System.Threading.Thread.Sleep (10);
			}

			aTimer.Enabled = true;

			Console.WriteLine ("Press the Enter key to exit the program... ");
			Console.ReadLine ();
			Console.WriteLine ("Terminating the application...");


		}
	}
}
