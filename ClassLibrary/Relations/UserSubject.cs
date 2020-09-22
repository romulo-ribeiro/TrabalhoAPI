using Context.Models;

namespace Context.Relations
{
    public class UserSubject
    {
        public string IdUser { get; set; }
        public int IdSubject { get; set; }
        public int? Grade { get; set; }

        public virtual User User { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
