Us
namespace Task_4.Controllers.Components
{
    [ViewComponent]
    public class GetUsers
    {
        private readonly ApplicationDbContext _db;

        public GetUsers(ApplicationDbContext db)
        {
            _db = db;
        }

        public UserModel Invoke()
        {
            List<UserModel> users= _db.Users;
            return users;
        }
    }
}
