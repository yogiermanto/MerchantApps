using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantApi.Models;

namespace MerchantApi.Services
{
    public class MerchantRepository : IMerchantRepository
    {
        private MerchantDbContext _merchantDbContext;

        public MerchantRepository(MerchantDbContext merchantDbContext)
        {
            _merchantDbContext = merchantDbContext;
        }

        public bool CreateMerchant(Merchant merchant)
        {
            _merchantDbContext.Add(merchant);
            return Save();
        }

        public bool DeleteMerchant(Merchant merchant)
        {
            _merchantDbContext.Remove(merchant);
            return Save();
        }

        public Merchant GetMerchant(int merchantId)
        {
            return _merchantDbContext.Merchants.Where(m => m.Id == merchantId).FirstOrDefault();
        }

        public Merchant GetMerchantOfATransaction(int transactionId)
        {
            var merchantId = _merchantDbContext.Transactions.Where(t => t.Id == transactionId).Select(u => u.Merchant.Id).FirstOrDefault();
            return _merchantDbContext.Merchants.Where(m => m.Id == merchantId).FirstOrDefault();
        }

        public ICollection<Merchant> GetMerchants()
        {
            return _merchantDbContext.Merchants.OrderBy(m => m.Id).ToList();
        }

        public ICollection<Transaction> GeTransactionsByMerchant(int merchantId)
        {
            return _merchantDbContext.Transactions.Where(t => t.Merchant.Id == merchantId).ToList();
        }

        public bool MerchantExists(int merchantId)
        {
            return _merchantDbContext.Merchants.Any(m => m.Id == merchantId);
        }

        public bool Save()
        {
            var saved = _merchantDbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateMerchant(Merchant merchant)
        {
            _merchantDbContext.Update(merchant);
            return Save();
        }
    }
}
