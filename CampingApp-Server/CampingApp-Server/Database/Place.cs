using System;
namespace CampingApp_Server.Database
{
	public class Place
	{
		public int Id { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public double PricePerDay { get; set; }
	}
}

