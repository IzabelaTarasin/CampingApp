using System;
namespace CampingApp_Server.Database
{
	public class Status
	{
		public int Id { get; set; }
		public static int StatusActive = 1;
		public static int StatusCancel = 2;
		public static int StatusDone = 3;
		public string StatusName { get; set; }
	}
}

