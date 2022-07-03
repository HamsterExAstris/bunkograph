﻿using Bunkograph.Models;

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
        public DbSet<SeriesBook> SeriesBooks => Set<SeriesBook>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookEdition> BookEditions => Set<BookEdition>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SeriesBook>()
                .HasKey(k => new { k.SeriesId, k.BookId });
        }
    }
}