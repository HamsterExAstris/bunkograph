﻿// <auto-generated />
using System;
using Bunkograph.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    [DbContext(typeof(BunkographContext))]
    [Migration("20220712011618_CreateLicenseTable")]
    partial class CreateLicenseTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Bunkograph.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("EnglishKey")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Bunkograph.Models.BookEdition", b =>
                {
                    b.Property<int>("BookEditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<int>("SeriesLicenseId")
                        .HasColumnType("int");

                    b.HasKey("BookEditionId");

                    b.HasIndex("BookId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SeriesLicenseId");

                    b.ToTable("BookEditions");
                });

            modelBuilder.Entity("Bunkograph.Models.Language", b =>
                {
                    b.Property<string>("LanguageId")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Bunkograph.Models.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentPublisherPublisherId")
                        .HasColumnType("int");

                    b.HasKey("PublisherId");

                    b.HasIndex("ParentPublisherPublisherId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("Bunkograph.Models.Series", b =>
                {
                    b.Property<int>("SeriesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EnglishKey")
                        .HasColumnType("longtext");

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
                        .HasPrecision(6, 2)
                        .HasColumnType("decimal(6,2)");

                    b.HasKey("SeriesId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("SeriesBooks");
                });

            modelBuilder.Entity("Bunkograph.Models.SeriesLicense", b =>
                {
                    b.Property<int>("SeriesLicenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CompletionStatus")
                        .HasColumnType("longtext");

                    b.Property<string>("LanguageId")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.HasKey("SeriesLicenseId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SeriesId");

                    b.ToTable("SeriesLicenses");
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

            modelBuilder.Entity("Bunkograph.Models.BookEdition", b =>
                {
                    b.HasOne("Bunkograph.Models.Book", "Book")
                        .WithMany("Editions")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.SeriesLicense", "SeriesLicense")
                        .WithMany()
                        .HasForeignKey("SeriesLicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Publisher");

                    b.Navigation("SeriesLicense");
                });

            modelBuilder.Entity("Bunkograph.Models.Publisher", b =>
                {
                    b.HasOne("Bunkograph.Models.Publisher", "ParentPublisher")
                        .WithMany()
                        .HasForeignKey("ParentPublisherPublisherId");

                    b.Navigation("ParentPublisher");
                });

            modelBuilder.Entity("Bunkograph.Models.SeriesBook", b =>
                {
                    b.HasOne("Bunkograph.Models.Book", "Book")
                        .WithMany("SeriesBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.Series", "Series")
                        .WithMany("SeriesBooks")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Bunkograph.Models.SeriesLicense", b =>
                {
                    b.HasOne("Bunkograph.Models.Language", "Lanuage")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bunkograph.Models.Series", "Series")
                        .WithMany("SeriesLicenses")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lanuage");

                    b.Navigation("Publisher");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Bunkograph.Models.Book", b =>
                {
                    b.Navigation("Editions");

                    b.Navigation("SeriesBooks");
                });

            modelBuilder.Entity("Bunkograph.Models.Series", b =>
                {
                    b.Navigation("SeriesBooks");

                    b.Navigation("SeriesLicenses");
                });
#pragma warning restore 612, 618
        }
    }
}
