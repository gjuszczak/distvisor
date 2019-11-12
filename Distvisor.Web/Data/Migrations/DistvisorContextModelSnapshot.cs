﻿// <auto-generated />
using System;
using Distvisor.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Distvisor.Web.Data.Migrations
{
    [DbContext(typeof(DistvisorContext))]
    partial class DistvisorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("Distvisor.Web.Data.Models.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpireOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("IssuedAtUtc")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LockoutUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Distvisor.Web.Data.Models.Session", b =>
                {
                    b.HasOne("Distvisor.Web.Data.Models.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
