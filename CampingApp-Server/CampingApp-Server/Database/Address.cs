using System;
namespace CampingApp_Server.Database
{
	public class Address
	{
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string HouseNumber { get; set; }
        public string LocalNumber { get; set; }
        public string Street { get; set; }
        public string Voivodeship { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            var address = "";

            if(!string.IsNullOrWhiteSpace(Street))
            {
                address += $"ul. {Street}";
            }

            if (!string.IsNullOrWhiteSpace(HouseNumber))
            {
                address += $" {HouseNumber}";
            }

            if (!string.IsNullOrWhiteSpace(LocalNumber))
            {
                address += $" / {LocalNumber}\n";
            }

            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                address += $"{PostalCode}";
            }

            if (!string.IsNullOrWhiteSpace(City))
            {
                address += $" {City}\n";
            }

            if (!string.IsNullOrWhiteSpace(Voivodeship))
            {
                address += $"województwo: {Voivodeship}\n";
            }

            if (!string.IsNullOrWhiteSpace(Country))
            {
                address += $"kraj: {Country}\n";
            }

            return address;
        }
    }
}

