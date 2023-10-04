using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.Data
{
    public static class ConnectionReg
    {
        public static void RegisterVendorDataConnections(this ModelBuilder builder)
        {
            builder.Entity<City>()
                .HasOne(e => e.Country)
                .WithMany(p => p.Cities)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_City_Country");

            builder.Entity<Zip>()
                .HasOne(e => e.Country)
                .WithMany(p => p.ZipCodes)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Zip_Country");

            builder.Entity<Address>()
                .HasOne(e => e.Zip)
                .WithMany(p => p.Addresses)
                .HasForeignKey(x => x.ZipId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Zip_Address");

            builder.Entity<Address>()
                .HasOne(e => e.City)
                .WithMany(p => p.Addresses)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_City_Addresses");

            builder.Entity<Vendor>()
                .HasMany(e => e.Addresses)
                .WithOne(p => p.Vendor)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Vendor_Address");

            builder.Entity<Vendor>()
                .HasMany(x => x.ContactPersons)
                .WithOne(x => x.Vendor)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Vendor_ContactPerson");

            builder.Entity<Vendor>()
                .HasMany(x => x.BankAccounts)
                .WithOne(x => x.Vendor)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Vendor_BankAccount");

            builder.Entity<Email>()
                .HasOne(e => e.Vendor)
                .WithMany(p => p.Emails)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Vendor_Email");

            builder.Entity<Email>()
                .HasOne(e => e.Person)
                .WithMany(p => p.Emails)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Person_Email");

            builder.Entity<Phone>()
                .HasOne(e => e.Vendor)
                .WithMany(p => p.Phones)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Vendor_Phone");

            builder.Entity<Phone>()
                .HasOne(e => e.Person)
                .WithMany(p => p.Phones)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Person_Phone");
        }
    }
}
