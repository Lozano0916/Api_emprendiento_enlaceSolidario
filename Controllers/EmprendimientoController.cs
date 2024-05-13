using Emprendimiento_Api.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Emprendimiento_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprendimientoController : ControllerBase
    {
        private readonly ConexionSQLServer _context;
        private const int elementosPag = 10;


        public EmprendimientoController(ConexionSQLServer context)
        {
            _context = context;
        }
        // GET: api/<EmprendimientoController>
        [HttpGet]
        public IActionResult Index([FromQuery] int pagina = 1, [FromQuery] string? nombreEmprendimiento = "")
        {
            IQueryable<Models.Emprendimiento> query = _context.Emprendimiento;

            if (!string.IsNullOrEmpty(nombreEmprendimiento))
            {
                string nombreMinusculas = nombreEmprendimiento.ToLower();
                query = query.Where(u => u.Nombre.Contains(nombreMinusculas));
            }

            var totalEmprendimientos = query.Count();

            int totalPag = (int)Math.Ceiling((double)totalEmprendimientos / elementosPag);
            pagina = Math.Max(1, Math.Min(pagina, totalPag));

            int startIndex = (pagina - 1) * elementosPag;

            var Iemprendimientos = query.Skip(startIndex).Take(elementosPag).ToList();

            var paginationMetadata = new
            {
                totalCount = totalEmprendimientos,
                pageSize = elementosPag,
                currentPage = pagina,
                totalPages = totalPag
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(new { total_usuarios = totalEmprendimientos, Iemprendimientos});




            var Iemprendimiento = _context.Emprendimiento.ToList();
            return Ok(Iemprendimiento);

        }
        // POST api/<EmprendimientoController>
        [HttpPost("CrearEmprendimiento")]
        public async Task<IActionResult> CrearEmprendimiento([FromBody] Models.Emprendimiento nuevoEmprendimiento)
        {
            if (ModelState.IsValid)
            {
                await _context.Emprendimiento.AddAsync(nuevoEmprendimiento);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<EmprendimientoController>/5
        [HttpPut("actualizarEmprendimiento")]
        public async Task<IActionResult> actualizarEmprendimiento(int id, [FromBody] Models.Emprendimiento nuevoEmprendimientoData)
        {
            var emprendimiento_ = await _context.Emprendimiento.FindAsync(id);


            if (emprendimiento_ == null)
            {
                return NotFound();
            }

            emprendimiento_.Nombre = nuevoEmprendimientoData.Nombre;
            emprendimiento_.Descripcion = nuevoEmprendimientoData.Descripcion;
            emprendimiento_.ChatWhatsApp = nuevoEmprendimientoData.ChatWhatsApp;
            emprendimiento_.UrlFoto = nuevoEmprendimientoData.UrlFoto;
            emprendimiento_.Estado  = nuevoEmprendimientoData.  Estado;

            _context.Emprendimiento.Update(emprendimiento_);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/<EmprendimientoController>/5
        [HttpDelete("eliminarEmprendimiento")]
        public async Task<IActionResult> eliminarEmprendimiento(int id)
        {
            var emprendimiento_ = await _context.Emprendimiento.FindAsync(id);

            if (emprendimiento_ == null)
            {
                return NotFound();
            }


            _context.Emprendimiento.Remove(emprendimiento_);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
