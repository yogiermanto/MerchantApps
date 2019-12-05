using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantApi.Models;

namespace MerchantApi.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private MerchantDbContext _transactionDbContext;

        public TransactionRepository(MerchantDbContext transactionDbContext)
        {
            _transactionDbContext = transactionDbContext;
        }

        public bool CreateTransaction(Transaction transaction)
        {
            _transactionDbContext.Add(transaction);
            return Save();
        }

        public bool DeleteTransaction(Transaction transaction)
        {
            _transactionDbContext.Remove(transaction);
            return Save();
        }

        public Merchant GetMerchantOfATransaction(int transactionId)
        {
            var merchantId = _transactionDbContext.Transactions.Where(t => t.Id == transactionId).Select(u => u.Merchant.Id).FirstOrDefault();
            return _transactionDbContext.Merchants.Where(m => m.Id == merchantId).FirstOrDefault();
        }

        public Transaction GetTransaction(int transactionId)
        {
            return _transactionDbContext.Transactions.Where(t => t.Id == transactionId).FirstOrDefault();
        }

        public ICollection<Transaction> GetTransactions()
        {
            return _transactionDbContext.Transactions.OrderBy(t => t.Id).ToList();
        }

        public ICollection<Transaction> GetTransactionsOfAnMerchant(int merchantId)
        {
            return _transactionDbContext.Transactions.Where(t => t.Merchant.Id == merchantId).ToList();
        }

        public ICollection<Transaction> GetTransactionsOfAnUser(int userId)
        {
            return _transactionDbContext.Transactions.Where(t => t.User.Id == userId).ToList();
        }

        public User GetUserOfATransaction(int transactionId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _transactionDbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool TransactionExist(int transactionId)
        {
            return _transactionDbContext.Transactions.Any(t => t.Id == transactionId);
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            _transactionDbContext.Update(transaction);
            return Save(); 
        }
    }
}
