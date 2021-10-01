using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCatalogoLivros.Entities;

namespace WebApiCatalogoLivros.Repositories
{
    public interface ILivroRepository : IDisposable
    {
        Task<List<Livro>> Obter(int pagina, int quantidade);
        Task<Livro> Obter(Guid id);
        Task<List<Livro>> Obter(string nome, string produtora);
        Task Inserir(Livro livro);
        Task Atualizar(Livro livro);
        Task Remover(Guid id);

    }
}
