using Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Context.Tools
{
    public class Connection<T>
    {
        private EscolaContext dbConnection;

        public bool AddCourse(Course course)
        {
            using (dbConnection = new EscolaContext())
            {
                var retorno = GetCourse(course);
                if (!retorno)
                {
                    dbConnection.Curso.Add(course);
                    dbConnection.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GetCourse(Course course)
        {
            return dbConnection.Curso.Where(q => q.IdCourse == course.IdCourse).Any();
        }
    }
}
