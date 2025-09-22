using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();

            if (produtos == null || !produtos.Any())
            {
                return NotFound("Produtos não encontrados");
            }

            return Ok(produtos);
        }

        // GET: /Produtos/5
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound("Produto não foi encontrado");
            }

            return Ok(produto);
        }

        // POST: /Produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Produto inválido");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            // Retorna o produto criado com a rota para consulta
            return CreatedAtRoute("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        // PUT: /Produtos/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("ID do produto não confere");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        // DELETE: /Produtos/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
