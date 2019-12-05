using MerchantApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Services
{
    public interface ITransactionRepository
    {
        ICollection<Transaction> GetTransactions();
        Transaction GetTransaction(int transactionId);
        ICollection<Transaction> GetTransactionsOfAnUser(int userId);
        User GetUserOfATransaction(int transactionId);
        ICollection<Transaction> GetTransactionsOfAnMerchant(int merchantId);
        Merchant GetMerchantOfATransaction(int transactionId);
        bool TransactionExist(int transactionId);
        bool CreateTransaction(Transaction transaction);
        bool UpdateTransaction(Transaction transaction);
        bool DeleteTransaction(Transaction transaction);
        bool Save();
    }
}
