﻿// <auto-generated />
using System;
using MVC.Respository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MVC.Migrations.PostgresDB
{
    [DbContext(typeof(PostgresDBContext))]
    partial class PostgresDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MVC.GraphQL.Data.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Abstract")
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int?>("TrackId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.SessionAttendee", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int>("AttendeeId")
                        .HasColumnType("integer");

                    b.HasKey("SessionId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("SessionAttendee");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.SessionSpeaker", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int>("SpeakerId")
                        .HasColumnType("integer");

                    b.HasKey("SessionId", "SpeakerId");

                    b.HasIndex("SpeakerId");

                    b.ToTable("SessionSpeaker");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Speaker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Website")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.HasKey("Id");

                    b.ToTable("Speakers");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("MVC.Models.AccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Session", b =>
                {
                    b.HasOne("MVC.GraphQL.Data.Track", "Track")
                        .WithMany("Sessions")
                        .HasForeignKey("TrackId");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.SessionAttendee", b =>
                {
                    b.HasOne("MVC.GraphQL.Data.Attendee", "Attendee")
                        .WithMany("SessionsAttendees")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC.GraphQL.Data.Session", "Session")
                        .WithMany("SessionAttendees")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attendee");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.SessionSpeaker", b =>
                {
                    b.HasOne("MVC.GraphQL.Data.Session", "Session")
                        .WithMany("SessionSpeakers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC.GraphQL.Data.Speaker", "Speaker")
                        .WithMany("SessionSpeakers")
                        .HasForeignKey("SpeakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("Speaker");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Attendee", b =>
                {
                    b.Navigation("SessionsAttendees");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Session", b =>
                {
                    b.Navigation("SessionAttendees");

                    b.Navigation("SessionSpeakers");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Speaker", b =>
                {
                    b.Navigation("SessionSpeakers");
                });

            modelBuilder.Entity("MVC.GraphQL.Data.Track", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}