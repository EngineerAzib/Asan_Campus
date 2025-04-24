namespace Asan_Campus.Model
{
    public class Semester
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public ICollection<DepartmentSemester> DepartmentSemesters { get; set; }
    }
}
