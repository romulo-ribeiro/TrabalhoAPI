using Context.Models;

namespace Context.Relations
{
    public class CourseSubject
    {
        public int Id { get; set; }
        public int IdCourse { get; set; }
        public int IdSubject { get; set; }

        public virtual Course Course { get; set; }
        public virtual Subject Subject { get; set; }

        public CourseSubject(int idCourse, int idSubject)
        {
            IdCourse = idCourse;
            IdSubject = idSubject;
        }
    }
}
