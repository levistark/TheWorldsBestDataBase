using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Contexts;

public partial class ProductDataContext : DbContext
{
    public ProductDataContext()
    {
    }

    public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LanguageEntity> Languages { get; set; }

    public virtual DbSet<ManufacturerEntity> Manufacturers { get; set; }

    public virtual DbSet<ProductEntity> Products { get; set; }

    public virtual DbSet<ProductCategoryEntity> ProductCategories { get; set; }

    public virtual DbSet<ProductDescriptionEntity> ProductDescriptions { get; set; }

    public virtual DbSet<ProductReviewEntity> ProductReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LanguageEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC074923DDBD");
        });

        modelBuilder.Entity<ManufacturerEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07E625A0D5");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C9911432CA5B8FA");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Manufa__70A8B9AE");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Produc__719CDDE7");
        });

        modelBuilder.Entity<ProductCategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC073456206C");
        });

        modelBuilder.Entity<ProductDescriptionEntity>(entity =>
        {
            entity.HasKey(e => new { e.ArticleNumber, e.LanguageId }).HasName("PK__ProductD__970A941940A4DA5C");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.ProductDescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Artic__7755B73D");

            entity.HasOne(d => d.Language).WithMany(p => p.ProductDescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Langu__7849DB76");
        });

        modelBuilder.Entity<ProductReviewEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07A82048DD");

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.ProductReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductRe__Artic__74794A92");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
