﻿// <auto-generated />
using System;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Distvisor.Web.Data.Reads.Migrations
{
    [DbContext(typeof(ReadStoreContext))]
    partial class ReadStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.FinancialAccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FinancialAccounts");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.FinancialAccountPaycardEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("FinancialAccountPaycards");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.ProcessedEmailEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("BodyMime")
                        .HasColumnType("text");

                    b.Property<string>("UniqueKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UniqueKey")
                        .IsUnique();

                    b.ToTable("ProcessedEmails");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.RedirectionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Redirections");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.SecretsVaultEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Key")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SecretsVault");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.FinancialAccountPaycardEntity", b =>
                {
                    b.HasOne("Distvisor.Web.Data.Reads.Entities.FinancialAccountEntity", "Account")
                        .WithMany("Paycards")
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Reads.Entities.FinancialAccountEntity", b =>
                {
                    b.Navigation("Paycards");
                });
#pragma warning restore 612, 618
        }
    }
}
