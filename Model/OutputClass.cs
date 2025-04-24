namespace Asan_Campus.Model
{
    public class OutputClass
    {
        public class GenderStats
        {
            public decimal Percentage { get; set; }
            public int Count { get; set; }
        }

        public class DepartmentList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int TotalStudents { get; set; }
            public object GenderStats { get; set; }
        }


    }
}
