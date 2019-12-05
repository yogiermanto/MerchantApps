using MerchantApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Services
{
    public interface IMerchantRepository
    {
        ICollection<Merchant> GetMerchants();
        Merchant GetMerchant(int merchantId);
        ICollection<Transaction> GeTransactionsByMerchant(int merchantId);
        Merchant GetMerchantOfATransaction(int transactionId);
        bool MerchantExists(int merchantId);
        bool CreateMerchant(Merchant merchant);
        bool UpdateMerchant(Merchant merchant);
        bool DeleteMerchant(Merchant merchant);
        bool Save();
    }
}
