using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

internal class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();

            if(!(string.IsNullOrEmpty(tableName)))
                entity.SetTableName(tableName.ToLower());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToLower());
            }        
        }

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(p => p.PaymentType).HasColumnName("payment_type");

            entity.Property(p => p.Title).HasColumnType("varchar(255)");
            entity.Property(p => p.Description).HasColumnType("varchar(2000)");
            entity.Property(p => p.Amount).HasColumnType("decimal(10,2)");
        });
    }
}
