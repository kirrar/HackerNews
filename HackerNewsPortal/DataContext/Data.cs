using HackerNewsPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace HackerNewsPortal.DataContext
{
    public class Data : DbContext
    {
        public Data(DbContextOptions<Data> options)
            : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }
    }
}
