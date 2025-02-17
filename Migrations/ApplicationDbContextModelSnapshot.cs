﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderManagement.Data;

#nullable disable

namespace OrderManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("OrderManagement.Models.Order", b =>
                {
                    b.Property<string>("OrderNo")
                        .HasColumnType("TEXT");

                    b.Property<int>("CmpTax")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CustOrderNo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CustSubNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeptCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EmpCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequiredDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SlipComment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Updater")
                        .HasColumnType("TEXT");

                    b.Property<string>("WhCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrderNo");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderManagement.Models.OrderDetail", b =>
                {
                    b.Property<string>("OrderNo")
                        .HasColumnType("TEXT");

                    b.Property<int>("SoRowNo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CmpTaxRate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompleteFlg")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeliveredQty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeliveryDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("DeliveryOrderQty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Discount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderNo1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProdCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProdName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReserveQty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UnitPrice")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Updater")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderNo", "SoRowNo");

                    b.HasIndex("OrderNo1");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("OrderManagement.Models.OrderDetail", b =>
                {
                    b.HasOne("OrderManagement.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderNo1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OrderManagement.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
