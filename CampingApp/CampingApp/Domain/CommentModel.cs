using System;
namespace CampingApp.Domain
{
	public class CommentModel
	{
        public int Id { get; set; }
        public PlaceModel PlaceModel { get; set; }
        public string StaffOpinion { get; set; }
        public string ComfortOpinion { get; set; }
        public string FacilitiesOpinion { get; set; }
        public string CleanlinessOpinion { get; set; }
        public string LocationOpinion { get; set; }
        public string KitchenOpinion { get; set; }
    }
}

