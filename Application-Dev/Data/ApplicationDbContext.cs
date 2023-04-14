using Application_Dev.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application_Dev.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}
		public DbSet<Book>? Books { get; set; }
		public DbSet<Category>? Categories { set; get; }
		public DbSet<Order>? Orders { set; get; }
		public DbSet<OrderDetail>? OrderDetails { set; get; }
	}
}