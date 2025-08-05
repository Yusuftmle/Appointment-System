using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelRvDbContext.Infrastructure.Persistence.EntityConfiguration
{
    public class BlogPostEntityConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
           
            builder.ToTable("BlogPost", HotelVRContext.DEFAULT_SCHEMA);

            // Başlık alanı için kısıtlamalar
            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            // İçerik alanı için kısıtlamalar
            builder.Property(b => b.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // Özet alanı için kısıtlamalar
            builder.Property(b => b.Summary)
                .IsRequired()
                .HasMaxLength(500);

            // Slug alanı için kısıtlamalar
            builder.Property(b => b.Slug)
                .HasMaxLength(200)
                .HasComputedColumnSql("LOWER(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), ',', ''), '.', ''))", stored: true); // 'persisted' = stored: true

            // Anahtar kelimeler için kısıtlamalar
            builder.Property(b => b.Keywords)
                .HasMaxLength(500);

            // Görüntü sayısı için varsayılan değer
            builder.Property(b => b.ViewCount)
                .HasDefaultValue(0);

            // Yayınlanma durumu için varsayılan değer
            builder.Property(b => b.IsPublished)
                .HasDefaultValue(false);

            // Oluşturulma tarihi için varsayılan değer
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(b => b.Slug).IsUnique();

            // User ilişkisi
            builder.HasOne(bp => bp.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            // BlogPostTag ilişkisi
           


        }
    }
}
