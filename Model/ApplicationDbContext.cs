using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asan_Campus.Model
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DepartmentSemester> departmentSemesters { get; set; }  
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<AcadmicDetail>AcadmicDetails { get; set; }
        public DbSet<Teacher> Teachers { get; set; }    
        public DbSet<Student> Students { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<CourseAttendance> CourseAttendances { get;set ; }  
        public DbSet<Feedback>Feedbacks { get; set; }
        public DbSet<Schedule>Schedules { get; set; }
        public DbSet<ExamSchedule>ExamSchedules { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<SemesterRegistration> SemesterRegistration { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure self-referencing many-to-many for prerequisites
            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.PrerequisiteCourse)
                .WithMany(c => c.RequiredFor)
                .HasForeignKey(p => p.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DepartmentSemester>()
       .HasKey(ds => new { ds.DepartmentId, ds.SemesterId,ds.CourseId }); // Composite key

            // Define relationships
            modelBuilder.Entity<DepartmentSemester>()
                .HasOne(ds => ds.Department)
                .WithMany(d => d.DepartmentSemesters)
                .HasForeignKey(ds => ds.DepartmentId);

            modelBuilder.Entity<DepartmentSemester>()
                .HasOne(ds => ds.Semester)
                .WithMany(s => s.DepartmentSemesters)
                .HasForeignKey(ds => ds.SemesterId);
            modelBuilder.Entity<DepartmentSemester>()
              .HasOne(ds => ds.Course)
              .WithMany(s => s.DepartmentSemesters)
              .HasForeignKey(ds => ds.CourseId);
        }
    }
}
