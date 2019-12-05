using MerchantApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Services
{
    public class MerchantDbContext : DbContext
    {
        public MerchantDbContext(DbContextOptions<MerchantDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }
}
