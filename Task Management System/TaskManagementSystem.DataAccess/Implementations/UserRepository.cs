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
        //a method that adds a user in the database
        public void AddUser(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }
        //a method that returns a user, based on given username and password
        public User GetUserByUserNameAndPassword(string userName, string password)
        {
            return _dbContext.Users.SingleOrDefault(x => x.Username == userName && x.Password == password);
        }
    }
}
