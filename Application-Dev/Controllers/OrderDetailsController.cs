using Application_Dev.Data;
using Application_Dev.Models;
using Application_Dev.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Application_Dev.Controllers
{
	[Authorize]
	public class OrderDetailsController : Controller
	{
		private ApplicationDbContext context;
		private readonly UserManager<User> userManager;

		public OrderDetailsController(ApplicationDbContext context, UserManager<User> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index(int id)
		{
			Cart cart = new Cart();		
			int result = 0;

			if (id != 0)
			{
				cart.orderDetails = context.OrderDetails.Include(t => t.Order)
					.Include(t => t.Book).Where(t => t.OrderId == id).ToList();
				foreach (var item in cart.orderDetails)
				{
					result += item.Price;
				}
				cart.totalPrice = result;

				return View(cart);
			}
			cart.orderDetails = context.OrderDetails.Include(t => t.Book).Include(t => t.Order)
				.Where(t => t.Order.StatusOrder == Enums.OrderStatus.Unconfirmed 
				&& t.Order.UserId == userManager.GetUserId(User)).ToList();
			foreach(var item in cart.orderDetails)
			{
				result += item.Price;
			}
			cart.totalPrice =  result;

			return View(cart);
		}

		[HttpGet]
		public IActionResult Delete (int id)
		{
			var orderDetail = context.OrderDetails.Include(t => t.Order).SingleOrDefault(t => t.Id == id);
			orderDetail.Order.PriceOrder = 0;
			context.Remove(orderDetail);
			List<OrderDetail> orderDetails = context.OrderDetails.Include(t => t.Order).Where(t => t.OrderId == orderDetail.OrderId).ToList();

			foreach (var item in orderDetails)
			{			
				item.Order.PriceOrder += item.Price;
			}
			context.SaveChanges();

			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult Edit (Cart cart)
		{
			foreach (var item in cart.orderDetails)
			{
				var a = context.OrderDetails.SingleOrDefault(t => t.Id == item.Id);
			
					a.Quantity = item.Quantity;

	
			}
			context.SaveChanges();
			return RedirectToAction("Checkout", "Orders");
		}
	}
}
