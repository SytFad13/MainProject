using System;
using System.ComponentModel.DataAnnotations;

namespace PersonManagement.Domain.POCO
{
	public class AcademyEvent
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateAndTime { get; set; }
		public string Address { get; set; }
		public DateTime CreatedAt { get; set; }
		public int EditDurationInDays { get; set; }
		public bool IsApproved { get; set; }
		public string ImgUrl { get; set; }
		public string AuthorUsername { get; set; }
	}
}
