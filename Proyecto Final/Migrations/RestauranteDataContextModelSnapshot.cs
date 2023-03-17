﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proyecto_Final.clases;

#nullable disable

namespace ProyectoFinal.Migrations
{
    [DbContext(typeof(RestauranteDataContext))]
    partial class RestauranteDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
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

            modelBuilder.Entity("Proyecto_Final.clases.Componente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Componentes");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Entrada", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("existencias")
                        .HasColumnType("int");

                    b.Property<int>("existencias_iniciales")
                        .HasColumnType("int");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Entradas");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Pedido", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
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

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("id_pedido")
                        .HasColumnType("int");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("pedido_tiene_productos");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Producto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("precio")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Proyecto_Final.clases.Producto_tiene_componentes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("id_componente")
                        .HasColumnType("int");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("producto_tiene_componentes");
                });
#pragma warning restore 612, 618
        }
    }
}
