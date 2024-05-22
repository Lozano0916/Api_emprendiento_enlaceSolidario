using Emprendimiento_Api.Context;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Emprendimiento_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ConexionSQLServer _context;
        private const int elementosPag = 10;


        public ProductsController(ConexionSQLServer context)
        {
            _context = context;
        }
        // GET: api/<ProductosController>
        [HttpGet]
        public IActionResult Index([FromQuery] int pagina = 1, [FromQuery] string? nombreProductos = "", [FromQuery] int? idEmprendimiento = 0)
        {
            IQueryable<Models.Products> query = _context.Productos;

            if (!string.IsNullOrEmpty(nombreProductos))
            {
                string nombreMinusculas = nombreProductos.ToLower();
                query = query.Where(u => u.NombreProducto.Contains(nombreMinusculas));
            }

            if (idEmprendimiento.HasValue && idEmprendimiento > 0)
            {
                query = query.Where(u => u.IdEmprendimiento == idEmprendimiento.Value);
            }

            var totalProductos = query.Count();

            int totalPag = (int)Math.Ceiling((double)totalProductos / elementosPag);
            pagina = Math.Max(1, Math.Min(pagina, totalPag));

            int startIndex = (pagina - 1) * elementosPag;

            var Iproductos = query.Skip(startIndex).Take(elementosPag).ToList();

            var paginationMetadata = new
            {
                totalCount = totalProductos,
                pageSize = elementosPag,
                currentPage = pagina,
                totalPages = totalPag
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(new { total_productos = totalProductos, Iproductos });


        }

        // POST api/<ProductosController>
        [HttpPost("CrearProducto")]
        public async Task<IActionResult> CrearProducto([FromBody] Models.Products nuevoEmprendimiento)
        {
            if (ModelState.IsValid)
            {
                await _context.Productos.AddAsync(nuevoEmprendimiento);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<ProductosController>/5
        [HttpPut("actualizarProducto")]
        public async Task<IActionResult> actualizarProducto(int id, [FromBody] Models.Products nuevoProductoData)
        {
            var productos_ = await _context.Productos.FindAsync(id);


            if (productos_ == null)
            {
                return NotFound();
            }

            productos_.IdEmprendimiento = nuevoProductoData.IdEmprendimiento;
            productos_.NombreProducto = nuevoProductoData.NombreProducto;
            productos_.precio = nuevoProductoData.precio;
            productos_.Descripcion = nuevoProductoData.Descripcion;
            productos_.UrlFoto = nuevoProductoData.UrlFoto;

            _context.Productos.Update(productos_);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("eliminarProducto")]
        public async Task<IActionResult> eliminarProducto(int id)
        {
            var producto_ = await _context.Productos.FindAsync(id);

            if (producto_ == null)
            {
                return NotFound();
            }


            _context.Productos.Remove(producto_);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
