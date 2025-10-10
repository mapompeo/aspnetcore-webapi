using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            _logger.LogWarning("Nenhum produto encontrado.");
            return NotFound("Nenhum produto encontrado.");
        }

        var produtosDto = produtos!.ToProdutoDTOList();
        return Ok(produtosDto);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado.");
            return NotFound($"Produto com id={id} não encontrado.");
        }

        var produtoDto = produto!.ToProdutoDTO();
        return Ok(produtoDto);
    }

    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null || !produtos.Any())
        {
            _logger.LogWarning($"Nenhum produto encontrado para a categoria id={id}.");
            return NotFound($"Nenhum produto encontrado para a categoria id={id}.");
        }

        var produtosDto = produtos!.ToProdutoDTOList();
        return Ok(produtosDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
        {
            _logger.LogWarning("Dados inválidos no corpo da requisição.");
            return BadRequest("Dados inválidos");
        }

        var produto = produtoDto!.ToProduto();
        var novoProduto = _uof.ProdutoRepository.Create(produto!);
        _uof.Commit();

        var novoProdutoDto = novoProduto!.ToProdutoDTO();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto.ProdutoId },
            novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
        {
            _logger.LogWarning("ID do produto não corresponde ao corpo da requisição.");
            return BadRequest("Dados inválidos");
        }

        var produto = produtoDto!.ToProduto();
        var produtoAtualizado = _uof.ProdutoRepository.Update(produto!);
        _uof.Commit();

        var produtoAtualizadoDto = produtoAtualizado!.ToProdutoDTO();
        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado.");
            return NotFound($"Produto com id={id} não encontrado.");
        }

        var produtoDeletado = _uof.ProdutoRepository.Delete(produto!);
        _uof.Commit();

        var produtoDeletadoDto = produtoDeletado!.ToProdutoDTO();
        return Ok(produtoDeletadoDto);
    }
}
