﻿// <auto-generated />
using System;
using DataMigration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataMigration.Migrations
{
    [DbContext(typeof(DataMigrationContext))]
    [Migration("20181109182911_AddedBuddAccount")]
    partial class AddedBuddAccount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlpEntities.BuddAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Number")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("BuddAccounts");
                });

            modelBuilder.Entity("BlpEntities.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("TestEntities");
                });

            modelBuilder.Entity("BlpEntities.TestEntityNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Note");

                    b.Property<int?>("TestEntityId");

                    b.HasKey("Id");

                    b.HasIndex("TestEntityId");

                    b.ToTable("TestEntityNotes");
                });

            modelBuilder.Entity("BlpEntities.TestEntityNote", b =>
                {
                    b.HasOne("BlpEntities.TestEntity", "TestEntity")
                        .WithMany("TestEntityNotes")
                        .HasForeignKey("TestEntityId");
                });
#pragma warning restore 612, 618
        }
    }
}
