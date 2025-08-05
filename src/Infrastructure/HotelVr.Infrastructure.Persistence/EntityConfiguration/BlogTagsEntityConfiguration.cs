using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using HotelRvDbContext.Infrastructure.Persistence.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelRv.Infrastructure.Persistence.EntityConfiguration
{
    public class BlogTagsEntityConfiguration:BaseEntityConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            base.Configure(builder);
            builder.ToTable("BlogTags", HotelVRContext.DEFAULT_SCHEMA); // Çoğul isim

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // Case-insensitive arama için

            // Benzersiz indeks
            builder.HasIndex(b => b.Name)
                .IsUnique();

        }
    }
}
