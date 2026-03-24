using System;
using System.Collections.Generic;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class SiteProfile
    {
        public int Id { get; set; }

        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;

        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public int Language { get; set; }

        public string Avatar { get; set; } = string.Empty;

        public ICollection<SiteProfileTag> Tags { get; set; } = new List<SiteProfileTag>();
    }
}