using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUserNameAndPassword(string userName, string password);
        void AddUser(User user);
    }
}
