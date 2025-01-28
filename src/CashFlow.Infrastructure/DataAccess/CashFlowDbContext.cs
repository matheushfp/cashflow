﻿using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=cashflow-db;User=root;Password=root";
        var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));

        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

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
        });
    }
}
