using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularApp4.Server.Models;
using AngularApp4.Server.Models.DTO;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;
using System.Data;
using Dapper;

namespace AngularApp4.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;



        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes(int first = 0, int rows = 10, string searchTerm = null)
        {
            if (first < 0) first = 0;
            if (rows < 1) rows = 10;

            // Construcción de la consulta base
            var query = _context.Cliente.AsQueryable();
            // Aplicar filtro si se proporciona un término de búsqueda
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                bool isNumeric = int.TryParse(searchTerm, out int searchTermInt);
                query = query.Where(c =>
                    (isNumeric && c.Id == searchTermInt) ||
                    c.Rut.ToLower().Contains(searchTerm) ||
                    c.Nombre.ToLower().Contains(searchTerm) ||
                    c.Apellido.ToLower().Contains(searchTerm) ||
                    c.Direccion.ToLower().Contains(searchTerm) ||
                    c.Email.ToLower().Contains(searchTerm) ||
                    c.Telefono.ToLower().Contains(searchTerm) ||
                    c.Pais!.Nombre.ToLower().Contains(searchTerm) 
                );
            }

            //int totalRecords = await _context.Cliente.CountAsync();
            int totalRecords = await query.CountAsync();

            //return await _context.Cliente.ToListAsync();
            List<Cliente> clientes = await query
                                                .Include(c => c.Pais)
                                                .OrderByDescending(c => c.Id)
                                                .Skip(first) // Omite los primeros 'first' registros
                                                .Take(rows) // Toma los siguientes 'rows' registros
                                                .ToListAsync();
            var result = new
            {
                totalRecords = totalRecords,
                records = clientes
            };
            return Ok(result);

        }

        // GET DESDE PRODECIMIENTO ALMACENADO
        [HttpGet("GetClientes2")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes2(int first = 0, int rows = 10, string searchTerm = null )
        {
            // ejecutar procedimiento almacena llamado sp_ObtenerClientes
            // y pasar el parámetro @PageIndex
            if (first < 0) first = 0;
            if (rows < 1) rows = 10;

            var clientes = new List<ClienteDTO>();

            string connectionString = _context.Database.GetDbConnection().ConnectionString;

            try
            {
                await using var connection = new SqlConnection(connectionString);

                /*
                clientes = connection.Query<ClienteDTO>("sp_ObtenerClientes @PageIndex",
                                                new {
                                                    PageIndex = pageIndex
                                                }).ToList();
                */

                clientes = (await connection.QueryAsync<ClienteDTO>("sp_GetClientesConPaginacion @first, @rows, @searchTerm",
                                                new { 
                                                    first =  first,
                                                    rows = rows,
                                                    searchTerm = searchTerm
                                                })).ToList();

            }
            catch (SqlException ex)
            {
                // Manejo de excepciones específico de SQL
                Console.Error.WriteLine($"Error de SQL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw;
            }
            var result = new
            {
                totalRecords = clientes[0].TotalRecords,
                records = clientes
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            if (ModelState.IsValid) { 
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
                return CreatedAtAction("PostCliente", new { id = cliente.Id }, cliente);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            { 
                try
                {
                    _context.Entry(cliente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        private bool ClienteExists(int id) 
        {
                return _context.Cliente.Any(c => c.Id == id);
        }

    }

   
}
