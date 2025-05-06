namespace Asan_Campus.Model
{
   
        public class UserDevice
        {
            public int Id { get; set; }
            public string ExpoPushToken { get; set; }
            public int StudentId { get; set; }
            public Student Student { get; set; }
            public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        }
    
}
