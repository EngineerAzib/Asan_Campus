using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Event
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ImageUrl { get; set; }
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set;}
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set;}
        public bool _isActive { get; set; }

       
        public int EventCategoryId { get; set; }
        [ForeignKey("EventCategoryId")]
        public EventCategory EventCategory { get; set; } 



    }
}
