using Microsoft.EntityFrameworkCore;
using PickapicBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Data
{
    public class DataContext: DbContext
    {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
