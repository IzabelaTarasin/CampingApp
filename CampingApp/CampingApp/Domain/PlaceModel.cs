using System;
namespace CampingApp.Domain
{
	public class PlaceModel
	{
		public int Id { get; set; }
		public UserModel UserModel { get; set; }
		public AddressModel AddressModel { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public double PricePerDay { get; set; }
	}
}

