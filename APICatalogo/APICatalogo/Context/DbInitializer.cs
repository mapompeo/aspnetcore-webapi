using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        // Aplica migrations se existirem, caso contrário cria o banco
        if (context.Database.GetPendingMigrations().Any())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }

        // Popula apenas se não houver dados
        if (!context.Categorias!.Any())
        {
            var categorias = new List<Categoria>
            {
                new Categoria { Nome = "Entradas", ImageUrl = "https://exemplo.com/img/entradas.jpg" },
                new Categoria { Nome = "Pratos Principais", ImageUrl = "https://exemplo.com/img/principais.jpg" },
                new Categoria { Nome = "Sobremesas", ImageUrl = "https://exemplo.com/img/sobremesas.jpg" },
                new Categoria { Nome = "Bebidas", ImageUrl = "https://exemplo.com/img/bebidas.jpg" },
                new Categoria { Nome = "Porções", ImageUrl = "https://exemplo.com/img/porcoes.jpg" }
            };

            await context.AddRangeAsync(categorias);
            await context.SaveChangesAsync();

            var produtos = new List<Produto>
            {
                // Entradas
                new Produto
                {
                    Nome = "Pão de Alho",
                    Descricao = "Pão italiano com manteiga de alho e ervas",
                    Preco = 18.90m,
                    ImagemUrl = "https://exemplo.com/img/pao-alho.jpg",
                    Estoque = 50,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[0].CategoriaId
                },
                new Produto
                {
                    Nome = "Salada Caesar",
                    Descricao = "Alface romana, croutons, parmesão e molho caesar",
                    Preco = 32.90m,
                    ImagemUrl = "https://exemplo.com/img/salada-caesar.jpg",
                    Estoque = 30,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[0].CategoriaId
                },
                
                // Pratos Principais
                new Produto
                {
                    Nome = "Picanha ao Ponto",
                    Descricao = "Picanha grelhada com arroz, farofa e vinagrete",
                    Preco = 89.90m,
                    ImagemUrl = "https://exemplo.com/img/picanha.jpg",
                    Estoque = 20,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[1].CategoriaId
                },
                new Produto
                {
                    Nome = "Salmão Grelhado",
                    Descricao = "Salmão grelhado com legumes e purê de batatas",
                    Preco = 78.90m,
                    ImagemUrl = "https://exemplo.com/img/salmao.jpg",
                    Estoque = 15,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[1].CategoriaId
                },

                // Sobremesas
                new Produto
                {
                    Nome = "Pudim de Leite",
                    Descricao = "Pudim de leite condensado tradicional",
                    Preco = 15.90m,
                    ImagemUrl = "https://exemplo.com/img/pudim.jpg",
                    Estoque = 25,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[2].CategoriaId
                },
                new Produto
                {
                    Nome = "Petit Gateau",
                    Descricao = "Bolo quente de chocolate com sorvete de creme",
                    Preco = 28.90m,
                    ImagemUrl = "https://exemplo.com/img/petit-gateau.jpg",
                    Estoque = 20,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[2].CategoriaId
                },

                // Bebidas
                new Produto
                {
                    Nome = "Caipirinha",
                    Descricao = "Cachaça, limão, açúcar e gelo",
                    Preco = 22.90m,
                    ImagemUrl = "https://exemplo.com/img/caipirinha.jpg",
                    Estoque = 100,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[3].CategoriaId
                },
                new Produto
                {
                    Nome = "Suco de Laranja Natural",
                    Descricao = "Suco de laranja natural da fruta",
                    Preco = 12.90m,
                    ImagemUrl = "https://exemplo.com/img/suco-laranja.jpg",
                    Estoque = 50,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[3].CategoriaId
                },

                // Porções
                new Produto
                {
                    Nome = "Batata Frita",
                    Descricao = "Porção de batata frita crocante",
                    Preco = 35.90m,
                    ImagemUrl = "https://exemplo.com/img/batata-frita.jpg",
                    Estoque = 40,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[4].CategoriaId
                },
                new Produto
                {
                    Nome = "Isca de Frango",
                    Descricao = "Iscas de frango empanadas",
                    Preco = 42.90m,
                    ImagemUrl = "https://exemplo.com/img/isca-frango.jpg",
                    Estoque = 35,
                    DataCadastro = DateTime.Now,
                    CategoriaId = categorias[4].CategoriaId
                }
            };

            await context.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
        }
    }
}