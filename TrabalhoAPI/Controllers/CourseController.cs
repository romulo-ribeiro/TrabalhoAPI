using Context;
using Context.Enum;
using Context.Models;
using Context.Relations;
using Context.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace TrabalhoAPI.Controllers
{
    [ApiController]
    [Route("Course")]
    public class CourseController : ControllerBase
    {
        private readonly EscolaContext db;

        public CourseController(EscolaContext banco)
        {
            db = banco;
        }

        [HttpPost]
        [Route("AddCourse")]
        public ActionResult AddCourse(string course_name, CourseStatus status)
        {

            var result = new ResultData<Course>() { Error = true, Status = HttpStatusCode.BadRequest };
            Course course;
            try
            {
                if (Regex.IsMatch(course_name, @"[a-zA-Z\s]+"))
                {
                    course = new Course(course_name, status);
                }
                else
                {
                    throw new ArgumentException("Nome inválido");
                }
                using (db)
                {

                    if (db.Curso.Where(q => q.Name.ToLower() == course_name.ToLower()).Any())
                    {
                        throw new ArgumentException($"O nome {course.Name} já esta cadastrado");
                    }

                    db.Curso.Add(course);
                    db.SaveChanges();
                    result.Error = false;
                    result.Status = HttpStatusCode.OK;
                    result.Data = db.Curso.ToList();
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
        }


        [HttpPost]
        [Route("AddSubject")]
        public ActionResult AddSubject(string course_name, string subject_name)
        {
            var result = new ResultData<Course>() { Error = true, Status = HttpStatusCode.BadRequest };
            using (db)
            {
                try
                {
                    var cursos = db.Curso.ToList();
                    var materias = db.Materia.ToList();
                    var curso = cursos.Where(q => q.Name.ToLower() == course_name.ToLower()).Select(q => q).FirstOrDefault();
                    var materia = materias.Where(q => q.Name.ToLower() == subject_name.ToLower()).Select(q => q).FirstOrDefault();

                    if (curso == null || materia == null)
                    {
                        throw new ArgumentException("Item não encontrado");
                    }
                    if (materia.Status == SubjectStatus.Past || materia.Status == SubjectStatus.Discontinued)
                    {
                        throw new ArgumentException("Materia com Status 'INATIVO', não pode ser adicionada ao curso!");
                    }

                    curso.Enrollments.Add(new CourseSubject()
                    {
                        Subject = materia,
                        Course = curso
                    });

                    db.SaveChanges();

                    result.Error = false;
                    result.Message.Add("Matéria adicionada com sucesso");
                    result.Status = HttpStatusCode.OK;
                    result.Data = db.Curso.Include(q => q.Enrollments).ToList();
                    return Ok(result);

                }
                catch (Exception e)
                {
                    result.Error = true;
                    result.Message.Add(e.Message);
                    result.Status = HttpStatusCode.BadRequest;
                    return BadRequest(result);
                }
            }
        }

        [HttpPut]
        [Route("UpdateCourse")]
        public ActionResult UpdateCourse(int id, string new_name, int? new_status)
        {
            var result = new ResultData<Course>();
            try
            {
                using (db)
                {
                    var course = db.Curso.Where(q => q.IdCourse == id).FirstOrDefault();
                    course.Name = (new_name is null) ? course.Name : new_name;
                    course.Status = (new_status is null) ? course.Status : (CourseStatus)new_status;
                    result.Error = false;
                    result.Message.Add("OK");
                    result.Status = HttpStatusCode.OK;
                    db.SaveChanges();
                    return Ok(result);
                }

            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                result.Status = HttpStatusCode.BadRequest;
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetCourse")]
        public ActionResult GetCourses()
        {
            var result = new ResultData<Course>();
            try
            {
                using (db)
                {
                    result.Error = false;
                    result.Message.Add("OK");
                    result.Status = HttpStatusCode.OK;
                    result.Data = db.Curso.ToList();
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                result.Error = true;
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
        }
        [HttpDelete]
        [Route("DeleteCourse")]
        public ActionResult DeleteCourse(int id)
        {
            {
                var result = new ResultData<Course>();
                try
                {
                    using (db)
                    {
                        if (!db.Curso.Select(q => q.Enrollments).Any() && !db.Curso.Select(q => q.CourseContains).Any())
                        {
                            var course = db.Curso.Where(q => q.IdCourse == id).FirstOrDefault();
                            db.Curso.Remove(course);
                            db.SaveChanges();
                            result.Error = false;
                            result.Data = db.Curso.ToList();
                            result.Message.Add("OK");
                            result.Status = HttpStatusCode.OK;
                            return Ok(result);
                        }
                        throw new ArgumentException("O curso possui materias ou alunos");
                    }
                }
                catch (Exception e)
                {
                    result.Error = true;
                    result.Message.Add(e.Message);
                    return BadRequest(result);
                }
            }
        }
    }
}
