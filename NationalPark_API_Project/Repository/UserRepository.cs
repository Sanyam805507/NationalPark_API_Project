using NationalPark_API_Project.DATA;
using NationalPark_API_Project.Models;
using NationalPark_API_Project.Repository.IRepository;

namespace NationalPark_API_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User Authenticate(string username, string Password)
        {
            var UserInDb = _context.Users.FirstOrDefault(u => u.UserName == username && u .Password == Password);
            if (UserInDb == null)  return null;
            //JWT Token 

            //*****
            UserInDb.Password = "";
            return UserInDb;    
        }

        public bool IsUniqueUser(string username)
        {
            var UserInDb =_context.Users.FirstOrDefault(u=>u.UserName == username);
            if (UserInDb == null) return true; return false;
        }

        public User Register(string username, string password)
        {
            User user = new User()
            {
                UserName = username,
                Password = password ,
                Role ="Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }

}
