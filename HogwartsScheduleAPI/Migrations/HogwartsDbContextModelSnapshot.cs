﻿// <auto-generated />
using System;
using HogwartsScheduleAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HogwartsScheduleAPI.Migrations
{
    [DbContext(typeof(HogwartsDbContext))]
    partial class HogwartsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CoursesStudents", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("int");

                    b.Property<int>("StudentsId")
                        .HasColumnType("int");

                    b.HasKey("CoursesId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("CoursesStudents");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ProfessorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique(true);

                    b.HasIndex("ProfessorId")
                        .IsUnique()
                        .HasFilter("[ProfessorId] IS NOT NULL");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.House", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("HouseHeadId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("HouseHeadId")
                        .IsUnique()
                        .HasFilter("[HouseHeadId] IS NOT NULL");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Professor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Patronus")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique(true);

                    b.HasKey("Id");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EnrollmentYear")
                        .HasColumnType("datetime2");

                    b.Property<int>("Family")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("HouseId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CoursesStudents", b =>
                {
                    b.HasOne("HogwartsScheduleAPI.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HogwartsScheduleAPI.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Course", b =>
                {
                    b.HasOne("HogwartsScheduleAPI.Models.Professor", "Professor")
                        .WithOne("Course")
                        .HasForeignKey("HogwartsScheduleAPI.Models.Course", "ProfessorId");

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.House", b =>
                {
                    b.HasOne("HogwartsScheduleAPI.Models.Professor", "HouseHead")
                        .WithOne("HeadingHouse")
                        .HasForeignKey("HogwartsScheduleAPI.Models.House", "HouseHeadId");

                    b.Navigation("HouseHead");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Student", b =>
                {
                    b.HasOne("HogwartsScheduleAPI.Models.House", "House")
                        .WithMany("Students")
                        .HasForeignKey("HouseId");

                    b.Navigation("House");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.House", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("HogwartsScheduleAPI.Models.Professor", b =>
                {
                    b.Navigation("Course");

                    b.Navigation("HeadingHouse");
                });
#pragma warning restore 612, 618
        }
    }
}
