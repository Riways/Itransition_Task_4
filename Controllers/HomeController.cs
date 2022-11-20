using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task_4.Models;
using Task_4.Services;

namespace Task_4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            List<UserModel> users = _userService.GetUsersInList();
            return View(users);
        }

        [HttpPost]
        public IActionResult BlockUser()
        {
            var idsToBlock = HttpContext.Request.Form["userId"];
            foreach (var id in idsToBlock)
                _userService.BlockUserById(id, HttpContext.User);
            return RedirectPermanent("~/Home/Index");
        }
        
        [HttpPost]
        public IActionResult UnblockUser()
        {
            var idsToUnblock = HttpContext.Request.Form["userId"];
            foreach (var id in idsToUnblock)
                _userService.UnblockUserById(id);
            return RedirectPermanent("~/Home/Index");
        }

        [HttpPost]
        public IActionResult DeleteUser()
        {
            var idsToDelete = HttpContext.Request.Form["userId"];
            foreach (var id in idsToDelete)
                _userService.DeleteUserById(id, HttpContext.User);
            return RedirectPermanent("~/Home/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}