using Application_Dev.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application_Dev.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public User User { get; set; }
		public DateTime DateOrder { get; set; } = DateTime.Now;
		public int PriceOrder { get; set; }
		public OrderStatus StatusOrder { get; set; }
		public List<OrderDetail>? OrderDetails { get; set; }
	}
}