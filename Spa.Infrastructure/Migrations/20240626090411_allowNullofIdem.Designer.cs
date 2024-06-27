﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spa.Infrastructure;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    [DbContext(typeof(SpaDbContext))]
    [Migration("20240626090411_allowNullofIdem")]
    partial class allowNullofIdem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Spa.Domain.Entities.Admin", b =>
                {
                    b.Property<long?>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long?>("AdminID"));

                    b.Property<string>("AdminCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Appointment", b =>
                {
                    b.Property<long>("AppointmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AppointmentID"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("BranchID")
                        .HasColumnType("bigint");

                    b.Property<long>("CustomerID")
                        .HasColumnType("bigint");

                    b.Property<double?>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<int?>("DiscountPercentage")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Total")
                        .HasColumnType("float");

                    b.HasKey("AppointmentID");

                    b.HasIndex("BranchID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Assignment", b =>
                {
                    b.Property<long>("EmployerID")
                        .HasColumnType("bigint");

                    b.Property<long>("AppointmentID")
                        .HasColumnType("bigint");

                    b.HasKey("EmployerID", "AppointmentID");

                    b.HasIndex("AppointmentID");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Branch", b =>
                {
                    b.Property<long>("BranchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("BranchID"));

                    b.Property<string>("BranchAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BranchID");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Spa.Domain.Entities.ChooseService", b =>
                {
                    b.Property<long>("ServiceID")
                        .HasColumnType("bigint");

                    b.Property<long>("AppointmentID")
                        .HasColumnType("bigint");

                    b.HasKey("ServiceID", "AppointmentID");

                    b.HasIndex("AppointmentID");

                    b.ToTable("ChooseServices");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Customer", b =>
                {
                    b.Property<long>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CustomerID"));

                    b.Property<string>("CustomerCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerTypeID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.HasIndex("CustomerTypeID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Spa.Domain.Entities.CustomerPhoto", b =>
                {
                    b.Property<long>("PhotoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PhotoID"));

                    b.Property<long>("AppointmentID")
                        .HasColumnType("bigint");

                    b.Property<string>("PhotoPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PhotoID");

                    b.HasIndex("AppointmentID");

                    b.ToTable("CustomerPhotos");
                });

            modelBuilder.Entity("Spa.Domain.Entities.CustomerType", b =>
                {
                    b.Property<int>("CustomerTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerTypeID"));

                    b.Property<string>("CustomerTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerTypeID");

                    b.ToTable("ClientTypes");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Employee", b =>
                {
                    b.Property<long?>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long?>("EmployeeID"));

                    b.Property<long?>("BranchID")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("JobTypeID")
                        .HasColumnType("bigint");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeID");

                    b.HasIndex("BranchID");

                    b.HasIndex("JobTypeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Spa.Domain.Entities.JobType", b =>
                {
                    b.Property<long>("JobTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("JobTypeID"));

                    b.Property<string>("JobTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobTypeID");

                    b.ToTable("JobTypes");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Payment", b =>
                {
                    b.Property<long>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PaymentID"));

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<long>("AppointmentID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerID")
                        .HasColumnType("bigint");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentID");

                    b.HasIndex("AppointmentID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Product", b =>
                {
                    b.Property<long>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ProductID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Purchase", b =>
                {
                    b.Property<long>("SaleID")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductID")
                        .HasColumnType("bigint");

                    b.Property<long>("PurchaseID")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("SaleID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Sale", b =>
                {
                    b.Property<long>("SaleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SaleID"));

                    b.Property<long>("CustomerID")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeeID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("SaleID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Spa.Domain.Entities.ServiceEntity", b =>
                {
                    b.Property<long>("ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServiceID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ServiceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Spa.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<long?>("AdminID")
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<long?>("EmployeeID")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminID")
                        .IsUnique()
                        .HasFilter("[AdminID] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmployeeID")
                        .IsUnique()
                        .HasFilter("[EmployeeID] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Warehouse", b =>
                {
                    b.Property<long>("BranchID")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ImportDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("BranchID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Branch", "Branch")
                        .WithMany("Appointments")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Customer", "Customer")
                        .WithMany("Appointments")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Assignment", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Appointment", "Appointment")
                        .WithMany("Assignments")
                        .HasForeignKey("AppointmentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Employee", "Employees")
                        .WithMany("Assignments")
                        .HasForeignKey("EmployerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Spa.Domain.Entities.ChooseService", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Appointment", "Appointment")
                        .WithMany("ChooseServices")
                        .HasForeignKey("AppointmentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.ServiceEntity", "Service")
                        .WithMany("ChooseServices")
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Customer", b =>
                {
                    b.HasOne("Spa.Domain.Entities.CustomerType", "CustomerType")
                        .WithMany("Customers")
                        .HasForeignKey("CustomerTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CustomerType");
                });

            modelBuilder.Entity("Spa.Domain.Entities.CustomerPhoto", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Appointment", "Appointments")
                        .WithMany("CustomerPhotos")
                        .HasForeignKey("AppointmentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Branch", "Branch")
                        .WithMany("Employees")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Spa.Domain.Entities.JobType", "JobType")
                        .WithMany("Employees")
                        .HasForeignKey("JobTypeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Branch");

                    b.Navigation("JobType");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Payment", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Appointment", "Appointment")
                        .WithMany("Payments")
                        .HasForeignKey("AppointmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Customer", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Purchase", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Product", "Product")
                        .WithMany("Purchases")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Sale", "Sale")
                        .WithMany("Purchases")
                        .HasForeignKey("SaleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Sale", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Customer", "Customer")
                        .WithMany("Sales")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Employee", "Employee")
                        .WithMany("Sales")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Spa.Domain.Entities.User", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Admin", "Admin")
                        .WithMany("User")
                        .HasForeignKey("AdminID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Spa.Domain.Entities.Employee", "Employee")
                        .WithMany("User")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Admin");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Warehouse", b =>
                {
                    b.HasOne("Spa.Domain.Entities.Branch", "Branch")
                        .WithMany("Warehouse")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Spa.Domain.Entities.Product", "Product")
                        .WithMany("Warehouses")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Admin", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Appointment", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("ChooseServices");

                    b.Navigation("CustomerPhotos");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Branch", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Employees");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Payments");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Spa.Domain.Entities.CustomerType", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Employee", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Sales");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Spa.Domain.Entities.JobType", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Product", b =>
                {
                    b.Navigation("Purchases");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("Spa.Domain.Entities.Sale", b =>
                {
                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("Spa.Domain.Entities.ServiceEntity", b =>
                {
                    b.Navigation("ChooseServices");
                });
#pragma warning restore 612, 618
        }
    }
}
