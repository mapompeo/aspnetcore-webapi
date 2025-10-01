using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("primeiro")] // /api/produtos/primeiro
        [HttpGet("teste")]    // /api/produtos/teste
        [HttpGet("/primeiro")] // rota absoluta: /primeiro (ignora "api/produtos")
        [HttpGet("{valor:alpha:length(5)}")] // aceita strings apenas com 5 letras
        public ActionResult<Produto> GetPrimeiro(string valor)
        {
            var teste = valor;
            var produto = _context.Produtos.FirstOrDefault(); // Busca o primeiro produto

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            return Ok(produto);
        }

        // api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos.Take(10).AsNoTracking().ToListAsync(); // Consulta assíncrona

            if (produtos == null || !produtos.Any())
            {
                return NotFound("Produtos não encontrados");
            }

            return Ok(produtos);
        }

        // api/produtos/1
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery] int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id); // Busca assíncrona

            if (produto == null)
            {
                return NotFound("Produto não foi encontrado");
            }

            return Ok(produto);
        }

        // api/produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Produto inválido");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

            return CreatedAtRoute("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        // api/produtos/1
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("ID do produto não confere");
            }

            _context.Entry(produto).State = EntityState.Modified; // Marca toda a entidade como modificada
            _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

            return Ok(produto);
        }

        // api/produtos/1
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id); // Busca produto

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges(); // Poderia usar SaveChangesAsync para método assíncrono

            return Ok(produto);
        }
    }
}
