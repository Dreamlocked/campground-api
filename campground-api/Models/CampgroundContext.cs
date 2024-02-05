using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace campground_api.Models;

public partial class CampgroundContext : DbContext
{
    public CampgroundContext()
    {
    }

    public CampgroundContext(DbContextOptions<CampgroundContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Campground> Campgrounds { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bookings__3213E83F74317137");

            entity.ToTable("bookings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArrivingDate).HasColumnName("arriving_date");
            entity.Property(e => e.CampgroundId).HasColumnName("campground_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.LeavingDate).HasColumnName("leaving_date");
            entity.Property(e => e.NumNights).HasColumnName("num_nights");
            entity.Property(e => e.PricePerNight)
                .HasColumnType("decimal(14, 4)")
                .HasColumnName("price_per_night");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Campground).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CampgroundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bookings__campgr__7D0E9093");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bookings__user_i__7C1A6C5A");
        });

        modelBuilder.Entity<Campground>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campgrou__3213E83F12B55943");

            entity.ToTable("campgrounds");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.HostId).HasColumnName("host_id");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(15, 9)")
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(15, 9)")
                .HasColumnName("longitude");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("price");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.Host).WithMany(p => p.Campgrounds)
                .HasForeignKey(d => d.HostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campgroun__host___7849DB76");

            entity.HasOne(d => d.Province).WithMany(p => p.Campgrounds)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campgroun__provi__793DFFAF");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__images__3213E83F5AC8F589");

            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampgroundId).HasColumnName("campground_id");
            entity.Property(e => e.Filename).HasColumnName("filename");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.Campground).WithMany(p => p.Images)
                .HasForeignKey(d => d.CampgroundId)
                .HasConstraintName("FK__images__campgrou__02C769E9");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83F7D1D93FB");

            entity.ToTable("notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Message)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Viewed)
                .HasDefaultValue(false)
                .HasColumnName("viewed");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificat__user___2EA5EC27");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__province__3213E83FD1548A32");

            entity.ToTable("provinces");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.RegionId).HasColumnName("region_id");

            entity.HasOne(d => d.Region).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__provinces__regio__756D6ECB");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__regions__3213E83FFD8866DB");

            entity.ToTable("regions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reviews__3213E83F88EACE71");

            entity.ToTable("reviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.BookingsId).HasColumnName("bookings_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Review1).HasColumnName("review");

            entity.HasOne(d => d.Bookings).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookingsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reviews__booking__7FEAFD3E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F61A52AC9");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Hash).HasColumnName("hash");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
