namespace ReactMaterialUIShowcaseApi.Entities
{
    public class SiteProfileTag
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;

        public int SiteProfileId { get; set; }
        public SiteProfile SiteProfile { get; set; } = null!;
    }
}