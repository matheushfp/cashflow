﻿// <auto-generated />
using System;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CashFlow.Infrastructure.Migrations
{
    [DbContext(typeof(CashFlowDbContext))]
    [Migration("20250311222650_CreateUsersTable")]
    partial class CreateUsersTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CashFlow.Domain.Entities.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(2000)")
                        .HasColumnName("description");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int")
                        .HasColumnName("payment_type");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("expenses");
                });

            modelBuilder.Entity("CashFlow.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
