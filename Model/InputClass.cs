using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class InputClass
    {
        public class AddCourseRequest
        {
            public string Name { get; set; }
            public string InitialName { get; set; }
            public bool isElective { get; set; }
            public int Credits { get; set; }
            public int DepartmentId { get; set; }
            public int SemesterId { get; set; }
            public int teacherId { get; set; }
         
            public string? day { get; set; }
            public List<int> PrerequisiteIds { get; set; }
           
            public string type { get; set; }
        }
        public class AddStudent
        {
           
            [EmailAddress]
            public string Email { get; set; }
          
            
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public int Id { get; set; }

            public string Name { get; set; }
            public string Father_Name { get; set; }
            public string CNIC { get; set; }
            public string Phone { get; set; }
            public string Address {  get; set; }    
            public string DOB { get; set; }
            public string Gender { get; set; }
            public string RollNo { get; set; }
            public int DepartmentId { get; set; }
           
      
        }

        public class DateDto
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }

        public class TimeDto
        {
            public int Hour { get; set; }
            public int Minute { get; set; }
        }

        public class AddNew
        {
            public string Title { get; set; }
            public DateDto? PublishedDate { get; set; } // Custom object for date
            public TimeDto? PublishedTime { get; set; } // Custom object for time
            public string Description { get; set; }
          
            public int categoryId { get; set; }
        }
        public class AddInternship
        {

         
            public string Title { get; set; }

            
            public string CompanyName { get; set; }

            public string? Description { get; set; } // Nullable string

            
            public string? Location { get; set; } // Nullable string

        
            public string? Duration { get; set; } // Nullable string

         
            public DateTime ApplicationDeadline { get; set; }

            public int Stippend { get; set; }
            public int Position { get; set; }
            public int Year { get; set; }
            public int departmentId { get; set; }


          

        }
        public class AddEvent()
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
           
            public DateDto StartDate { get; set; }

            public DateDto EndDate { get; set; }
            public TimeDto StartTime { get; set; }
            public TimeDto EndTime { get; set; }
       
            public int EventCategoryId { get; set; }
        }
        public class DepartmentSemester()
        {
            public Department department { get; set; }
            public Semester semester { get; set;}
        }

        public class CourseAttendance()
        {
            public double overallAttendance { get; set; }
            public CourseAttendance CourseAttendances { get; set; } 
        }
        public class CourseAttendanceDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int CreditHours { get; set; }
            public int TotalClasses { get; set; }
            public int AttendedClasses { get; set; }
            public double Percentage { get; set; }
        }

        public class SemesterAttendanceDto
        {
            public double OverallAttendance { get; set; }
            public List<CourseAttendanceDto> Courses { get; set; }
        }

        public class AttendanceDto
        {
            public SemesterAttendanceDto CurrentSemester { get; set; }
        }

        public class DepartmentDT0
        {
            public int Id { get; set; }
            public string DepartmentName { get; set; }
            public string? DepartmentCode { get; set; }
            public int FemaleStudent { get; set; }
            public int MaleStudent { get; set; }
        
        }
        public class AddExamSchedule()
        {
            public DateTime ExamDate { get; set; }

           
            public string ExamDay { get; set; }

            // Foreign Key
            public int YearId { get; set; }
           

            public int DepartmentID { get; set; }
          
       
            public int SemesterID { get; set; }
           
            public List<examslot> Examslots { get; set; }

           
            
           




        }

        public class examslot
        {
            public string TimeSlot { get; set; }
            public int CourseId { get; set; }
            public string Venue { get; set; }
        }
        public class AddTeacher
        {
            public string Email { get; set; }


            [DataType(DataType.Password)]
            public string Password { get; set; }
            public string Name { get; set; }
            public int DepartmentID { get; set; }
            
            public string? designation { get; set; }
            public string? expertise { get; set; }
          
            
            public string? gender { get; set; }
           
          
        }
        public class AddAcedemic
        {
           
            public int studentId { get; set; }
            public int semesterId { get; set; }
            public List<AdedmicDetails> AcadmicDetail { get; set;}
           
        }
        public class AdedmicDetails
        {
            
            public int courseId { get; set; }
            public double gpa { get; set; }
        }

        public class AddAttendance
        {
            public List<AttendanceRecords> attendanceRecords { get; set; }
            public int semesterId { get; set; }
            public int studentId { get; set; }
            
        }
        public class AttendanceRecords
        {
            public int attendedClasses { get; set;}
            public int courseId { get; set;}
            public int totalClasses { get; set; }
        }

        public class CreateRegistration
        {
           
            public int MaxStudent { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public int DepartmentID { get; set; }
           
            public int SemesterID { get; set; }
           
            public int CourseId { get; set; }
        }
        public class AddTeacherSchedule
        {
            public int TeacherId { get; set; }
          
            public string Day { get; set; }
            public string TimeSlot { get; set; }
            public int DepartmentID { get; set; }
          
            public string Section { get; set; }
        }
        public class AddTeacherAttendance
        {
            public int TeacherId { get; set; }
           
            public int CourseId { get; set; }
        
            public int TotalClasses { get; set; }
            public int ClassesTaken { get; set; }
            public int DepartmentID { get; set; }
          
            public string Section { get; set; }
        }

        public class AddNotification
        {
           

      

            public string Message { get; set; }

           

            public DateTime Timestamp { get; set; }

            public string Title { get; set; }

            public string Type { get; set; }
            public int semesterId { get; set; }
            public int departmentId { get; set; }
            public string date { get; set; }
        }
        public class DeviceRegistrationDto
        {
            public string userId { get; set; }
            public string ExpoPushToken { get; set; }
        }
        public class AddReg
        {
            
          

           public List<string> courses { get; set; }
            public string semester { get; set; }
           
           
          
        }


    }
}
