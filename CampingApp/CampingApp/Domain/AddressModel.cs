﻿using System;
namespace CampingApp.Domain
{
	public class AddressModel
	{
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string HouseNumber { get; set; }
        public string LocalNumber { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
    }
}

