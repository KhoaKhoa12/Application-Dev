using Application_Dev.Data;
using Application_Dev.Models;
using Application_Dev.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application_Dev.Controllers
{
	public class BooksController : Controller
	{
			private ApplicationDbContext _context;
			public BooksController(ApplicationDbContext context)
			{
				_context = context;
			}

			[HttpGet]
			public async Task<IActionResult> Index(string searchString)
			{
				var books = await _context.Books.ToListAsync();
				if (!String.IsNullOrEmpty(searchString))
				{
					var searchBooks = await _context.Books.Where(s => s.NameBook!.Contains(searchString)).ToListAsync();
					return View(searchBooks);

				}
				return View(books);
			}

			[HttpGet]
			public async Task<IActionResult> Create()
			{
				var viewModel = new BookViewModel()
				{
					Categories = await _context.Categories
						.Where(c => c.Status == Enums.CategoryStatus.Accepted)
						.ToListAsync()
				};
				return View(viewModel);
			}

			[HttpPost]
			public async Task<IActionResult> Create(BookViewModel viewModel)
			{

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


			[HttpGet]
			public async Task<IActionResult> Delete(int id)
			{
				var bookInDb = await _context.Books.SingleOrDefaultAsync(t => t.Id == id);
				if (bookInDb is null)
				{
					return NotFound();
				}
				_context.Books.Remove(bookInDb);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}

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
