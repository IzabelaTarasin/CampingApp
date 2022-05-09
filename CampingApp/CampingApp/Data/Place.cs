using System;
namespace CampingApp.Data
{
	public class Place
	{
		public int id { get; set; }
        public User user { get; set; }
        public Address address { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string imagePath { get; set; }

	}
}

