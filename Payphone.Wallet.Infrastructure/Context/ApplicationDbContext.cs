using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Domain.Entities;

namespace PayphoneWallet.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATE()");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATE()");

            entity.HasOne(e => e.Wallet)
                  .WithMany(e => e.Transactions)
                  .HasForeignKey(e => e.WalletId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
        foreach (var entry in entries)
        {
            var ent = (BaseEntity)entry.Entity;
            ent.UpdatedAt = DateTime.Now;
        }
        return base.SaveChanges();
    }
}
