using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SummerCamp.DataModels.Models;

public partial class SummerCampDbContext : DbContext
{
    public SummerCampDbContext()
    {
    }

    public SummerCampDbContext(DbContextOptions<SummerCampDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<CompetitionMatch> CompetitionMatches { get; set; }

    public virtual DbSet<CompetitionTeam> CompetitionTeams { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamSponsor> TeamSponsors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SummerCamp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Coach__3214EC079105E649");

            entity.ToTable("Coach");

            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC07D6BB33EB");

            entity.ToTable("Competition");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__Competiti__Spons__6383C8BA");
        });

        modelBuilder.Entity<CompetitionMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC07244292F8");

            entity.ToTable("CompetitionMatch");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.CompetitionMatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__AwayT__160F4887");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionMatches)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Compe__17036CC0");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.CompetitionMatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__HomeT__17F790F9");
        });

        modelBuilder.Entity<CompetitionTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC077EF6D2E1");

            entity.ToTable("CompetitionTeam");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionTeams)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__Competiti__Compe__66603565");

            entity.HasOne(d => d.Team).WithMany(p => p.CompetitionTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Competiti__TeamI");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC0725AA17C7");

            entity.ToTable("Player");

            entity.HasIndex(e => new { e.TeamId, e.ShirtNumber }, "UQ_Team_Name").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Player__TeamId");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sponsor__3214EC076393C9F4");

            entity.ToTable("Sponsor");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC07538B2A62");

            entity.ToTable("Team");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NickName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Coach).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK__Team__CoachId__403A8C7D");
        });

        modelBuilder.Entity<TeamSponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamSpon__3214EC071380FA60");

            entity.ToTable("TeamSponsor");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.TeamSponsors)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__TeamSpons__Spons__5FB337D6");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamSponsors)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TeamSpons__TeamI__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
