using Microsoft.EntityFrameworkCore;
using TestMinimalAPI.Data.Models;

namespace TestMinimalAPI.Data.Config;

public partial class PersonDbContext : DbContext
{
    public PersonDbContext() {}

    public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) {}

    public virtual DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.ToTable("person");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });
    }
}
