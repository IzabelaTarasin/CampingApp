﻿using System;
namespace CampingApp.Domain
{
	public class ReservationModel //nie dadjemy user Id bo nie bedzie wyswietlana ataka ainformacja na froncie
    {
        public int Id { get; set; }
        public PlaceModel PlaceModel { get; set; }
        public StatusModel StatusModel { get; set; } //nie status id bo nie musimy dzieki temy robic dodatkowego zpaytaania do bazy danych
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

