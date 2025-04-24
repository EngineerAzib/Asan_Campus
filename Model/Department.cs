namespace Asan_Campus.Model
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public int FemaleStudent {  get; set; }
        public int MaleStudent {  get; set; }
        public ICollection<DepartmentSemester> DepartmentSemesters { get; set; }
    }
}
