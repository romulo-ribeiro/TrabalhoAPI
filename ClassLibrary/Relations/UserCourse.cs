using Context.Models;

namespace Context.Relations
{
    public class UserCourse
    {
        public string IdUser { get; set; }
        public int IdCourse { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
