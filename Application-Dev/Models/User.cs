using Microsoft.AspNetCore.Identity;

namespace Application_Dev.Models
{
	public class User : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string? Address { set; get; }
		List<Order>? Orders { get; set; }
	}
}
