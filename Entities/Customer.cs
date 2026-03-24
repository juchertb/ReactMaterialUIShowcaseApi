using ReactMaterialUIShowcaseApi.Entities;
using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }

        public string Avatar { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }

        public bool HasOrdered { get; set; }
        public bool HasNewsletter { get; set; }

        public string? LatestPurchase { get; set; }

        public int NbCommands { get; set; }
        public float TotalSpent { get; set; }

        public string Sex { get; set; } = string.Empty;

        public string HomePhone { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string TwitterUrl { get; set; } = string.Empty;
        public string InstagramUrl { get; set; } = string.Empty;
        public string FacebookUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}