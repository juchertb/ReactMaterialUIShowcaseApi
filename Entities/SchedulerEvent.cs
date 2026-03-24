using System;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class SchedulerEvent
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }
        public string Date { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string StartHour { get; set; } = string.Empty;

        public SchedulerEventCategory? Category { get; set; }
        public int CategoryId { get; set; }

        public string Organizer { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;

        public bool IsAllDay { get; set; }
        public bool IsRepeated { get; set; }

        public string RepeatInterval { get; set; } = string.Empty;
        public int RepeatEvery { get; set; }
        public int RepeatOnWeekday { get; set; }

        public string RepeatEnd { get; set; } = string.Empty;
        public int RepeatEndOn { get; set; }
        public string RepeatEndAfter { get; set; } = string.Empty;
    }
}