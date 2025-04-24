namespace Asan_Campus.Model
{
    public class Notification
    {
        public int Id { get; set; } // 

        public bool IsRead { get; set; }

        public string Message { get; set; }

        public string Priority { get; set; }

        public DateTime Timestamp { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }

}
