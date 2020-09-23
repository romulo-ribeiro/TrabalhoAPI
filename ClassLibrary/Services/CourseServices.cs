using System;
using System.Text.RegularExpressions;

namespace Context.Services
{
    public class CourseServices
    {
        public static bool ValidateName(string course_name)
        {
            if(Regex.IsMatch(course_name, @"[a-zA-Z\s]+"))
            {
                return true;
            }
            throw new ArgumentException("Nome de curso inválido");
        }
    }
}
