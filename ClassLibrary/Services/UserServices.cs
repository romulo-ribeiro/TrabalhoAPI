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
            throw new ArgumentException("Senha fraca");
        }

        public static bool Access(User user, string id, string password)
        {
            //var hash = password.GetHashCode(); //TODO: Check password as hashcode
            //return id == user.IdUser && hash.ToString() == user.Password;
            return id == user.IdUser && password == user.Password;
        }
    }
}
