using Application_Dev.Data;
using Application_Dev.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application_Dev.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.ToList();
            return View(categories);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            var newCategory = new Category
            {
                NameCategory = category.NameCategory,
                Status = Enums.CategoryStatus.InProgess
            };

            _context.Add(newCategory);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult ManageCategory()
        {
            var categories = _context.Categories
                .Where(t => t.Status == Enums.CategoryStatus.InProgess)
                .ToList();

            return View("ManageCategory", categories);
        }

        [HttpGet]
        public IActionResult AcceptCategory(int id)
        {
            var categoryVerify = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryVerify == null)
            {
                return BadRequest();
            }

            categoryVerify.Status = Enums.CategoryStatus.Accepted;
            _context.SaveChanges();

            return RedirectToAction("ManageCategory");
        }

        [HttpGet]
        public IActionResult RejectCategory(int id)
        {
            var categoryVerify = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryVerify == null)
            {
                return BadRequest();
            }

            categoryVerify.Status = Enums.CategoryStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("ManageCategory");
        }
    }
}
