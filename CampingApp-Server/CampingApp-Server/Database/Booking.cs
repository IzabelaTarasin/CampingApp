using System;
namespace CampingApp_Server.Database
{
	public class Booking
	{
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public User User { get; set; } //nie uerId
        public Status Status { get; set; } //nie status id bo nie musimy dzieki temy robic dodatkowego zpaytaania do bazy danych
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool HasAnimals { get; set; }
    }
}

