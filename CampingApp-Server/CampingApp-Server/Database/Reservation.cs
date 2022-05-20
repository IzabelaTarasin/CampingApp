using System;
namespace CampingApp_Server.Database
{
	public class Reservation
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public string UserId { get; set; } //normanie id ale uzywany usera z biblioteki security wiec bedzie jako string
        public User User { get; set; } //nie uerId
        public Status Status { get; set; } //nie status id bo nie musimy dzieki temy robic dodatkowego zpaytaania do bazy danych
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}

