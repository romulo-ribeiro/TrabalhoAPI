using Context.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Context.Services
{
    public class UserServices
    {
        public static void CreateId(User user, HashSet<string> ids)
        {
            Random rand = new Random();
            bool validId;
            string IdUser;
            do
            {
                IdUser = rand.Next(1000, 9999).ToString();
                validId = ids.Add(IdUser);
            } while (!validId);
            user.IdUser = IdUser;
        }

        public static void CreatePassword(User user, string password)
        {
            if (Regex.IsMatch(password, @"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"))
            {
                //var hash = password.GetHashCode(); //TODO: Save password as hashcode
                //user.Password = hash.ToString();
                user.Password = password;
                return;
            }
            throw new ArgumentException("Senha deve conter:" +
                "\n-No mínimo 8 caracteres" +
                "\n-Letras maiúsculas e minúsculas" +
                "\n-Caracteres especiais" +
                "\n-Dígitos");
        }

        public static bool Access(User user, string id, string password)
        {
            //var hash = password.GetHashCode(); //TODO: Check password as hashcode
            //return id == user.IdUser && hash.ToString() == user.Password;
            return id == user.IdUser && password == user.Password;
        }

        public static bool ValidateCpf(string cpf)
        {
            //Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$");
            if (!Regex.IsMatch(cpf, @"^[0-9]{11}"))
            {
                throw new ArgumentException("Cpf inválido");
            }
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = (resto < 2) ? 0 : 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();
            bool isValid = cpf.EndsWith(digito);
            if (!isValid)
            {
                throw new ArgumentException("Cpf Inválido");
            }
            return isValid;
        }
        public static bool ValidateBirthday(string dateStr, out DateTime date)
        {
            if (!Regex.IsMatch(dateStr, @"^[0-9]{2}/[0-9]{2}/[0-9]{4}"))
            {
                throw new ArgumentException("Formato inválido");
            }
            bool isValid = DateTime.TryParse(dateStr, out DateTime result);
            if (isValid && result < new DateTime(2002, 1, 1))
            {
                date = result;
                return isValid;
            }
            throw new ArgumentException("Data inválida");
        }
    }
}
