using Application_Dev.Data;
using Application_Dev.Models;
using Application_Dev.Utils;
using Application_Dev.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Application_Dev.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, 
                                UserManager<User> userManager, 
                                RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {

            if(name == "AllUser" || name == null)
            {
             var model = new AdminViewModel();

            foreach (var user in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(user, Role.ADMIN))
                {
                    model.Users.Add(user);
                }
            }

            return View(model);
            }

            else
            {
                var model = new AdminViewModel();

                foreach (var user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, name))
                    {
                        model.Users.Add(user);
                    }
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            var getUser = _context.Users.SingleOrDefault(t => t.Id == id);
            if (getUser == null || getUser.EmailConfirmed == false)
            {
                TempData["Message"] = "Can not update Because Email not confirmed";
                
                return View(getUser);
            }

            return View(getUser);
        }

        [HttpPost]
        public IActionResult ChangePassword(string id, [Bind("PasswordHash")] User user)
        {
            var getUser = _context.Users.SingleOrDefault(t => t.Id == id);
            var newPassword = user.PasswordHash; 

            if (getUser == null && getUser.EmailConfirmed == false)
            {
                return BadRequest();
            }
            if(newPassword == null)
            {
                ModelState.AddModelError("NoInput", "You have not input new password.");
                return View(getUser);
            }
          
            getUser.PasswordHash = _userManager.PasswordHasher.HashPassword(getUser, newPassword);
            TempData["Message"] = "Update Successfully";
            


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
