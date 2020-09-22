using Context.Enum;
using Context.Relations;
using System;
using System.Collections.Generic;

namespace Context.Models
{
    public class Subject
    {
        public int IdSubject { get; private set; }
        public string Name { get; set; }
        public DateTime Registry { get; set; }
        public SubjectStatus Status { get; set; }

        public Subject(string name, DateTime registry, SubjectStatus status)
        {
            Name = name;
            Registry = registry;
            Status = status;
        }

        public virtual ICollection<UserSubject> Allocation { get; set; } = new HashSet<UserSubject>();
        public virtual ICollection<CourseSubject> Enrollment { get; set; } = new HashSet<CourseSubject>();

    }
}
