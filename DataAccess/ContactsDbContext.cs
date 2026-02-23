using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DataAccess
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext()
        {
        }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts => Set<Contact>();

        public DbSet<ContactCustomField> ContactCustomFields => Set<ContactCustomField>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactCustomField>()
                .HasOne(cf => cf.Contact)
                .WithMany(c => c.CustomValues)
                .HasForeignKey(cf => cf.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContactCustomField>()
                .HasIndex(cf => new { cf.ContactId, cf.FieldName })
                .IsUnique();

            modelBuilder.Entity<ContactCustomField>()
                .Property(cf => cf.DataType)
                .IsRequired();

            modelBuilder.Entity<ContactCustomField>()
                .Property(cf => cf.Value)
                .IsRequired();
        }
    }
}
