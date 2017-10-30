using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiUsuarios.Models;
using OfficeOpenXml;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private readonly UsuariosContext _context;

        public UsuariosController(UsuariosContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public IEnumerable<Object> GetAllUsuarios()
        {
            return _context.Usuarios.Select(u => new { u.Id, u.Email}).ToList();
        }

        [HttpGet("{id}",Name = "GetUsuarios")]
        public IActionResult GetUsuariosById(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if(usuario == null)
            {
                return NotFound();
            }
            return new ObjectResult(usuario);
        }

        [HttpGet]
        [Route("xlsx")]
        public IActionResult GetUsuariosXLSX()
        {
            byte[] arquivo;
            using (ExcelPackage package = new ExcelPackage())
            {
                
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Usuarios");
                worksheet.Cells.LoadFromCollection(_context.Usuarios.ToList(),true);               
                package.Save();
                arquivo = package.GetAsByteArray();
            }
            var result = new FileContentResult(arquivo, "application/octet-stream");
            result.FileDownloadName = "usuarios.xlsx";
            return result;            
        }

 
        [HttpPost]
        public IActionResult PostUsuarios([FromBody]Usuarios usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            if (EmailExiste(usuario.Email))
                return BadRequest(String.Format("E-mail {0} já existe na base de dados", usuario.Email));
 
            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
            } catch(Exception ex)
            {
                return Forbid(ex.Message);
            }
            return CreatedAtRoute("GetUsuarios", new {id  = usuario.Id },usuario);
        }

        
        [HttpDelete("{id}", Name = "DeleteUsuarios")]
        public IActionResult DeleteUsuariosById(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return NotFound();
            
            try
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
            return Ok();
        }

        private bool EmailExiste(string email)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                return false;
            else
                return true;            
        }

       
    }
}