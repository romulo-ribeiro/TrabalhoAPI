﻿// <auto-generated />
using System;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Context.Migrations
{
    [DbContext(typeof(EscolaContext))]
    [Migration("20200921165155_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Context.Models.Course", b =>
                {
                    b.Property<int>("IdCourse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasMaxLength(100);

                    b.HasKey("IdCourse");

                    b.ToTable("Curso");
                });

            modelBuilder.Entity("Context.Models.Subject", b =>
                {
                    b.Property<int>("IdSubject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Registry")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("IdSubject");

                    b.ToTable("Materia");
                });

            modelBuilder.Entity("Context.Models.User", b =>
                {
                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Cpf")
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("IdUser");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Context.Relations.CourseSubject", b =>
                {
                    b.Property<int>("IdSubject")
                        .HasColumnType("int");

                    b.Property<int>("IdCourse")
                        .HasColumnType("int");

                    b.HasKey("IdSubject");

                    b.HasIndex("IdCourse");

                    b.ToTable("Contem");
                });

            modelBuilder.Entity("Context.Relations.UserCourse", b =>
                {
                    b.Property<int>("IdCourse")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCourse");

                    b.HasIndex("IdUser");

                    b.ToTable("Lotacao");
                });

            modelBuilder.Entity("Context.Relations.UserSubject", b =>
                {
                    b.Property<int>("IdSubject")
                        .HasColumnType("int");

                    b.Property<int?>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdSubject");

                    b.HasIndex("IdUser");

                    b.ToTable("Matricula");
                });

            modelBuilder.Entity("Context.Relations.CourseSubject", b =>
                {
                    b.HasOne("Context.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("IdCourse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Context.Models.Subject", "Subject")
                        .WithMany("Enrollment")
                        .HasForeignKey("IdSubject")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Context.Relations.UserCourse", b =>
                {
                    b.HasOne("Context.Models.Course", "Course")
                        .WithMany("CourseContains")
                        .HasForeignKey("IdCourse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Context.Models.User", "User")
                        .WithMany("InCourse")
                        .HasForeignKey("IdUser");
                });

            modelBuilder.Entity("Context.Relations.UserSubject", b =>
                {
                    b.HasOne("Context.Models.Subject", "Subject")
                        .WithMany("Allocation")
                        .HasForeignKey("IdSubject")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Context.Models.User", "User")
                        .WithMany("Registered")
                        .HasForeignKey("IdUser");
                });
#pragma warning restore 612, 618
        }
    }
}
