using Application_Dev.Data;
using Application_Dev.Models;
using Application_Dev.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application_Dev.Controllers
{
	public class BooksController : Controller
	{
			// 1 - Declare ApplicationDbContext
			private ApplicationDbContext _context;
			public BooksController(ApplicationDbContext context)
			{
				_context = context;
			}

			// 2 - Search Book by Name
			[HttpGet]
			public async Task<IActionResult> Index(string searchString)
			{
				var books = from book in _context.Books select book;
				if (!String.IsNullOrEmpty(searchString))
				{
					books = books.Where(s => s.NameBook!.Contains(searchString));
				}
				return View(await books.ToListAsync());
			}

			// 3 - Create Book Data
			[HttpGet]
			public IActionResult Create()
			{
				var viewModel = new BookViewModel()
				{
					Categories = _context.Categories
						.Where(c => c.Status == Enums.CategoryStatus.Accepted)
						.ToList()
				};
				return View(viewModel);
			}

			[HttpPost]
			public async Task<IActionResult> Create(BookViewModel viewModel)
			{
				if (!ModelState.IsValid)
				{
					viewModel = new BookViewModel
					{
						Categories = _context.Categories
							.Where(c => c.Status == Enums.CategoryStatus.Accepted)
							.ToList()
					};
					return View(viewModel);
				}

				using (var memoryStream = new MemoryStream())
				{
					await viewModel.FormFile.CopyToAsync(memoryStream);

					var newBook = new Book
					{
						NameBook = viewModel.Book.NameBook,
						QuantityBook = viewModel.Book.QuantityBook,
						Price = viewModel.Book.Price,
						InformationBook = viewModel.Book.InformationBook,
						Image = memoryStream.ToArray(),
						CategoryId = viewModel.Book.CategoryId
					};
					_context.Books.Add(newBook);
					await _context.SaveChangesAsync();
				}
				return RedirectToAction("Index");
			}


			// 4 - Delete Book Data
			[HttpGet]
			public IActionResult Delete(int id)
			{
				var bookInDb = _context.Books.SingleOrDefault(t => t.Id == id);
				if (bookInDb is null)
				{
					return NotFound();
				}
				_context.Books.Remove(bookInDb);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}

			// 5 - Edit Book Data
			[HttpGet]
			public IActionResult Edit(int id)
			{
				var bookInDb = _context.Books.SingleOrDefault(t => t.Id == id);
				if (bookInDb is null)
				{
					return NotFound();
				}

				var viewModel = new BookViewModel
				{
					Book = bookInDb,
					Categories = _context.Categories
						.Where(c => c.Status == Enums.CategoryStatus.Accepted)
						.ToList()
				};
				return View(viewModel);
			}

			[HttpPost]
			public IActionResult Edit(BookViewModel viewModel)
			{
				var bookInDb = _context.Books.SingleOrDefault(t => t.Id == viewModel.Book.Id);
				if (bookInDb is null)
				{
					return BadRequest();
				}

				if (!ModelState.IsValid)
				{
					viewModel = new BookViewModel
					{
						Book = viewModel.Book,
						Categories = _context.Categories
						 .Where(c => c.Status == Enums.CategoryStatus.Accepted)
						 .ToList()
					};
					return View(viewModel);
				}


				bookInDb.NameBook = viewModel.Book.NameBook;
				bookInDb.QuantityBook = viewModel.Book.QuantityBook;
				bookInDb.Price = viewModel.Book.Price;
				bookInDb.InformationBook = viewModel.Book.InformationBook;
				bookInDb.CategoryId = viewModel.Book.CategoryId;

				_context.SaveChanges();

				return RedirectToAction("Index");
			}

			// 6 - View Book Details
			[HttpGet]
			public IActionResult Details(int id)
			{
				var bookInDb = _context.Books
						.Include(t => t.Category)
						.SingleOrDefault(t => t.Id == id);
				if (bookInDb is null)
				{
					return NotFound();
				}
				string imageBase64Data = Convert.ToBase64String(bookInDb.Image);
				string image = string.Format("data:image/jpg;base64, {0}", imageBase64Data);
				ViewBag.Image = image;

				return View(bookInDb);
			}


		
	}
}
