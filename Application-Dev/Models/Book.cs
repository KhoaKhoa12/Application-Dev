using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Application_Dev.Models
{
	public class Book
	{
		[Key]
		public int Id { get; set; }

		public string NameBook { get; set; } = string.Empty;
		[Required(ErrorMessage = "Quantity can not be null")]
		public int QuantityBook { get; set; }
		[Required(ErrorMessage = "Price can not be null")]
		public int Price { get; set; }
		public string InformationBook { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public byte[]? Image { get; set; } 

		[Required]
		[ForeignKey("Category")]
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
