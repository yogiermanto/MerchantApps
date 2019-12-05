using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
