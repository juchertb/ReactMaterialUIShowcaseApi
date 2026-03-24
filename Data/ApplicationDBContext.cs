using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Models;
using System.Reflection.Emit;
using ReactMaterialUIShowcaseApi.Enumerations;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Data
{
    /// <summary>
    /// Used for maintaining a database connection and transaction across repositories.
    /// This class is injected into the repositories via dependency injection in the program.cs file.
    /// </summary>
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        public DbSet<SchedulerEventCategory> SchedulerEventCategories { get; set; }
        public DbSet<SchedulerEvent> SchedulerEvents { get; set; }
        public DbSet<SiteProfile> SiteProfiles { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Collection> Collections => Set<Collection>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductTag> ProductTags => Set<ProductTag>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Review> Reviews => Set<Review>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SchedulerEventCategory>(entity =>
            {
                entity.ToTable("SchedulerEventCategories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Label).HasMaxLength(50);
                entity.Property(e => e.ChipColor).HasMaxLength(10);
                entity.Property(e => e.Icon).HasMaxLength(20);
            });

            builder.Entity<SchedulerEvent>(entity =>
            {
                entity.ToTable("SchedulerEvents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Organizer).HasMaxLength(200);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.SchedulerEvents)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<SiteProfile>(entity =>
            {
                entity.ToTable("SiteProfiles");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Firstname).HasMaxLength(100);
                entity.Property(p => p.Lastname).HasMaxLength(100);
                entity.Property(p => p.Email).HasMaxLength(200);
            });

            builder.Entity<SiteProfileTag>(entity =>
            {
                entity.ToTable("SiteProfileTags");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Tag).HasMaxLength(100);

                entity.HasOne(t => t.SiteProfile)
                      .WithMany(p => p.Tags)
                      .HasForeignKey(t => t.SiteProfileId);
            });

            builder.Entity<SiteSettings>(entity =>
            {
                entity.ToTable("SiteSettings");
                entity.HasKey(s => s.Id);
            });

            // Category
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            });

            // Collection
            builder.Entity<Collection>(entity =>
            {
                entity.ToTable("Collections");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            });

            // Color
            builder.Entity<Color>(entity =>
            {
                entity.ToTable("Colors");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            });

            // Product
            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Reference).HasMaxLength(200);
                entity.Property(p => p.Currency).HasMaxLength(10);
                entity.Property(p => p.Sku).HasMaxLength(50);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId);

                entity.HasOne(p => p.Collection)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CollectionId);

                entity.HasOne(p => p.Color)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.ColorId);
            });

            // ProductTag
            builder.Entity<ProductTag>(entity =>
            {
                entity.ToTable("ProductTags");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Tag).HasMaxLength(100);

                entity.HasOne(t => t.Product)
                      .WithMany(p => p.Tags)
                      .HasForeignKey(t => t.ProductId);
            });

            // Customer
            builder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Email).HasMaxLength(200);
                entity.Property(c => c.FirstName).HasMaxLength(100);
                entity.Property(c => c.LastName).HasMaxLength(100);
            });

            // Order
            builder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Customer)
                      .WithMany(cu => cu.Orders)
                      .HasForeignKey(c => c.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Invoice
            builder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id).HasMaxLength(64);

                entity.HasOne(i => i.Order)
                      .WithOne(c => c.Invoice)
                      .HasForeignKey<Invoice>(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Review
            builder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.Reviews)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.Order)
                      .WithMany(c => c.Reviews)
                      .HasForeignKey(r => r.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
  
            // 1. Seed a role
            var roleId = "b1a1e1c2-1111-4444-8888-1234567890ab";
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = roleId,
                    Name = "ApiUser",
                    NormalizedName = "APIUSER"
                }
            );
            var roleId2 = "b1a1e1c2-1111-4444-8888-1234567890a1";
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = roleId2,
                    Name = "RPDE RP Service Provider",
                    NormalizedName = "RPDE-RP-SERVICE-PROVIDER"
                }
            );

            // 2. Seed a user with a hashed password
            var hasher = new PasswordHasher<AppUser>();
            var userId = "a1b2c3d4-5678-1234-9876-abcdefabcdef";
            var testUser = new AppUser
            {
                Id = userId,
                UserName = "testuser",
                NormalizedUserName = "TESTUSER",
                Email = "testuser@example.com",
                NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = "fixed-value-1",
                ConcurrencyStamp = "fixed-value-2",
                // Set your custom properties as needed
                UserId = 1,
                GivenName = "Test",
                Surname = "User",
                OrganizationId = 1,
                Language = LanguageEnum.iEnglish,
                RefreshToken = "AQAAAAIAAYagAAAAEJCwhkk0FEN7KCWho5q",
                RefreshTokenExpiryDate = new DateTime(2030, 1, 1)
            };

            testUser.PasswordHash = "AQAAAAIAAYagAAAAEJCwhkk0FEN7KCWho5q / tFT9OiBfrt / Bnxhx1u2Cwi1R9DslOzF + ihtlSOsebwvT0w ==";

            builder.Entity<AppUser>().HasData(testUser);

            // 3. Assign user to role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = userId,
                    RoleId = roleId
                }
            );

            // 4. Seed additional data
            var userId2 = "a1b2c3d4-5678-1234-9876-abcdefabcde1";
            var testUser2 = new AppUser
            {
                Id = userId2,
                UserName = "someuser",
                NormalizedUserName = "SOMEUSER",
                Email = "someuser@example.com",
                NormalizedEmail = "SOMEUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = "fixed-value-3",
                ConcurrencyStamp = "fixed-value-4",
                // Set your custom properties as needed
                UserId = 2,
                Surname = "Some",
                GivenName = "User",
                OrganizationId = 1,
                Language = LanguageEnum.iEnglish,
                RefreshToken = "AQAAAAIAAYagAAAAEJCwhkk0FEN7KCWho5q",
                RefreshTokenExpiryDate = new DateTime(2030, 2, 2)
            };
            testUser2.PasswordHash = "AQAAAAIAAYagAAAAECvnMT1dCeuCofe0VM705HhPR3SFTo60kYB1b9/I+caNdRNWPpyyFEmeIdYxDbMb32Bg==";

            builder.Entity<AppUser>().HasData(testUser2);

            // 3. Assign user to role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = userId2,
                    RoleId = roleId2
                }
            );
        }
    }
}
