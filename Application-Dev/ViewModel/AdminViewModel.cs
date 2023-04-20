using Application_Dev.Models;
using Application_Dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Application_Dev.ViewModel
{
    public class AdminViewModel
    {
        public List<User> Users { set; get; } = new List<User>();
        public List<Category> Categories { set; get; } = new List<Category>();
        public List<Category> CategoriesHidden { set; get; } = new List<Category>();

        [BindProperty]
        public InputModel Input { get; set; }
        public SelectList RoleSelectList { get; set; }
        

        public class InputModel
        {
            
            public string Role { get; set; }


        }

    }
}
