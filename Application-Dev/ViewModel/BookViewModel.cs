using Application_Dev.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Application_Dev.ViewModel
{
	public class BookViewModel
	{
		public Book? Book { get; set; }
		public IEnumerable<Category>? Categories { get; set; }

		[Display(Name = "File")]
		public IFormFile? FormFile { get; set; }
	}
}
