using Probotbor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.ViewModels
{
    class ApplicationContext: DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<HistoryItem> HistoryItems { get; set; }
    }
}
