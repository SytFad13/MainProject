using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonManagement.MVC.ViewModels
{
    public class AcademyEventViewModel
    {
		public int Id;
		public string Name { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Date")]
		public DateTime DateAndTime { get; set; }
		public string Address { get; set; }
		[DisplayName("Created at")]
		public DateTime CreatedAt { get; set; }
		public string ImgUrl { get; set; }
		[DisplayName("Author username")]
		public string AuthorUsername { get; set; }
	}
}
