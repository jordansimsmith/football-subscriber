﻿// <auto-generated />
using System;
using FootballSubscriber.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    [DbContext(typeof(FootballSubscriberContext))]
    partial class FootballSubscriberContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Competition", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<long>("ApiId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApiId")
                        .IsUnique();

                    b.ToTable("Competition");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Fixture", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<long>("ApiId")
                        .HasColumnType("bigint");

                    b.Property<int>("AwayOrganisationId")
                        .HasColumnType("integer");

                    b.Property<string>("AwayOrganisationLogo")
                        .HasColumnType("text");

                    b.Property<long>("AwayTeamApiId")
                        .HasColumnType("bigint");

                    b.Property<int>("AwayTeamId")
                        .HasColumnType("integer");

                    b.Property<string>("AwayTeamName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CompetitionApiId")
                        .HasColumnType("bigint");

                    b.Property<int>("CompetitionId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("HomeOrganisationId")
                        .HasColumnType("integer");

                    b.Property<string>("HomeOrganisationLogo")
                        .HasColumnType("text");

                    b.Property<long>("HomeTeamApiId")
                        .HasColumnType("bigint");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("integer");

                    b.Property<string>("HomeTeamName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApiId")
                        .IsUnique();

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Fixture");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Subscription", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TeamId", "UserId")
                        .IsUnique();

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Team", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<long>("ApiId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApiId")
                        .IsUnique();

                    b.ToTable("Team");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Fixture", b =>
                {
                    b.HasOne("FootballSubscriber.Core.Entities.Team", "AwayTeam")
                        .WithMany("AwayFixtures")
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FootballSubscriber.Core.Entities.Competition", "Competition")
                        .WithMany("Fixtures")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FootballSubscriber.Core.Entities.Team", "HomeTeam")
                        .WithMany("HomeFixtures")
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AwayTeam");

                    b.Navigation("Competition");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Subscription", b =>
                {
                    b.HasOne("FootballSubscriber.Core.Entities.Team", "Team")
                        .WithMany("Subscriptions")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Competition", b =>
                {
                    b.Navigation("Fixtures");
                });

            modelBuilder.Entity("FootballSubscriber.Core.Entities.Team", b =>
                {
                    b.Navigation("AwayFixtures");

                    b.Navigation("HomeFixtures");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
