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

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Series> Series => Set<Series>();
        public DbSet<SeriesBook> SeriesBooks => Set<SeriesBook>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookEdition> BookEditions => Set<BookEdition>();
        public DbSet<Publisher> Publishers => Set<Publisher>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Series>()
                .Property(p => p.CompletionStatus)
                .HasConversion<string>();

            modelBuilder.Entity<SeriesBook>()
                .HasKey(k => new { k.SeriesId, k.BookId });
        }
    }
}