﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace WebScrapping.Migrations
{
    [DbContext(typeof(Progam.LogContext))]
    partial class LogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Progam+Log", b =>
                {
                    b.Property<int>("IdLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLog"), 1L, 1);

                    b.Property<string>("CodRob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateLog")
                        .HasColumnType("datetime2");

                    b.Property<string>("IngLog")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Processo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuRob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idProd")
                        .HasColumnType("int");

                    b.HasKey("IdLog");

                    b.ToTable("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}
