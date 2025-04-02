using System.Text.RegularExpressions;
using CashFlow.Domain.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();

            if(!(string.IsNullOrEmpty(tableName)))
                entity.SetTableName(tableName.Pluralize().ToLowerInvariant());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ConvertToSnakeCase(property.GetColumnName()));
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

    private string ConvertToSnakeCase(string input)
    {
        return Regex.Replace(input, @"([a-z])([A-Z])", "$1_$2").ToLowerInvariant();
    }
}
