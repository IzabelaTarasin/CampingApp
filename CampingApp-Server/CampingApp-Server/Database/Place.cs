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
		public bool AnimalsAllowed { get; set; }
		public bool RestaurantExist { get; set; }
		public bool ReceptionExist { get; set; }
		public bool MedicExist { get; set; }
		public bool GrillExist { get; set; }
		public bool WifiExist { get; set; }
		public bool SwimmingpoolExist { get; set; }
	}
}

