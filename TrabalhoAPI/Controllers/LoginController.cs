using Context;
using Context.Models;
using Context.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace TrabalhoAPI.Controllers
{

    [ApiController]
    [Route("Login")]
    public class LoginController : ControllerBase
    {
        private readonly EscolaContext db;

        public LoginController(EscolaContext database)
        {
            db = database;
        }

        [HttpPost]
        [Route("CreatePassword")]
        public ActionResult CreatePassword(string idUser, string password)
        {
            var result = new ResultData<User>() { Error = true, Status = HttpStatusCode.BadRequest };
            try
            {
                using (db)
                {
                    var userList = db.Usuario.Where(q => q.IdUser == idUser);
                    var user = userList.FirstOrDefault();
                    if (!userList.Select(q => q).Any())
                        throw new ArgumentException("O usuário não existe");

                    if (!string.IsNullOrEmpty(user.Password))
                        throw new ArgumentException("O usuário já possui senha cadastrada");

                    UserServices.CreatePassword(user, password);
                    db.SaveChanges();
                    result.Error = false;
                    result.Message.Add("Senha criada com sucesso");
                    result.Status = HttpStatusCode.OK;
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                result.Message.Add(e.Message);
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("CheckLogin")]
        public ActionResult CheckLogin(string idUser, string password)
        {
            var result = new ResultData<User>() { Error = true, Status = HttpStatusCode.BadRequest };
            try
            {
                var user = db.Usuario.Where(q => q.IdUser == idUser).FirstOrDefault();
                if (UserServices.Access(user, idUser, password))
                {
                    result.Error = false;
                    result.Message.Add(user.Role.ToString());
                    result.Status = HttpStatusCode.OK;
                    return Ok(result);
                }
                throw new ArgumentException("Acesso negado");
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

        [HttpPut]
        [Route("UpdatePassword")]
        public ActionResult UpdatePassword(string idUser, string old_password, string new_password)
        {
            var result = new ResultData<User>() { Error = true, Status = HttpStatusCode.BadRequest };
            try
            {
                var user = db.Usuario.Where(q => q.IdUser == idUser).FirstOrDefault();
                if (string.IsNullOrEmpty(user.Password))
                    throw new ArgumentException("O usuário não possui senha cadastrada");

                if (user.Password != old_password)
                    throw new ArgumentException("Senha antiga não é válida");

                UserServices.CreatePassword(user, new_password);
                db.SaveChanges();
                result.Error = false;
                result.Message.Add("Senha alterada com sucesso");
                result.Status = HttpStatusCode.OK;
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

    }
}
