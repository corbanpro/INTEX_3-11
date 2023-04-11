using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX_3_11.Models
{
    public class NewEntryContext : DbContext
    {
        public NewEntryContext(DbContextOptions<NewEntryContext> options) : base(options)
        {

        }

        public DbSet<NewEntry> Responses { get; set; }
        protected override void OnModelCreating(ModelBuilder mb)
        {
        }

    }
}
