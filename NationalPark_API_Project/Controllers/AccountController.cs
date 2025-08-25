using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API_Project.Models;
using NationalPark_API_Project.Repository.IRepository;

namespace NationalPark_API_Project.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public IActionResult Register ([FromBody] User user)                      //FromBody is used ---- DataSubmit HokrAye to
        {
            if (ModelState.IsValid)
            {
                var isUniqueUser = _userRepository.IsUniqueUser(user.UserName);
                if (!isUniqueUser) return BadRequest("User in Use!!!!");
                var UserInfo = _userRepository.Register(user.UserName, user.Password);
                if (UserInfo == null) return NotFound();
                user = UserInfo;
            }
            return Ok(user);
        }
        [HttpPost("login")] 
        public IActionResult Login ([FromBody] User user)
        {
            var UserFromDb =_userRepository.Authenticate(user.UserName,user.Password);
            if (UserFromDb == null) return BadRequest("Wrong User/PWD Enter");
            return Ok(UserFromDb);
        }
    }
}
