using Microsoft.AspNetCore.Mvc;
using Task_4.Data;
using Task_4.Models;

namespace Task_4.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetAllUsers()
        {
            List<UserModel> users = _db.Users.ToList();
            return View(users);
        }
    }
}
