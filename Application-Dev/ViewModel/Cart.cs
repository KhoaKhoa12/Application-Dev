using Application_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Application_Dev.ViewModel
{
	public class Cart
	{
		[BindProperty]
		public List<OrderDetail> orderDetails { get; set; }
		public int totalPrice { get; set; }
		
	}
}
