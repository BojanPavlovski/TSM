using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private TaskManagementSystemDbContext _dbContext;
        public UserRepository(TaskManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public User GetUserByUserNameAndPassword(string userName, string password)
        {
            return _dbContext.Users.SingleOrDefault(x => x.Username == userName && x.Password == password);
        }
    }
}
