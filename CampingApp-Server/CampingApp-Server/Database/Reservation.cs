using System;
namespace CampingApp_Server.Database
{
	public class Reservation
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; } //gdy jest Place to on wie czym jest to place i tego rzada, jak da PLaceId ti on ogolnie widzi pole ale nie zrada go do przekazania bo nie wie czym dokladnie to pole jest -  moglo by byc test test i by przeszlo
        public string UserId { get; set; } //normanie id ale uzywany usera z biblioteki security wiec bedzie jako string
        public User User { get; set; } //nie uerId
        public int StatusId { get; set; } 
        public Status Status { get; set; }//nie status id bo nie musimy dzieki temy robic dodatkowego zpaytaania do bazy danych
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

