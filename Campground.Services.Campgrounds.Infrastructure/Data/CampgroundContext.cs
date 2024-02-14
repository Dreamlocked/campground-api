using System;
using System.Collections.Generic;
using Campground.Services.Campgrounds.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Campground.Services.Campgrounds.Infrastructure.Data;

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

    public virtual DbSet<Domain.Entities.Campground> Campgrounds { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_9");

            entity.ToTable("bookings");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ArrivingDate)
                .HasColumnType("datetime")
                .HasColumnName("arriving_date");
            entity.Property(e => e.Attended)
                .HasDefaultValue(false)
                .HasColumnName("attended");
            entity.Property(e => e.CampgroundId).HasColumnName("campground_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.LeavingDate)
                .HasColumnType("datetime")
                .HasColumnName("leaving_date");
            entity.Property(e => e.Paid)
                .HasDefaultValue(false)
                .HasColumnName("paid");
            entity.Property(e => e.ReviewBody)
                .IsUnicode(false)
                .HasColumnName("review_body");
            entity.Property(e => e.ReviewCreateAt)
                .HasColumnType("datetime")
                .HasColumnName("review_create_at");
            entity.Property(e => e.ReviewRating).HasColumnName("review_rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Campground).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CampgroundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_campground_bookings");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_user_bookings");
        });

        modelBuilder.Entity<Domain.Entities.Campground>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_3");

            entity.ToTable("campgrounds");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.HostId).HasColumnName("host_id");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(15, 9)")
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(15, 9)")
                .HasColumnName("longitude");
            entity.Property(e => e.PricePerNight)
                .HasColumnType("decimal(15, 9)")
                .HasColumnName("price_per_night");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Host).WithMany(p => p.Campgrounds)
                .HasForeignKey(d => d.HostId)
                .HasConstraintName("FK_user_campgrounds");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_10");

            entity.ToTable("images");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Alt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("alt");
            entity.Property(e => e.CampgroundsId).HasColumnName("campgrounds_id");
            entity.Property(e => e.Filename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("filename");
            entity.Property(e => e.Url)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.Campgrounds).WithMany(p => p.Images)
                .HasForeignKey(d => d.CampgroundsId)
                .HasConstraintName("FK_campground_images");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_11");

            entity.ToTable("notifications");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Message)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Viewed)
                .HasDefaultValue(false)
                .HasColumnName("viewed");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_14");

            entity.ToTable("reviews");

            entity.HasIndex(e => e.BookingId, "UQ__reviews__5DE3A5B0C0D38889").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Comment)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Booking).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_REFERENCE_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_6");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Salt)
                .IsUnicode(false)
                .HasColumnName("salt");
            entity.Property(e => e.UrlPhoto)
                .IsUnicode(false)
                .HasColumnName("url_photo");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
