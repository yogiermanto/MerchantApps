using MerchantApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Services
{
    public interface IUserRepository
    {        
        ICollection<User> GetUsers();
        User GetUser(int userId);
        ICollection<Transaction> GetTransactionsByUser(int userId);
        User GetUserOfATransaction(int transactionId);
        bool UserExists(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
