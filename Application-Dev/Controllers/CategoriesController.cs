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


        // 2 - Create Category Data
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


    }
}
