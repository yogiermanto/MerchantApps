using System.Collections.Generic;
using System.Linq;
using MerchantApi.Models;

namespace MerchantApi.Services
{
    public class UserRepository : IUserRepository
    {
        private MerchantDbContext _userDbContext;

        public UserRepository(MerchantDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public bool CreateUser(User user)
        {
            _userDbContext.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _userDbContext.Remove(user);
            return Save();
        }

        public ICollection<Transaction> GetTransactionsByUser(int userId)
        {
            return _userDbContext.Transactions.Where(t => t.User.Id == userId).ToList();
        }

        public User GetUser(int userId)
        {
           return _userDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public User GetUserOfATransaction(int transactionId)
        {
            var userId = _userDbContext.Transactions.Where(t => t.Id == transactionId).Select(u => u.User.Id).FirstOrDefault();
            return _userDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {            
            return _userDbContext.Users.OrderBy(u => u.Id).ToList();
        }

        public bool Save()
        {
            var saved = _userDbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _userDbContext.Update(user);
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _userDbContext.Users.Any(u => u.Id == userId);
        }
    }
}
