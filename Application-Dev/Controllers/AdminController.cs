using Application_Dev.Data;
using Application_Dev.Models;
using Application_Dev.Utils;
using Application_Dev.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using static Application_Dev.Areas.Identity.Pages.Account.LoginModel;



namespace Application_Dev.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SelectList RoleSelectList { get; set; } = new SelectList(new List<string>
          {
            "All User",
            Role.STORE_OWNER,
            Role.CUSTOMER
          }
        ); 
        public AdminController(ApplicationDbContext context, 
                                UserManager<User> userManager, 
                                RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<Category> CategoriesHidden { set; get; } = new List<Category>();



        [HttpGet]
        public async Task<IActionResult> Index()
        {


            var model = new AdminViewModel();

            foreach (var user in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(user, Role.ADMIN))
                {
                    model.Users.Add(user);
                }
            }

            model.RoleSelectList = RoleSelectList; 


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(AdminViewModel adminViewModel)
        {
            var adminUser = new AdminViewModel();
         
            var roleSelectedInView = adminViewModel.Input.Role; 

            if(roleSelectedInView == Role.STORE_OWNER)
            {

                adminUser = getUserByRole(Role.STORE_OWNER); 
            }
            else if(roleSelectedInView == Role.CUSTOMER)
            {
                adminUser = getUserByRole(Role.CUSTOMER);
            }
            else
            {
                adminUser = new AdminViewModel();

                foreach (var user in _userManager.Users)
                {
                    if (!await _userManager.IsInRoleAsync(user, Role.ADMIN))
                    {
                        adminUser.Users.Add(user);
                    }
                }
            }

            adminUser.RoleSelectList = RoleSelectList;
            return View(adminUser);
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
        public IActionResult ShowCategoriesInProgress()
        {
            var categories = _context.Categories
                .Where(t => t.Status == Enums.CategoryStatus.InProgess)
                .ToList();

            return View("VerifyCategoryRequest", categories);
        }

        [HttpGet]
        public IActionResult VerifyCategoryRequest(string name, int id)
        {

            var listCategory = from Category in _context.Categories select Category;           
            var categoryAfterUpdate = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (name == "accept")
            {
                AcceptCategoryRequest(id);

            }
            if (name == "reject")

            {
                RejectCategoryRequest(id);
            }

            return RedirectToAction(nameof(ShowCategoriesInProgress));
        }



        [HttpGet]
        public IActionResult AcceptCategoryRequest(int id)
        {
            var categoryVerify = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryVerify == null)
            {
                return BadRequest();
            }

            categoryVerify.Status = Enums.CategoryStatus.Accepted;
            _context.SaveChanges();
               
            return RedirectToAction("VerifyCategoryRequest");
        } 

        [HttpGet]
        public IActionResult RejectCategoryRequest(int id)
        {
            var categoryVerify = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryVerify == null)
            {
                return BadRequest();
            }

            categoryVerify.Status = Enums.CategoryStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("VerifyCategoryRequest");
        }

       


        [NonAction]
        private AdminViewModel getUserByRole(string role)
        {
            var adminUser = new AdminViewModel()
            {
                Users = (List<User>)_userManager.GetUsersInRoleAsync(role).Result
            };
            return adminUser;
        }



    }
}
