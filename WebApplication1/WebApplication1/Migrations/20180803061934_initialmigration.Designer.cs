﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(NotesContext))]
    [Migration("20180803061934_initialmigration")]
    partial class initialmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication1.Models.checklist", b =>
                {
                    b.Property<int>("checklistId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteId");

                    b.Property<string>("listcontent");

                    b.HasKey("checklistId");

                    b.HasIndex("NoteId");

                    b.ToTable("checklist");
                });

            modelBuilder.Entity("WebApplication1.Models.Labels", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteId");

                    b.Property<string>("name");

                    b.HasKey("LabelId");

                    b.HasIndex("NoteId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("WebApplication1.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NoteContent");

                    b.Property<string>("NoteTitle");

                    b.Property<bool>("pinned");

                    b.HasKey("Id");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("WebApplication1.Models.checklist", b =>
                {
                    b.HasOne("WebApplication1.Models.Note")
                        .WithMany("check")
                        .HasForeignKey("NoteId");
                });

            modelBuilder.Entity("WebApplication1.Models.Labels", b =>
                {
                    b.HasOne("WebApplication1.Models.Note")
                        .WithMany("labellist")
                        .HasForeignKey("NoteId");
                });
#pragma warning restore 612, 618
        }
    }
}
