using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Dto.Users;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        void Register(AddUserDto addUserDto);
        string LogIn(LogInDto logInDto);
    }
}
