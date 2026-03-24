using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class SchedulerEventDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Date { get; set; } = string.Empty; // Formatted date string (yyyy-MM-dd)
        public string Title { get; set; } = string.Empty;
        public string StartHour { get; set; } = string.Empty; // Formatted time string (HH:mm)
        public SchedulerEventCategoryDto? Category { get; set; }
        public int CategoryId { get; set; }
        public string Organizer { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public bool IsAllDay { get; set; }
        public bool IsRepeated { get; set; }
        public string RepeatInterval { get; set; } = string.Empty; // "Daily", "Weekly", etc.
        public int RepeatEvery { get; set; }
        public int RepeatOnWeekday { get; set; }
        public string RepeatEnd { get; set; } = string.Empty; // "never", "on", "after"
        public int RepeatEndOn { get; set; }
        public string RepeatEndAfter { get; set; } = string.Empty; // Formatted date and time string
    }
}
