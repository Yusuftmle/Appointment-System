using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelRv.Infrastructure.Persistence.Context
{
    public class HotelVRContextFactory : IDesignTimeDbContextFactory<HotelVRContext>
    {

        public HotelVRContext CreateDbContext(string[] args)
        {

            // Config dosyalarını oku
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Önemli: migration çalıştırdığın klasör baz alınıyor
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Connection string'i al
            var connectionString = configuration["BlazorSozlukDbConnectionStrings"];

            // DbContextOptions oluştur
            var optionsBuilder = new DbContextOptionsBuilder<HotelVRContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Context'i döndür
            return new HotelVRContext(optionsBuilder.Options);

        }
    }
}
