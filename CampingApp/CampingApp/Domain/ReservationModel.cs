using System;
namespace CampingApp.Domain
{
	public class ReservationModel
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public UserModel UserModel { get; set; } //nie uerId
        public StatusModel StatusModel { get; set; } //nie status id bo nie musimy dzieki temy robic dodatkowego zpaytaania do bazy danych
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool HasAnimals { get; set; }
    }
}

