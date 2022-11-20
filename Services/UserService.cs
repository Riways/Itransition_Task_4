using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Task_4.Data;
using Task_4.Models;

namespace Task_4.Services
{
    public interface IUserService{
        public List<UserModel> GetUsersInList();
        public void BlockUserById(string id, ClaimsPrincipal userPrincipal);
        public void UnblockUserById(string id);
        public void DeleteUserById(string id, ClaimsPrincipal userPrincipal);
        public bool IsUserBlocked(string email);
        public void UpdateLastVisitDate(string username, DateTime newVisitDate);
        public void AddClaims(string username, string claimType, string claimValue);
        public void AddClaims(UserModel user, string claimType, string claimValue);
        public void RemoveClaims(UserModel user, string claimType, string claimValue);
        public void LogoutInactiveUserIfHeOnline(string username, ClaimsPrincipal userPrincipal);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public UserService(ILogger<UserService> logger, ApplicationDbContext db, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<UserModel> GetUsersInList()
        {
            return _db.Users.ToList();
        }

        public void BlockUserById(string id, ClaimsPrincipal userPrincipal)
        {
            UserModel userToBlock = _userManager.FindByIdAsync(id).Result;
            userToBlock.isBlocked= true;
            LogoutInactiveUserIfHeOnline(userToBlock.UserName, userPrincipal);
            AddClaims(userToBlock, "isBlocked", "true");
            _db.SaveChanges();
        }

        public void UnblockUserById(string id)
        {
            UserModel userToUnblock = _userManager.FindByIdAsync(id).Result;
            userToUnblock.isBlocked = false;
            RemoveClaims(userToUnblock, "isBlocked", "true");
            _db.SaveChanges();
        }

        public void DeleteUserById(string id, ClaimsPrincipal userPrincipal)
        {   
            UserModel userToDelete =  _userManager.FindByIdAsync(id).Result;
            LogoutInactiveUserIfHeOnline(userToDelete.UserName, userPrincipal);
            _userManager.DeleteAsync(userToDelete);
        }

        public bool IsUserBlocked(string username)
        {
            UserModel userToCheck = _userManager.FindByNameAsync(username).Result;
            if(userToCheck != null)
                return userToCheck.isBlocked;
            return false;
        }

        public void UpdateLastVisitDate(string username, DateTime newVisitDate)
        {
            UserModel user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return ;
            user.LastVisitDate = newVisitDate;
            _db.SaveChanges();
        }

        public void AddClaims(string username, string claimType, string claimValue)
        {
            UserModel user = _userManager.FindByNameAsync(username).Result;
            _userManager.AddClaimAsync(user,new Claim(claimType, claimValue)).Wait();
        }

        public void AddClaims(UserModel user, string claimType, string claimValue)
        {
            _userManager.AddClaimAsync(user, new Claim(claimType, claimValue)).Wait();
        }

        public void RemoveClaims(UserModel user, string claimType, string claimValue)
        {
            _userManager.RemoveClaimAsync(user, new Claim(claimType, claimValue));
        }

        public void LogoutInactiveUserIfHeOnline(string username, ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal.Identity != null && userPrincipal.Identity.IsAuthenticated && username.Equals(userPrincipal.Identity.Name))
            {
                _signInManager.SignOutAsync();
            }
        }
    }
}
