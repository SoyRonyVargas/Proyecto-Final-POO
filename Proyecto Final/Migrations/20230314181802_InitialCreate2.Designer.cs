﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proyecto_Final.clases;

#nullable disable

namespace ProyectoFinal.Migrations
{
    [DbContext(typeof(RestauranteDataContext))]
    [Migration("20230314181802_InitialCreate2")]
    partial class InitialCreate2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Proyecto_Final.clases.Cliente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("apellido")
                        .HasColumnType("longtext");

                    b.Property<string>("nombre")
                        .HasColumnType("longtext");

                    b.Property<string>("rfc")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Pedido", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("mesa")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int>("tipo_pedido")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Pedido_tiene_productos", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("id_pedido")
                        .HasColumnType("int");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.Property<int>("pedidoid")
                        .HasColumnType("int");

                    b.Property<int>("productoid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("pedidoid");

                    b.HasIndex("productoid");

                    b.ToTable("pedido_tiene_productos");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Producto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("id_pedido")
                        .HasColumnType("int");

                    b.Property<int?>("id_producto")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("id_producto");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Pedido_tiene_productos", b =>
                {
                    b.HasOne("Proyecto_Final.clases.Pedido", "pedido")
                        .WithMany()
                        .HasForeignKey("pedidoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Proyecto_Final.clases.Producto", "producto")
                        .WithMany()
                        .HasForeignKey("productoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("pedido");

                    b.Navigation("producto");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Producto", b =>
                {
                    b.HasOne("Proyecto_Final.clases.Pedido", null)
                        .WithMany("productos")
                        .HasForeignKey("id_producto");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Pedido", b =>
                {
                    b.Navigation("productos");
                });
#pragma warning restore 612, 618
        }
    }
}
