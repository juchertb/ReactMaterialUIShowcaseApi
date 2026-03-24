namespace ReactMaterialUIShowcaseApi.Entities
{
    public class SchedulerEventCategory
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public string ChipColor { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public ICollection<SchedulerEvent> SchedulerEvents { get; set; } = new List<SchedulerEvent>();
    }
}
