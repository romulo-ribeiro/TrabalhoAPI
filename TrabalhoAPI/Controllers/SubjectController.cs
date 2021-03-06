﻿using Context;
using Context.Enum;
using Context.Models;
using Context.Relations;
using Context.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace TrabalhoAPI.Controllers
{
    [ApiController]
    [Route("Subject")]
    public class SubjectController : ControllerBase
    {
        private readonly EscolaContext db;

        public SubjectController(EscolaContext database)
        {
            db = database;
        }

        [HttpPost]
        [Route("AddSubject")]
        public ActionResult AddSubject(string subject_name, string dateStr, SubjectStatus status)
        {
            var result = new ResultData<Subject>();

            try
            {
                SubjectServices.ValidateName(subject_name);
                SubjectServices.ValidateDate(dateStr, out DateTime date);

                if (db.Materia.Select(q => q.Name).Contains(subject_name))
                {
                    result.Error = true;
                    result.Message.Add($"O nome {subject_name} já esta cadastrado");
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }

                Subject subject = new Subject(subject_name, date, status);
                db.Materia.Add(subject);
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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
        [Route("AddStudent")]
        public ActionResult AddStudent(int idSubject, string idStudent)
        {
            var result = new ResultData<Subject>();

            try
            {
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var student = db.Usuario.Where(q => q.IdUser == idStudent).FirstOrDefault();
                if (subject.Allocation.Select(q => q.IdUser == student.IdUser).Any())
                {
                    result.Error = true;
                    result.Message.Add($"O nome {student.Name} já esta cadastrado");
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }

                subject.Allocation.Add(new UserSubject(idStudent, idSubject));
                db.SaveChanges();

                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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
        [Route("AddProfessor")]
        public ActionResult AddProfessor(int idSubject, string idProfessor)
        {
            var result = new ResultData<Subject>();

            try
            {
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var professor = db.Usuario.Where(q => q.IdUser == idProfessor).FirstOrDefault();
                if (subject.Allocation.Select(q => q.IdUser == professor.IdUser).Any())
                {
                    result.Error = true;
                    result.Message.Add($"O nome {professor.Name} já esta cadastrado");
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }

                subject.Allocation.Add(new UserSubject(idProfessor, idSubject));
                db.SaveChanges();

                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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
        [Route("AddGrade")]
        public ActionResult AddGrade(int idSubject, string idStudent, int grade)
        {
            var result = new ResultData<Subject>();

            try
            {
                SubjectServices.ValidateGrade(grade);
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var student = subject.Allocation.Where(q => q.IdSubject == idSubject && q.IdUser == idStudent).FirstOrDefault();
                if (student is null)
                {
                    result.Error = true;
                    result.Message.Add($"O aluno não existe");
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }
                
                student.Grade = grade;
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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
        [Route("UpdateSubject")]
        public ActionResult UpdateSubject(int id, string new_name, string new_dateStr, int? new_status)
        {
            var result = new ResultData<Subject>();
            try
            {
                SubjectServices.ValidateName(new_name);
                SubjectServices.ValidateDate(new_dateStr, out DateTime date);

                var subject = db.Materia.Where(q => q.IdSubject == id).FirstOrDefault();
                subject.Name = (new_name is null) ? subject.Name : new_name;
                subject.Registry = (new_dateStr is null) ? subject.Registry : date;
                subject.Status = (new_status is null) ? subject.Status : (SubjectStatus)new_status;
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
        [Route("UpdateGrade")]
        public ActionResult UpdateGrade(int idSubject, string idStudent, int grade)
        {
            var result = new ResultData<Subject>();

            try
            {
                SubjectServices.ValidateGrade(grade);
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var studentGrade = subject.Allocation.Where(q => q.IdSubject == idSubject && q.IdUser == idStudent).FirstOrDefault().Grade;
                if (studentGrade is null)
                {
                    result.Error = true;
                    result.Message.Add($"O aluno não possui nota para atualizar");
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }
                studentGrade = grade;
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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

        [HttpGet]
        [Route("GetSubjects")]
        public ActionResult GetSubjects()
        {
            var result = new ResultData<Subject>();
            try
            {
                result.Error = false;
                result.Message.Add("OK");
                result.Status = HttpStatusCode.OK;
                result.Data = db.Materia.ToList();
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
        [Route("DeleteSubject")]
        public ActionResult DeleteSubject(int id)
        {

            var result = new ResultData<Subject>();
            try
            {
                var subject = db.Materia.Where(q => q.IdSubject == id).FirstOrDefault();
                db.Materia.Remove(subject);
                db.SaveChanges();
                result.Error = false;
                result.Data = db.Materia.ToList();
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
        [HttpDelete]
        [Route("DeleteStudent")]
        public ActionResult DeleteStudent(int idSubject, string idStudent)
        {

            var result = new ResultData<Subject>();
            try
            {
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var student = subject.Allocation.Where(q => q.IdUser == idStudent).FirstOrDefault();
                subject.Allocation.Remove(student);
                db.SaveChanges();
                result.Error = false;
                result.Data = db.Materia.ToList();
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
        [HttpDelete]
        [Route("DeleteProfessor")]
        public ActionResult DeleteProfessor(int idSubject, string idProfessor)
        {

            var result = new ResultData<Subject>();
            try
            {
                var subject = db.Materia.Where(q => q.IdSubject == idSubject).FirstOrDefault();
                var professor = subject.Allocation.Where(q => q.IdUser == idProfessor).FirstOrDefault();
                subject.Allocation.Remove(professor);
                db.SaveChanges();
                result.Error = false;
                result.Data = db.Materia.ToList();
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

