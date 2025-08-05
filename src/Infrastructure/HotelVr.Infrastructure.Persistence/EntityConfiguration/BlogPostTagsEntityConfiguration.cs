using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using HotelRvDbContext.Infrastructure.Persistence.EntityConfiguration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelRv.Infrastructure.Persistence.EntityConfiguration
{
    public class BlogPostTagsEntityConfiguration : BaseEntityConfiguration<BlogPostTag>
    {
        public void Configure(EntityTypeBuilder<BlogPostTag> builder)
        {
            base.Configure(builder);
            builder.ToTable("BlogPostTags", HotelVRContext.DEFAULT_SCHEMA); // Çoğul isim kullanımı daha uygun

            // Composite primary key
            builder.HasKey(bpt => new { bpt.BlogPostId, bpt.BlogTagId });

            // BlogPost ilişkisi
            builder.HasOne(bpt => bpt.BlogPost)
                .WithMany(bp => bp.BlogPostTags)
                .HasForeignKey(bpt => bpt.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade); // Dikkat: Çift taraflı Cascade tehlikeli olabilir

            // BlogTag ilişkisi
            builder.HasOne(bpt => bpt.BlogTag)
                .WithMany(bt => bt.BlogPostTags)
                .HasForeignKey(bpt => bpt.BlogTagId)
                .OnDelete(DeleteBehavior.Cascade); // Dikkat: Çift taraflı Cascade

            // Ek indeksler
            builder.HasIndex(bpt => bpt.BlogPostId);
            builder.HasIndex(bpt => bpt.BlogTagId);

            // Zaman damgaları
            builder.Property(bpt => bpt.CreateDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
