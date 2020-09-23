using System;
using System.Text.RegularExpressions;

namespace Context.Services
{
    public class SubjectServices
    {
        public static bool ValidateDate(string dateStr, out DateTime date)
        {
            if (!Regex.IsMatch(dateStr, @"^[0-9]{2}/[0-9]{2}/[0-9]{4}"))
            {
                throw new ArgumentException("Formato inválido");
            }
            bool isValid = DateTime.TryParse(dateStr, out DateTime result);
            if (isValid && result < DateTime.Now)
            {
                date = result;
                return isValid;
            }
            throw new ArgumentException("Data inválida");
        }

        public static bool ValidateName(string subject_name)
        {
            if (Regex.IsMatch(subject_name, @"[a-zA-Z\s]+"))
            {
                return true;
            }
            throw new ArgumentException("Nome de matéria inválido");
        }
        public static bool ValidateGrade(int grade)
        {
            if (grade >= 0 && grade <= 100)
                return true;
            throw new ArgumentException("Nota inválido");
        }
    }
}
