using System;
namespace CampingApp_Server.Database
{
	public class Comment
	{
        public int Id { get; set; }
        public Place Place { get; set; }
        public string StaffOpinion { get; set; }
        public string ComfortOpinion { get; set; }
        public string FacilitiesOpinion { get; set; }
        public string CleanlinessOpinion { get; set; }
        public string LocationOpinion { get; set; }
        public string KitchenOpinion { get; set; }
    }
}

