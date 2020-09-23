using Context.Enum;
using Context.Relations;
using System;
using System.Collections.Generic;

namespace Context.Models
{
    public class User
    {
        public static HashSet<string> Ids = new HashSet<string>();

        public string IdUser { get;  set; }
        public string Password { get;  set; }
        public string Name { get; set; }
        public string Surname { get;  set; }
        public string Cpf { get; set; }
        public Occupation Role { get; set; }
        public DateTime? Birthday { get;  set; }

        public User(string name, string surname, string cpf, Occupation role, DateTime birthday)
        {
            Name = name;
            Surname = surname;
            Cpf = cpf;
            Role = role;
            Birthday = birthday;
        }

        public User(string name, string surname, string cpf, Occupation role)
        {
            Name = name;
            Surname = surname;
            Cpf = cpf;
            Role = role;
        }

        public virtual ICollection<UserCourse> InCourse { get; set; } = new HashSet<UserCourse>();
        public virtual ICollection<UserSubject> Registered { get; set; } = new HashSet<UserSubject>();

    }
}
