using Bunkograph.Models;
using Microsoft.EntityFrameworkCore;

namespace Bunkograph.DAL
{
    public class BunkographContext : DbContext
    {
        public BunkographContext(DbContextOptions<BunkographContext> options)
            : base(options)
        {
        }

        public DbSet<Series> Series => Set<Series>();
        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SeriesBook>()
                .HasKey(k => new { k.SeriesId, k.BookId });
        }
    }
}