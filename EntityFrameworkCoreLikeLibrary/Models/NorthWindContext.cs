﻿using System;
using EntityFrameworkCoreLikeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace EntityFrameworkCoreLikeLibrary.Models
{
    public partial class NorthWindContext : DbContext
    {
        // verbose
        //public static readonly ILoggerFactory LoggerFactory = new LoggerFactory(new[] {
        //    new ConsoleLoggerProvider((_, __) => true, true)
        //});


        public static readonly ILoggerFactory consoleLoggerSqlOnlyFactory
            = new LoggerFactory(new[] 
            {
                new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name && 
                                                               level == LogLevel.Information, true)
            });

        public NorthWindContext()
        {
        }

        public NorthWindContext(DbContextOptions<NorthWindContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<ContactContactDevices> ContactContactDevices { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<PhoneType> PhoneType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var connectionString = "Server=KARENS-PC;Database=NorthWindAzure2;Trusted_Connection=True;";

#if (EFC_LOG_ENABLED)
                optionsBuilder.UseLoggerFactory(consoleLoggerSqlOnlyFactory)
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(connectionString);
#else
        optionsBuilder.UseSqlServer(connectionString);
#endif
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.InUse).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ContactContactDevices>(entity =>
            {
                entity.HasOne(d => d.ContactIdentifierNavigation)
                    .WithMany(p => p.ContactContactDevices)
                    .HasForeignKey(d => d.ContactIdentifier)
                    .HasConstraintName("FK_ContactContactDevices_Contact");

                entity.HasOne(d => d.PhoneTypeIdenitfierNavigation)
                    .WithMany(p => p.ContactContactDevices)
                    .HasForeignKey(d => d.PhoneTypeIdenitfier)
                    .HasConstraintName("FK_ContactContactDevices_PhoneType");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerIdentifier)
                    .HasName("PK_Customers_1");

                entity.Property(e => e.InUse).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ContactIdentifierNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.ContactIdentifier)
                    .HasConstraintName("FK_Customers_Contact");

                entity.HasOne(d => d.ContactTypeIdentifierNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.ContactTypeIdentifier)
                    .HasConstraintName("FK_Customers_ContactType");

                entity.HasOne(d => d.CountryIdentfierNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CountryIdentfier)
                    .HasConstraintName("FK_Customers_Countries");
            });
        }
    }
}
