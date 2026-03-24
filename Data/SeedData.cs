using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace ReactMaterialUIShowcase.Data
{
    public static class SeedData
    {
        private static readonly Random _rand = new();
        private static readonly Dictionary<string, string[]> _categoryImages = new()
        {
            ["animals"] = new[]
            {
                "https://marmelab.com/posters/animals-1.jpeg",
                "https://marmelab.com/posters/animals-2.jpeg",
                "https://marmelab.com/posters/animals-3.jpeg"
            },
            ["beard"] = new[]
            {
                "https://marmelab.com/posters/people-1.jpeg",
                "https://marmelab.com/posters/people-2.jpeg"
            },
            ["business"] = new[]
            {
                "https://picsum.photos/seed/business1/600/600",
                "https://picsum.photos/seed/business2/600/600"
            },
            ["cars"] = new[]
            {
                "https://picsum.photos/seed/cars1/600/600",
                "https://picsum.photos/seed/cars2/600/600"
            },
            ["city"] = new[]
            {
                "https://picsum.photos/seed/city1/600/600",
                "https://picsum.photos/seed/city2/600/600"
            },
            ["flowers"] = new[]
            {
                "https://picsum.photos/seed/flowers1/600/600",
                "https://picsum.photos/seed/flowers2/600/600"
            },
            ["food"] = new[]
            {
                "https://picsum.photos/seed/food1/600/600",
                "https://picsum.photos/seed/food2/600/600"
            },
            ["nature"] = new[]
            {
                "https://picsum.photos/seed/nature1/600/600",
                "https://picsum.photos/seed/nature2/600/600"
            },
            ["people"] = new[]
            {
                "https://marmelab.com/posters/people-3.jpeg",
                "https://marmelab.com/posters/people-4.jpeg"
            },
            ["sports"] = new[]
            {
                "https://picsum.photos/seed/sports1/600/600",
                "https://picsum.photos/seed/sports2/600/600"
            },
            ["tech"] = new[]
            {
                "https://picsum.photos/seed/tech1/600/600",
                "https://picsum.photos/seed/tech2/600/600"
            },
            ["travel"] = new[]
            {
                "https://picsum.photos/seed/travel1/600/600",
                "https://picsum.photos/seed/travel2/600/600"
            },
            ["water"] = new[]
            {
                "https://picsum.photos/seed/water1/600/600",
                "https://picsum.photos/seed/water2/600/600"
            }
        };

        private static readonly string[] _descriptions =
        {
            "A beautifully crafted item designed with attention to detail and built to last.",
            "A customer favorite known for its reliability and elegant design.",
            "A versatile product that blends style and functionality effortlessly.",
            "Premium quality materials ensure durability and long‑lasting performance.",
            "A modern classic that fits perfectly into any lifestyle or environment.",
            "Lightweight, practical, and made for everyday use.",
            "A stylish and functional choice for those who appreciate quality.",
            "Engineered for comfort and designed to impress.",
            "A refined product offering exceptional value and craftsmanship.",
            "A bold and expressive piece that stands out in any collection."
        };

        private static class SchedulerEventCategoryIconEnum
        {
            public const string Eat = "FastFood";
            public const string Code = "Computer";
            public const string Sleep = "Hotel";
            public const string Repeat = "Repeat";
            public const string Meet = "Groups";
            public const string Call = "LocalPhone";
            public const string Review = "RateReview";
            public const string Onboarding = "PersonAdd";
        };

        public static void Initialize(ApplicationDBContext context, ILogger logger)
        {
            context.Database.Migrate();

            logger.LogInformation("Checking if database needs seeding...");
            if (context.Categories.Any() &&
                context.Collections.Any() && 
                context.Colors.Any() && 
                context.Products.Any() &&
                context.Customers.Any() &&
                context.Orders.Any() &&
                context.Invoices.Any() &&
                context.Reviews.Any() &&
                context.SchedulerEventCategories.Any() &&
                context.SchedulerEvents.Any() &&
                context.SiteProfiles.Any() &&
                context.SiteSettings.Any())
            {
                logger.LogInformation("Database already seeded. Skipping.");
                return;
            }

            SeedCategories(context, logger);
            SeedCollections(context, logger);
            SeedColors(context, logger);
            logger.LogInformation("Saving code table changes...");
            context.SaveChanges();

            SeedProducts(context, logger);
            logger.LogInformation("Saving changes...");
            context.SaveChanges();

            SeedCustomers(context, logger);
            logger.LogInformation("Saving changes...");
            context.SaveChanges();

            SeedCommands(context, logger);
            logger.LogInformation("Saving changes...");
            context.SaveChanges();

            SeedInvoices(context, logger);
            logger.LogInformation("Saving changes...");
            context.SaveChanges();
            
            SeedReviews(context, logger);
            SeedSchedulerEventCategories(context, logger);
            logger.LogInformation("Saving changes...");
            context.SaveChanges();

            SeedSchedulerEvents(context, logger);
            SeedSiteSettings(context, logger);

            logger.LogInformation("Saving remaining changes...");
            context.SaveChanges();
            logger.LogInformation("Seeding complete.");
        }

        private static void SeedCategories(ApplicationDBContext context, ILogger logger)
        {
            if (context.Categories.Any()) return;
            logger.LogInformation("Seeding categories...");

            var categories = new[]
            {
                new Category { Name = "animals" },
                new Category { Name = "beard" },
                new Category { Name = "business" },
                new Category { Name = "cars" },
                new Category { Name = "city" },
                new Category { Name = "flowers" },
                new Category { Name = "food" },
                new Category { Name = "nature" },
                new Category { Name = "people" },
                new Category { Name = "sports" },
                new Category { Name = "tech" },
                new Category { Name = "travel" },
                new Category { Name = "water" }
            };

            context.Categories.AddRange(categories);
        }

        private static void SeedCollections(ApplicationDBContext context, ILogger logger)
        {
            if (context.Collections.Any()) return;
            logger.LogInformation("Seeding collections...");

            var collections = new[]
            {
                new Collection { Name = "Summer" },
                new Collection { Name = "Winter" },
                new Collection { Name = "Fall" },
                new Collection { Name = "Hot" },
                new Collection { Name = "Wild" },
                new Collection { Name = "Flowery" },
                new Collection { Name = "Nature" },
                new Collection { Name = "Sports" },
                new Collection { Name = "People" }
            };

            context.Collections.AddRange(collections);
        }

        private static void SeedColors(ApplicationDBContext context, ILogger logger)
        {
            if (context.Colors.Any()) return;
            logger.LogInformation("Seeding colors...");

            var colors = new[]
            {
                new Color { Name = "Red" },
                new Color { Name = "Blue" },
                new Color { Name = "Green" },
                new Color { Name = "Yellow" },
                new Color { Name = "Black" },
                new Color { Name = "White" },
                new Color { Name = "Sand" },
                new Color { Name = "Grey" },
                new Color { Name = "Purple" },
                new Color { Name = "Pink" },
                new Color { Name = "Braun" },
                new Color { Name = "Orange" },
                new Color { Name = "Navy blue" }
            };

            context.Colors.AddRange(colors);
        }

        private static void SeedProducts(ApplicationDBContext context, ILogger logger)
        {
            if (context.Products.Any()) return;
            logger.LogInformation("Seeding products...");

            var categories = context.Categories.ToList();
            var collections = context.Collections.ToList();
            var colors = context.Colors.ToList();

            var products = new List<Product>();

            for (int i = 1; i <= 30; i++)
            {
                var category = categories[_rand.Next(categories.Count)];
                var collection = collections[_rand.Next(collections.Count)];
                var color = colors[_rand.Next(colors.Count)];

                // Pick category‑specific image
                var imagePool = _categoryImages.ContainsKey(category.Name)
                    ? _categoryImages[category.Name]
                    : new[] { $"https://picsum.photos/seed/fallback{i}/600/600" };

                var imageUrl = imagePool[_rand.Next(imagePool.Length)];

                var product = new Product
                {
                    Reference = $"{category.Name} Item {i}",
                    Width = _rand.Next(10, 100),
                    Height = _rand.Next(10, 100),
                    Price = (float)(_rand.NextDouble() * 200 + 10),
                    Thumbnail = imageUrl,
                    Image = imageUrl,
                    Description = _descriptions[_rand.Next(_descriptions.Length)],
                    Stock = _rand.Next(0, 200),
                    Sales = _rand.Next(0, 500),
                    Weight = (float)(_rand.NextDouble() * 5),
                    ShopifyHandle = $"{category.Name.ToLower()}-{i}",
                    FacebookAccount = "https://facebook.com",
                    InstagramAccount = "https://instagram.com",
                    Currency = "USD",
                    Sku = $"SKU{i:00000}",
                    CategoryId = category.Id,
                    CollectionId = collection.Id,
                    ColorId = color.Id
                };

                // Random tags
                var tags = new[] { "New", "Sale", "Featured", "Limited", "Hot", "Trending" };
                int tagCount = _rand.Next(1, 4);

                for (int t = 0; t < tagCount; t++)
                {
                    product.Tags.Add(new ProductTag { Tag = tags[_rand.Next(tags.Length)] });
                }

                products.Add(product);
            }

            context.Products.AddRange(products);
        }

        private static void SeedCustomers(ApplicationDBContext context, ILogger logger)
        {
            if (context.Customers.Any()) return;
            logger.LogInformation("Seeding customers...");

            var firstNames = new[] { "John", "Jane", "Alex", "Chris", "Sam", "Taylor", "Jordan", "Morgan" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

            var customers = new List<Customer>();

            for (int i = 1; i <= 40; i++)
            {
                var first = firstNames[_rand.Next(firstNames.Length)];
                var last = lastNames[_rand.Next(lastNames.Length)];

                var customer = new Customer
                {
                    FirstName = first,
                    LastName = last,
                    Email = $"{first.ToLower()}.{last.ToLower()}{i}@example.com",
                    Address = $"{_rand.Next(100, 999)} Main St",
                    Zipcode = $"{_rand.Next(10000, 99999)}",
                    City = "Metropolis",
                    Avatar = $"https://i.pravatar.cc/150?img={_rand.Next(1, 70)}",
                    Birthday = DateTime.Now.AddYears(-_rand.Next(18, 70)),
                    FirstSeen = DateTime.Now.AddMonths(-_rand.Next(1, 24)),
                    LastSeen = DateTime.Now.AddDays(-_rand.Next(0, 60)),
                    HasOrdered = _rand.Next(0, 2) == 1,
                    HasNewsletter = _rand.Next(0, 2) == 1,
                    LatestPurchase = "",
                    NbCommands = 0,
                    TotalSpent = 0,
                    Sex = _rand.Next(0, 2) == 0 ? "male" : "female",
                    HomePhone = "555-111-2222",
                    MobilePhone = "555-333-4444",
                    Position = "Customer",
                    TwitterUrl = "https://twitter.com",
                    InstagramUrl = "https://instagram.com",
                    FacebookUrl = "https://facebook.com",
                    LinkedInUrl = "https://linkedin.com",
                    Role = "User"
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
        }

        private static void SeedCommands(ApplicationDBContext context, ILogger logger)
        {
            if (context.Orders.Any()) return;
            logger.LogInformation("Seeding orders...");

            var customers = context.Customers.ToList();
            var orders = new List<Order>();

            for (int i = 1; i <= 50; i++)
            {
                var customer = customers[_rand.Next(customers.Count)];

                float subtotal = (float)(_rand.NextDouble() * 200 + 20);
                float delivery = (float)(_rand.NextDouble() * 10 + 5);
                float taxRate = 0.15f;
                float taxes = subtotal * taxRate;
                float total = subtotal + delivery + taxes;

                var order = new Order
                {
                    CustomerId = customer.Id,
                    Date = DateTime.Now.AddDays(-_rand.Next(1, 180)),
                    TotalExTaxes = subtotal,
                    DeliveryFees = delivery,
                    TaxRate = taxRate,
                    Taxes = taxes,
                    Total = total
                };

                orders.Add(order);
            }

            context.Orders.AddRange(orders);
        }

        private static void SeedInvoices(ApplicationDBContext context, ILogger logger)
        {
            if (context.Invoices.Any()) return;
            logger.LogInformation("Seeding invoices...");

            var orders = context.Orders.ToList();
            var invoices = new List<Invoice>();
            var statuses = new[] { "ordered", "delivered", "cancelled" };

            foreach (var order in orders)
            {
                var status = statuses[_rand.Next(statuses.Length)];

                invoices.Add(new Invoice
                {
                    Id = Guid.NewGuid().ToString("N"),
                    OrderId = order.Id,
                    Date = order.Date,
                    TotalExTaxes = order.TotalExTaxes,
                    DeliveryFees = order.DeliveryFees,
                    TaxRate = order.TaxRate,
                    Taxes = order.Taxes,
                    Total = order.Total,
                    Status = status,
                    Reference = $"ORDER{order.Id:00000}"
                });
            }

            context.Invoices.AddRange(invoices);
        }

        private static void SeedReviews(ApplicationDBContext context, ILogger logger)
        {
            if (context.Reviews.Any()) return;
            logger.LogInformation("Seeding reviews...");

            var customers = context.Customers.ToList();
            var orders = context.Orders.ToList();

            var reviews = new List<Review>();

            for (int i = 1; i <= 30; i++)
            {
                var customer = customers[_rand.Next(customers.Count)];
                var order = orders[_rand.Next(orders.Count)];

                reviews.Add(new Review
                {
                    CustomerId = customer.Id,
                    OrderId = order.Id,
                    Date = order.Date.AddDays(1),
                    TotalExTaxes = order.TotalExTaxes,
                    DeliveryFees = order.DeliveryFees,
                    TaxRate = order.TaxRate,
                    Taxes = order.Taxes,
                    Total = order.Total,
                    Text = "Auto‑generated review text."
                });
            }

            context.Reviews.AddRange(reviews);
        }

        private static void SeedSchedulerEventCategories(ApplicationDBContext context, ILogger logger)
        {
            if (context.SchedulerEventCategories.Any()) return;
            logger.LogInformation("Seeding scheduler events categories...");

            var categories = new[]
            {
                new SchedulerEventCategory { Label = "Meeting", ChipColor = "#02d3f8ff", Icon = SchedulerEventCategoryIconEnum.Meet },
                new SchedulerEventCategory { Label = "Call",ChipColor = "#33f802ff", Icon = SchedulerEventCategoryIconEnum.Call },
                new SchedulerEventCategory { Label = "Review", ChipColor = "#f8022bff", Icon = SchedulerEventCategoryIconEnum.Review },
                new SchedulerEventCategory { Label = "Onboarding", ChipColor = "#f8c902ff", Icon = SchedulerEventCategoryIconEnum.Onboarding },
                new SchedulerEventCategory { Label = "Eat", ChipColor = "#7cb8fdff", Icon = SchedulerEventCategoryIconEnum.Eat },
                new SchedulerEventCategory { Label = "Code", ChipColor = "#b3faa1ff", Icon = SchedulerEventCategoryIconEnum.Code },
                new SchedulerEventCategory { Label = "Sleep", ChipColor = "#ff96a8ff", Icon = SchedulerEventCategoryIconEnum.Sleep },
                new SchedulerEventCategory { Label = "Repeat", ChipColor = "#f3e39dff", Icon = SchedulerEventCategoryIconEnum.Repeat }
            };

            context.SchedulerEventCategories.AddRange(categories);
        }

        private static void SeedSchedulerEvents(ApplicationDBContext context, ILogger logger)
        {
            if (context.SchedulerEvents.Any()) return;
            logger.LogInformation("Seeding scheduler events...");

            var titles = new[] { "Meeting", "Workshop", "Call", "Review", "Planning", "Sync" };
            var categories = context.SchedulerEventCategories.ToList();

            var firstNames = new[] { "John", "Jane", "Alex", "Chris", "Sam", "Taylor", "Jordan", "Morgan" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

            var eventsList = new List<SchedulerEvent>();

            for (int i = 1; i <= 25; i++)
            {
                var date = DateTime.Now.AddDays(_rand.Next(-30, 60));
                var first = firstNames[_rand.Next(firstNames.Length)];
                var last = lastNames[_rand.Next(lastNames.Length)];

                eventsList.Add(new SchedulerEvent
                {
                    DateTime = date,
                    Date = date.ToString("yyyy-MM-dd"),
                    Title = titles[_rand.Next(titles.Length)],
                    StartHour = $"{_rand.Next(8, 18)}:00",
                    Category = categories[_rand.Next(categories.Count)],
                    Organizer = $"{first} {last}",
                    Details = "Auto‑generated event",
                    IsAllDay = false,
                    IsRepeated = false,
                    RepeatInterval = "",
                    RepeatEvery = 0,
                    RepeatOnWeekday = 0,
                    RepeatEnd = "",
                    RepeatEndOn = 0,
                    RepeatEndAfter = ""
                });
            }

            context.SchedulerEvents.AddRange(eventsList);
        }

        private static void SeedSiteProfiles(ApplicationDBContext context, ILogger logger)
        {
            if (context.SiteProfiles.Any()) return;
            logger.LogInformation("Seeding site profile..."); SeedSiteProfiles(context, logger);


            var profile = new SiteProfile
            {
                Firstname = "Admin",
                Lastname = "User",
                Gender = 1,
                BirthDate = DateTime.Now.AddYears(-35),
                Email = "admin@example.com",
                Location = "HQ",
                Phone = "555-999-8888",
                Language = 1,
                Avatar = "https://i.pravatar.cc/150?img=2"
            };

            profile.Tags.Add(new SiteProfileTag { Tag = "Admin" });
            profile.Tags.Add(new SiteProfileTag { Tag = "Internal" });

            context.SiteProfiles.Add(profile);
        }

        private static void SeedSiteSettings(ApplicationDBContext context, ILogger logger)
        {
            if (context.SiteSettings.Any())
            {
                return;
            }
            logger.LogInformation("Seeding site settings...");


            var settings = new SiteSettings
            {
                ProfileInvisibleMode = false,

                AccountsSlack = true,
                AccountsSpotify = false,
                AccountsAtlassian = true,
                AccountsAsana = false,

                NotifMentionsEmail = true,
                NotifMentionsPush = true,
                NotifMentionsSms = false,

                NotifCommentsEmail = true,
                NotifCommentsPush = false,
                NotifCommentsSms = false,

                NotifFollowsEmail = true,
                NotifFollowsPush = false,
                NotifFollowsSms = false,

                NotifLoginEmail = true,
                NotifLoginPush = false,
                NotifLoginSms = false
            };

            context.SiteSettings.Add(settings);
        }

    }
}





