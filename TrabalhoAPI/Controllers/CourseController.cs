using Context;
using Context.Enum;
using Context.Models;
using Context.Relations;
using Context.Services;
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
            try
            {
                CourseServices.ValidateName(course_name);
                Course course = new Course(course_name, status);
                if (db.Curso.Where(q => q.Name.ToLower() == course_name.ToLower()).Any())
                    throw new ArgumentException($"O nome {course.Name} já esta cadastrado");

                db.Curso.Add(course);
                db.SaveChanges();
                result.Error = false;
                result.Status = HttpStatusCode.OK;
                result.Data = db.Curso.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
            finally
            {
                db.Dispose();
            }
        }


        [HttpPost]
        [Route("AddSubject")]
        public ActionResult AddSubject(int courseId, int subjectId)
        {
            var result = new ResultData<Course>() { Error = true, Status = HttpStatusCode.BadRequest };

            try
            {
                var course = db.Curso.Where(q => q.IdCourse == courseId).FirstOrDefault();
                var subject = db.Materia.Where(q => q.IdSubject == subjectId).FirstOrDefault();

                if (course == null || subject == null)
                {
                    throw new ArgumentException("Item não encontrado");
                }
                if (subject.Status == SubjectStatus.Inactive)
                {
                    throw new ArgumentException("Materia com Status 'INATIVO' não pode ser adicionada ao curso.");
                }

                course.Enrollments.Add(new CourseSubject(courseId,subjectId));

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
            finally
            {
                db.Dispose();
            }

        }

        [HttpPost]
        [Route("AddStudent")]
        public ActionResult AddStudent(int courseId, string studentId)
        {
            var result = new ResultData<Course>() { Error = true, Status = HttpStatusCode.BadRequest };
            using (db)
            {
                try
                {
                    var course = db.Curso.Where(q => q.IdCourse == courseId).FirstOrDefault();
                    var student = db.Usuario.Where(q => q.IdUser == studentId && q.Role == Occupation.Student).FirstOrDefault();

                    if (course == null || student == null)
                    {
                        throw new ArgumentException("Item não encontrado");
                    }
                    if (course.Status == CourseStatus.Inactive)
                    {
                        throw new ArgumentException("Curso com Status 'INATIVO' não pode receber novos alunos.");
                    }

                    course.CourseContains.Add(new UserCourse(studentId,courseId));
                    db.SaveChanges();

                    result.Error = false;
                    result.Message.Add("Aluno adicionado com sucesso");
                    result.Status = HttpStatusCode.OK;
                    result.Data = db.Curso.Include(q => q.CourseContains).ToList();
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
                    if (!db.Curso.Where(q => q.IdCourse == id).Any())
                    {
                        throw new ArgumentException("O curso não existe");
                    }
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

        [HttpDelete]
        [Route("DeleteSubject")]
        public ActionResult DeleteSubject(int id, int subjectId)
        {
            {
                var result = new ResultData<Course>();
                try
                {
                    var subject = db.Curso.SelectMany(q => q.Enrollments).Where(q => q.IdSubject == subjectId).FirstOrDefault();
                    var hasStudents = db.Materia.SelectMany(q => q.Allocation).Where(q => q.IdSubject == subjectId).Any();
                    if (subject is null)
                    {
                        throw new ArgumentException("A materia não existe");
                    }
                    if (!hasStudents)
                    {
                        var course = db.Curso.Where(q => q.IdCourse == id).Select(q => q.Enrollments).FirstOrDefault();
                        course.Remove(subject);
                        db.SaveChanges();
                        result.Error = false;
                        result.Data = db.Curso.ToList();
                        result.Message.Add("OK");
                        result.Status = HttpStatusCode.OK;
                        return Ok(result);
                    }
                    throw new ArgumentException("A materia possui alunos");
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
        [HttpDelete]
        [Route("DeleteStudent")]
        public ActionResult DeleteStudent(int id, string studentId)
        {

            var result = new ResultData<Course>();
            try
            {
                var student = db.Curso.SelectMany(q => q.CourseContains).Where(q => q.User.Role == Occupation.Student && q.IdUser == studentId).FirstOrDefault();
                if (student is null)
                {
                    throw new ArgumentException("O aluno não existe");
                }
                if (!db.Curso.Select(q => q.Enrollments).Any() && !db.Curso.Select(q => q.CourseContains).Any())
                {
                    var course = db.Curso.Where(q => q.IdCourse == id).Select(q => q.CourseContains).FirstOrDefault();
                    course.Remove(student);
                    db.SaveChanges();
                    result.Error = false;
                    result.Data = db.Curso.ToList();
                    result.Message.Add("OK");
                    result.Status = HttpStatusCode.OK;
                    return Ok(result);
                }
                throw new ArgumentException("O curso possui materias ou alunos");
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
