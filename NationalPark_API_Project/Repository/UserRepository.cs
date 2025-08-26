using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NationalPark_API_Project.DATA;
using NationalPark_API_Project.Models;
using NationalPark_API_Project.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NationalPark_API_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appsettings;  
        public UserRepository(ApplicationDbContext context,IOptions<AppSettings> appsettings)
        {
            _context = context;
            _appsettings = appsettings.Value;
        }
        public User Authenticate(string username, string Password)
        {
            var UserInDb = _context.Users.FirstOrDefault(u => u.UserName == username && u .Password == Password);
            if (UserInDb == null)  return null;

            //JWT Token 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,UserInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role,UserInDb.Role)    
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token =tokenHandler.CreateToken(tokenDescriptor);
            UserInDb.Token =tokenHandler.WriteToken(token); 
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
