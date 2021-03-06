﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using WebApplication15.Models;

namespace WebApplication15.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20181125231123_DepositAccount")]
    partial class DepositAccount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication15.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Credit");

                    b.Property<decimal>("Debet");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<string>("Number");

                    b.Property<decimal>("Saldo");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Account");
                });

            modelBuilder.Entity("WebApplication15.Models.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsRevocable");

                    b.Property<string>("Name");

                    b.Property<decimal>("Rate");

                    b.Property<int>("Term");

                    b.HasKey("Id");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("WebApplication15.Models.DepositContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BeginDate");

                    b.Property<int>("Currency");

                    b.Property<decimal>("DepositAmount");

                    b.Property<int>("DepositId");

                    b.Property<decimal>("DepositPercent");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Number");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DepositId");

                    b.HasIndex("UserId");

                    b.ToTable("DepositContracts");
                });

            modelBuilder.Entity("WebApplication15.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressOfResidence");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("BirthPlace");

                    b.Property<int>("CityOfResidence");

                    b.Property<int>("DisabilityGroup");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("HomePhone");

                    b.Property<DateTime>("IssueDate");

                    b.Property<string>("IssuedBy");

                    b.Property<int>("MaritalStatus");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("MonthlyIncome");

                    b.Property<int>("Nationality");

                    b.Property<string>("PassportId");

                    b.Property<string>("PassportNumber");

                    b.Property<string>("PassportSeries");

                    b.Property<bool>("Pensioner");

                    b.Property<int>("RegistrationCity");

                    b.Property<string>("SecondName");

                    b.Property<int>("Sex");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication15.Models.CreditAccount", b =>
                {
                    b.HasBaseType("WebApplication15.Models.Account");


                    b.ToTable("CreditAccount");

                    b.HasDiscriminator().HasValue("CreditAccount");
                });

            modelBuilder.Entity("WebApplication15.Models.DepositAccount", b =>
                {
                    b.HasBaseType("WebApplication15.Models.Account");

                    b.Property<int>("DepositContractId");

                    b.Property<bool>("IsForPercents");

                    b.HasIndex("DepositContractId");

                    b.ToTable("DepositAccount");

                    b.HasDiscriminator().HasValue("DepositAccount");
                });

            modelBuilder.Entity("WebApplication15.Models.Account", b =>
                {
                    b.HasOne("WebApplication15.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication15.Models.DepositContract", b =>
                {
                    b.HasOne("WebApplication15.Models.Deposit", "Deposit")
                        .WithMany()
                        .HasForeignKey("DepositId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication15.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication15.Models.DepositAccount", b =>
                {
                    b.HasOne("WebApplication15.Models.DepositContract", "DepositContract")
                        .WithMany()
                        .HasForeignKey("DepositContractId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
