using Application_Dev.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application_Dev.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Category can not be null")]
		[StringLength(255)]
		public string NameCategory { get; set; } = string.Empty;
		public CategoryStatus Status { get; set; } = CategoryStatus.InProgess;
		public List<Book>? Books { get; set; }
	}
}