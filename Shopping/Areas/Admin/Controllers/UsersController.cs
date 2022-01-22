using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;
using Shopping.Models;

namespace Shopping.Areas.Admin.Controllers
{
  
        [Authorize(Roles = "admin")]
        [Area("Admin")]
        public class UsersController : Controller
        {
            private readonly UserManager<AppUser> userManager;
            private readonly ShoppingContext context;

        public UsersController(ShoppingContext context, UserManager<AppUser> userManager)
            {
                this.userManager = userManager;
                this.context = context;
        }

        /*public async Task<List<User>> GetUsersAsync()
        {
            var user = context.Users;
           /// return await User.FindAll();
           
        }
        */
        public IActionResult Index()
            {
            var users = userManager.Users;
            return View(users.ToList());
            }
        
        }
}
