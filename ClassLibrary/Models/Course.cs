using Context.Enum;
using Context.Relations;
using System.Collections.Generic;

namespace Context.Models
{
    public class Course
    {
        public int IdCourse { get; protected set; }
        public string Name { get; set; }
        public CourseStatus Status { get; set; }

        public Course(string name, CourseStatus status)
        {
            Name = name;
            Status = status;
        }

        public virtual ICollection<CourseSubject> Enrollments { get; set; } = new HashSet<CourseSubject>();
        public virtual ICollection<UserCourse> CourseContains { get; set; } = new HashSet<UserCourse>();
    }
}
