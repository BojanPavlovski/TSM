﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagementSystem.DataAccess;

#nullable disable

namespace TaskManagementSystem.DataAccess.Migrations
{
    [DbContext(typeof(TaskManagementSystemDbContext))]
    [Migration("20241219140147__Mig")]
    partial class _Mig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskManagementSystem.Domain.Domain.TaskModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Create domain model TaskModel and its properties accordingly",
                            IsCompleted = true,
                            Name = "Create Domain Models."
                        },
                        new
                        {
                            Id = 2,
                            Description = "Create a service class that will communicate with controllers and database",
                            IsCompleted = false,
                            Name = "Create service class library project."
                        },
                        new
                        {
                            Id = 3,
                            Description = "Create a DataAccess class library project that will comunicate with a database",
                            IsCompleted = true,
                            Name = "Create DataAccess class library project."
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
