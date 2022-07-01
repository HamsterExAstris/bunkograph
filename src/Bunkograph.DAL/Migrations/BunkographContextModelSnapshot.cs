﻿// <auto-generated />
using Bunkograph.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    [DbContext(typeof(BunkographContext))]
    partial class BunkographContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Bunkograph.Models.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("JapaneseName")
                        .HasColumnType("longtext");

                    b.Property<string>("RomanizedName")
                        .HasColumnType("longtext");

                    b.HasKey("AuthorId");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Bunkograph.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Bunkograph.Models.Series", b =>
                {
                    b.Property<int>("SeriesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SeriesId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("Bunkograph.Models.SeriesBook", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayIndex")
                        .HasColumnType("longtext");

                    b.Property<decimal>("SortOrder")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("SeriesId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("SeriesBook");
                });

            modelBuilder.Entity("Bunkograph.Models.Book", b =>
                {
                    b.HasOne("Bunkograph.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Bunkograph.Models.SeriesBook", b =>
                {
                    b.HasOne("Bunkograph.Models.Book", "Book")
                        .WithMany("SeriesBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Bunkograph.Models.Book", b =>
                {
                    b.Navigation("SeriesBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
