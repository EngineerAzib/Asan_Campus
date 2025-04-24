using Asan_Campus.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asan_Campus.Service
{
    public class ImplementationSevices
    {

        private readonly ApplicationDbContext _context;
        public ImplementationSevices(ApplicationDbContext context)
        {
            _context = context;   
        }
        public decimal getPercentage(int student,int totalStudent)
        {
            if (totalStudent == 0) return 0; // To prevent division by zero.

            decimal res = (student * 100m) / totalStudent; // Use 'm' to specify a decimal literal
            return Math.Round(res, 1); // Round to 1 decimal place

        }
        public dynamic GetTeacher(int teacherId)
        {
            var res = _context.Teachers
                .Where(x => x.Id == teacherId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.registrationNo,
                    x.designation,
                    Status = x.status==true ? "Active" : "Inactive", // Fixed ternary operator and added key
                    x.gender
                })
                .FirstOrDefault();

            return res;
        }
        public dynamic GetSchedule(int TeacherId)
        {
            var schedules = _context.Schedules
                .Where(x => x.TeacherId == TeacherId)
                .Select(x => new
                {
                    x.Day,  // Assuming there's a column for day
                    Time = x.TimeSlot,
                    Department = x.Department.DepartmentName,
                    x.Section
                })
                .ToList();

            var groupedSchedule = schedules
                .GroupBy(x => x.Day)
                .ToDictionary(
                    g => g.Key.ToLower(), // Convert keys to lowercase for consistency
                    g => g.Select(x => new
                    {
                        time = x.Time,
                        department = x.Department,
                        section = x.Section
                    }).ToList()
                );

           
            return groupedSchedule ;
        }
        public dynamic GetcourseAttendanse(int teacherId)
        {
            var res = _context.CourseAttendances.Where(x => x.TeacherId == teacherId).Select(
                x => new
                {
                    x.Section,
                    x.Id,
                    code = x.Course.InitialName,
                    name = x.Course.Name,
                    x.TotalClasses,
                    x.ClassesTaken,
                    department = x.Department.DepartmentName,
                }).ToList();
            return res;
        }
        public dynamic GetFeedback(int teacherId)
        {
            var res = _context.Feedbacks
                .Where(x => x.TeacherId == teacherId)
                .Select(x => new
                {
                    Course = new  
                    {
                        code = x.Course.InitialName,
                        name = x.Course.Name
                    },
                    x.Id,
                    x.Section,
                    x.Rating,
                    x.TeachingRating,
                    x.KnowledgeRating,
                    x.CommunicationRating
                })
                .ToList();  
            return res;  
        }

        public dynamic GetStudentBasicInfo(int stdid)
        {
            var res = _context.Students.Where(x => x.Id == stdid).Select(x => new
            {
                FullName = x.Name + x.Father_Name,
                x.Id,
                x.EndrollementNo,
                x.RollNo,
                x.Phone,
                x.Semester,
                Department = x.Department.DepartmentName,
                x.User.Email,
                password = "********************"


            }).FirstOrDefault();
            return res;
        }

        public dynamic GetAcademicDetail(int stdId)
        {
            var academicDetails = _context.AcadmicDetails
                .Where(x => x.studentId == stdId)
                .Include(x => x.semester)
                .Include(x => x.students)
                .Include(x => x.course)
                .ToList();

            if (!academicDetails.Any()) // Check if academicDetails is empty
            {
                return new { currentCGPA = 0.0, semesterGPAs = new List<object>() };
            }

            var groupedData = academicDetails
                .GroupBy(x => x.semester.SemesterName)
                .Select(g => new
                {
                    semester = g.Key,
                    gpa = g.Any() ? g.Average(x => x.gpa) : 0.0, // Handle empty groups
                    courses = g.Select(x => new
                    {
                        code = x.course.InitialName,
                        name = x.course.Name,
                        creditHours = x.course.Credits,
                        grade = x.grade
                    }).ToList()
                })
                .ToList();

            var response = new
            {
                currentCGPA = academicDetails.Any() ? academicDetails.Average(x => x.gpa) : 0.0, // Handle empty list
                semesterGPAs = groupedData
            };

            return response;
        }


        public dynamic CourseAttendance(int stdId)
        {
            var attendanceData = _context.StudentAttendances
                .Where(sa => sa.StudentID == stdId)
                .Include(sa => sa.Course)
                .Include(sa => sa.Semester)
                .GroupBy(sa => sa.CourseID) // Group by semester
                .Select(group => new
                {
                    CurrentSemester = new
                    {
                        SemesterID = group.Key,
                        OverallAttendance = group.Sum(sa => sa.AttendedClasses) * 100.0 / group.Sum(sa => sa.TotalClasses),
                        Courses = group.Select(sa => new
                        {
                            sa.Course.Id,
                            sa.Course.InitialName,
                            sa.Course.Name,
                            sa.TotalClasses,
                            sa.AttendedClasses,
                            Percentage = sa.TotalClasses == 0 ? 0 : (double)sa.AttendedClasses / sa.TotalClasses * 100
                        }).ToList()
                    }
                })
                .OrderByDescending(s => s.CurrentSemester.SemesterID) // Get the latest semester
                .FirstOrDefault();

            return attendanceData ;
        }
        public string Grade(double gpa)
        {
            if (gpa >= 2.0 && gpa <= 2.4)
                return "E";
            else if (gpa > 2.4 && gpa <= 2.7)
                return "D";
            else if (gpa > 2.7 && gpa <= 3.0)
                return "C";
            else if (gpa > 3.0 && gpa <= 3.3)
                return "B";
            else if (gpa > 3.3 && gpa <= 3.7)
                return "A";
            else if (gpa > 3.7 && gpa <= 4.0)
                return "A+";
            else
                return "F"; // Default case for GPAs below 2.0
        }



        public bool Icomplete(double gpa)
        {
            if (gpa >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }





    }
}
