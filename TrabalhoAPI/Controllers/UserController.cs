﻿using Context;
using Context.Enum;
using Context.Models;
using Context.Relations;
using Context.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace TrabalhoAPI.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly EscolaContext db;

        public UserController(EscolaContext banco)
        {
            db = banco;
        }

        [HttpPost]
        [Route("AddStudent")]
        public ActionResult AddStudent(string name, string surname, string cpf, string dateStr)
        {

            var result = new ResultData<User>();

            try
            {
                UserServices.ValidateCpf(cpf);
                UserServices.ValidateBirthday(dateStr, out DateTime date);
                User user = new User(name, surname, cpf, Occupation.Student, date);
                HashSet<string> ids = new HashSet<string>();
                foreach (var userId in db.Usuario.Select(q => q.IdUser))
                {
                    ids.Add(userId);
                }
                foreach (var item in db.Usuario.Where(q => q.Role == Occupation.Student))
                {
                    if (item.Cpf == user.Cpf)
                    {
                        result.Error = true;
                        result.Message.Add($"O cpf {user.Cpf} já esta cadastrado");
                        result.Status = HttpStatusCode.BadRequest;
                        return BadRequest(result);
                    }
                }
                UserServices.CreateId(user, ids);
                db.Usuario.Add(user);
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Usuario.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpPost]
        [Route("AddStaff")]
        public ActionResult AddStaff(string name, string surname, string cpf, Occupation role)
        {
            User user = new User(name, surname, cpf, role);
            var result = new ResultData<User>();

            try
            {
                UserServices.ValidateCpf(cpf);
                HashSet<string> ids = new HashSet<string>();
                foreach (var userId in db.Usuario.Select(q => q.IdUser))
                {
                    ids.Add(userId);
                }
                foreach (var item in db.Usuario.Where(q => q.Role != Occupation.Student))
                {
                    if (item.Cpf == user.Cpf)
                    {
                        result.Error = true;
                        result.Message.Add($"O cpf {user.Cpf} já esta cadastrado");
                        result.Status = HttpStatusCode.BadRequest;
                        return BadRequest(result);
                    }
                }
                UserServices.CreateId(user, ids);
                db.Usuario.Add(user);
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Usuario.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpPut]
        [Route("UpdateStudent")]
        public ActionResult UpdateStudent(string id, string new_name, string new_surname, string new_cpf, string new_dateStr)
        {
            var result = new ResultData<User>();
            try
            {
                UserServices.ValidateCpf(new_dateStr);
                UserServices.ValidateBirthday(new_dateStr, out DateTime date);
                var user = db.Usuario.Where(q => q.IdUser == id).FirstOrDefault();

                user.Name = (new_name is null) ? user.Name : new_name;
                user.Surname = (new_surname is null) ? user.Surname : new_surname;
                user.Cpf = (new_cpf is null) ? user.Cpf : new_cpf;
                user.Birthday = (new_dateStr is null) ? user.Birthday : date;

                result.Error = false;
                result.Message.Add("OK");
                result.Status = HttpStatusCode.OK;
                db.SaveChanges();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                result.Status = HttpStatusCode.BadRequest;
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpPut]
        [Route("UpdateStaff")]
        public ActionResult UpdateStaff(string id, string new_name, string new_surname, string new_cpf, int? new_role)
        {
            var result = new ResultData<User>();
            try
            {
                UserServices.ValidateCpf(new_cpf);
                var user = db.Usuario.Where(q => q.IdUser == id).FirstOrDefault();
                user.Name = (new_name is null) ? user.Name : new_name;
                user.Surname = (new_surname is null) ? user.Surname : new_surname;
                user.Cpf = (new_cpf is null) ? user.Cpf : new_cpf;
                user.Role = (new_role is null) ? user.Role : (Occupation)new_role;
                result.Error = false;
                result.Message.Add("OK");
                result.Status = HttpStatusCode.OK;
                db.SaveChanges();
                return Ok(result);

            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                result.Status = HttpStatusCode.BadRequest;
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public ActionResult GetUsers()
        {
            var result = new ResultData<User>();
            try
            {
                result.Error = false;
                result.Message.Add("OK");
                result.Status = HttpStatusCode.OK;
                result.Data = db.Usuario.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete]
        [Route("DeleteUser")]
        public ActionResult DeleteUser(string id)
        {

            var result = new ResultData<User>();
            try
            {

                var user = db.Usuario.Where(q => q.IdUser == id).FirstOrDefault();
                db.Usuario.Remove(user);
                db.SaveChanges();
                result.Error = false;
                result.Data = db.Usuario.ToList();
                result.Message.Add("OK");
                result.Status = HttpStatusCode.OK;
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }

        }
    }
}
