using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // api/categorias/produtos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
        {
            try
            {
                return _context.Categorias
                               .Include(p => p.Produtos) // Carrega a coleção de Produtos relacionada
                               .Where(categoria => categoria.CategoriaId <= 5)
                               .AsNoTracking() // Melhor performance quando não vai alterar a entidade
                               .ToList(); // Consulta síncrona
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }

        // api/categorias
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList(); // Consulta síncrona
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }

        // api/categorias/1
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id); // Consulta síncrona

                if (categoria == null)
                {
                    return NotFound($"Categoria com ID {id} não foi encontrado");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }

        // api/categorias
        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria == null)
                {
                    return BadRequest("Categoria inválido");
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

                return CreatedAtRoute("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }

        // api/categorias/1
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest("ID do categoria não confere");
                }

                _context.Entry(categoria).State = EntityState.Modified; // Marca toda a entidade como modificada
                _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }

        // api/categorias/1
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id); // Busca categoria

                if (categoria == null)
                {
                    return NotFound($"Categoria com ID {id} não encontrado");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor!");
            }
        }
    }
}
