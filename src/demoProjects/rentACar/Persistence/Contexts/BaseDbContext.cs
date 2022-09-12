using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF fluent mapping

            modelBuilder.Entity<Brand>(a =>
            {
                a.ToTable("Brands").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name");
                a.HasMany(p => p.Models);
            });

            Brand[] brandEntitySeeds = { new(1, "BMW"), new(2, "Mercedes") };
            modelBuilder.Entity<Brand>().HasData(brandEntitySeeds);

            modelBuilder.Entity<Model>(a =>
            {
                a.ToTable("Models").HasKey(m => m.Id);
                a.Property(m => m.Id).HasColumnName("Id");
                a.Property(m => m.Name).HasColumnName("Name");
                a.Property(m => m.DailyPrice).HasColumnName("DailyPrice");
                a.Property(m => m.ImageUrl).HasColumnName("ImageUrl");
                a.Property(m => m.BrandId).HasColumnName("brandId");
                a.HasOne(m => m.Brand);
            });

            Model[] models = { new()
                {
                    Id = 1,
                    BrandId = 1,
                    Name = "Series 4",
                    DailyPrice = 1500,
                    ImageUrl = "",
                }
            };
            modelBuilder.Entity<Model>().HasData(models);
        }
    }
}