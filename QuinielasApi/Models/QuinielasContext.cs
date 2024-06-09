using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuinielasApi.Models;

public partial class QuinielasContext : DbContext
{
    public QuinielasContext()
    {
    }

    public QuinielasContext(DbContextOptions<QuinielasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Pool> Pools { get; set; }

    public virtual DbSet<Prediction> Predictions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseMySQL(connectionString!);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.PoolId, "pool_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("b'1'")
                .HasColumnName("active");
            entity.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");
            entity.Property(e => e.GameDate)
                .HasColumnType("datetime")
                .HasColumnName("game_date");
            entity.Property(e => e.PoolId).HasColumnName("pool_id");
            entity.Property(e => e.Team1)
                .HasMaxLength(255)
                .HasColumnName("team1");
            entity.Property(e => e.Team1Score).HasColumnName("team1_score");
            entity.Property(e => e.Team2)
                .HasMaxLength(255)
                .HasColumnName("team2");
            entity.Property(e => e.Team2Score).HasColumnName("team2_score");

            entity.HasOne(d => d.Pool).WithMany(p => p.Games)
                .HasForeignKey(d => d.PoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_ibfk_1");
        });

        modelBuilder.Entity<Pool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.AdminId, "admin_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("b'1'")
                .HasColumnName("active");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Private).HasColumnName("private");
            entity.Property(e => e.UsersLimit).HasColumnName("users_limit");

            entity.HasOne(d => d.Admin).WithMany(p => p.Pools)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pools_ibfk_1");
        });

        modelBuilder.Entity<Prediction>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.UserId }).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("b'1'")
                .HasColumnName("active");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Team1Score).HasColumnName("team1_score");
            entity.Property(e => e.Team2Score).HasColumnName("team2_score");

            entity.HasOne(d => d.Game).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Predictions_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Predictions_ibfk_2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("b'1'")
                .HasColumnName("active");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasMany(d => d.PoolsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersPool",
                    r => r.HasOne<Pool>().WithMany()
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UsersPools_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UsersPools_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "PoolId").HasName("PRIMARY");
                        j.ToTable("UsersPools");
                        j.HasIndex(new[] { "PoolId" }, "pool_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("PoolId").HasColumnName("pool_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
