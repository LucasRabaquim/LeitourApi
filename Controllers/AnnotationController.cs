using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Services;
using LeitourApi.Repository;
using LeitourApi.Interfaces;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Storage;

namespace LeitourApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationsController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly MessageService message;
        public AnnotationsController(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            message = new MessageService("Anotações", "o");
         
        }

        [HttpGet("savedBook/{id}")]
        public async Task<ActionResult<List<Annotation>>>? GetUserAnnotations([FromHeader] string token, int id)
        {
            int Id = TokenService.DecodeToken(token);
            User? user = await uow.UserRepository.GetById(Id);
            if (user == null)
                return message.MsgInvalid();
            SavedBook? saved = await uow.SavedRepository.GetById(id);
            if (saved == null)// || (!saved.Public && saved.UserId != id))
                return message.MsgDebug("Livro null");
            var annotation = await uow.AnnotationRepository.GetAllByCondition(a => a.SavedBookId == saved.Id);
            if (annotation == null)
                return message.MsgDebug("Annotação nula");
            return Ok(annotation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavedBook>> GetUserAnnotation([FromHeader] string token, int id)
        {
            int Id = TokenService.DecodeToken(token);
            User? user = await uow.UserRepository.GetById(Id);
            if (user == null)
                return message.MsgInvalid();
            var annotation = await uow.AnnotationRepository.GetById(Id);
            if (annotation == null)
                return message.MsgNotFound();
            SavedBook saved = await uow.SavedRepository.GetById(annotation.SavedBookId);
            if (!saved.Public && saved.UserId != id)
                return message.NotFound();
            return Ok(annotation);
        }

        [HttpPost("savedBook/{id}")]
        public async Task<ActionResult<List<Annotation>>>? AddAnnotations([FromHeader] string token, int id, [FromBody] Annotation annotation)
        {
            int Id = TokenService.DecodeToken(token);
            User? user = await uow.UserRepository.GetById(Id);
            if (user == null)
                return message.MsgInvalid();
            SavedBook saved = await uow.SavedRepository.GetById(id);
            if (saved == null)
                return new MessageService("Livr","o").MsgNotFound();
            uow.AnnotationRepository.Add(annotation);
            return Ok(annotation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SavedBook>>? UpdateAnnotation([FromHeader] string token, int id,[FromBody] Annotation annotation)
        {
            int userId = TokenService.DecodeToken(token);
            User? user = await uow.UserRepository.GetById(userId);
            if (user == null)
                return message.MsgNotFound();
            if (user.Access == "Desativado")
                return message.MsgDeactivate();
            if (id != annotation.AnnotationId || userId != user.Id)
                return message.MsgInvalid();
            uow.AnnotationRepository.Update(annotation);
            return message.MsgAlterated();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SavedBook>>? DeleteAnnotation([FromHeader] string token, int id)
        {
            int userId = TokenService.DecodeToken(token);
            User? user = await uow.UserRepository.GetById(userId);
            if (user == null)
                return message.MsgNotFound();
            if (user.Access == "Desativado")
                return message.MsgDeactivate();
            var annotation = await uow.AnnotationRepository.GetById(id);
            if (annotation == null)
                return message.MsgNotFound();
            if (id != annotation.AnnotationId || userId != user.Id)
                return message.MsgInvalid();
            uow.AnnotationRepository.Delete(annotation);
            return message.MsgDeleted();
        }
    }
}