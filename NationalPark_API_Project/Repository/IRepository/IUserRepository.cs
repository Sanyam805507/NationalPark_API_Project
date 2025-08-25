using NationalPark_API_Project.Models;

namespace NationalPark_API_Project.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser (string username);
        User Authenticate (string username,string Password);
        User Register (string username,string password);
       
    }

}

