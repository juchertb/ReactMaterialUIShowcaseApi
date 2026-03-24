namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class SiteProfileDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int Gender { get; set; } // Assuming Gender is an enum (int)
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Language { get; set; } // Assuming Language is an enum (int)
        public string Avatar { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }
}