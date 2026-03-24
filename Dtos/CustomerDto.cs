using System;

namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }

        public string Avatar { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }

        public bool HasOrdered { get; set; }

        public string? LatestPurchase { get; set; }

        public bool HasNewsletter { get; set; }

        public List<string> Groups { get; set; } = new();

        public int NbCommands { get; set; }
        public float TotalSpent { get; set; }

        public string Sex { get; set; } = string.Empty;

        public string home_phone { get; set; } = string.Empty;
        public string mobile_phone { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string twitter_url { get; set; } = string.Empty;
        public string instagram_url { get; set; } = string.Empty;
        public string facebook_url { get; set; } = string.Empty;
        public string linkedin_url { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}