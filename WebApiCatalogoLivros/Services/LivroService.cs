using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCatalogoLivros.Entities;
using WebApiCatalogoLivros.Exceptions;
using WebApiCatalogoLivros.InputModel;
using WebApiCatalogoLivros.Repositories;
using WebApiCatalogoLivros.ViewModel;

namespace WebApiCatalogoLivros.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        //contrato interface
        public async Task<List<LivroViewModel>> Obter(int pagina, int quantidade)
        {
            var livro = await _livroRepository.Obter(pagina, quantidade);

            return livro.Select(livro => new LivroViewModel
            {
                Id = livro.Id,
                Nome = livro.Nome,
                Produtora = livro.Produtora,
                Preco = livro.Preco
            }).ToList();
        }

        public async Task<LivroViewModel> Obter(Guid id)
        {
            var livro = await _livroRepository.Obter(id);

            if (livro == null)
                return null;

            return new LivroViewModel
            {
                Id = livro.Id,
                Nome = livro.Nome,
                Produtora = livro.Produtora,
                Preco = livro.Preco
            };
        }

        public async Task<LivroViewModel> Inserir(LivroInputModel livro)
        {
            var entidadeLivro = await _livroRepository.Obter(livro.Nome, livro.Produtora);

            if (entidadeLivro.Count > 0)
                throw new LivroJaCadastradoException();

            var livroInsert = new Livro
            {
                Id = Guid.NewGuid(),
                Nome = livro.Nome,
                Produtora = livro.Produtora,
                Preco = livro.Preco
            };

            await _livroRepository.Inserir(livroInsert);

            return new LivroViewModel
            {
                Id = livroInsert.Id,
                Nome = livro.Nome,
                Produtora = livro.Produtora,
                Preco = livro.Preco
            };
        }

        public async Task Atualizar(Guid id, LivroInputModel livro)
        {
            var entidadeLivro = await _livroRepository.Obter(id);

            if (entidadeLivro == null)
                throw new LivroNaoCadastradoException();

            entidadeLivro.Nome = livro.Nome;
            entidadeLivro.Produtora = livro.Produtora;
            entidadeLivro.Preco = livro.Preco;

            await _livroRepository.Atualizar(entidadeLivro);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeLivro = await _livroRepository.Obter(id);

            if (entidadeLivro == null)
                throw new LivroNaoCadastradoException();

            entidadeLivro.Preco = preco;

            await _livroRepository.Atualizar(entidadeLivro);
        }

        public async Task Remover(Guid id)
        {
            var livro = await _livroRepository.Obter(id);

            if (livro == null)
                throw new LivroNaoCadastradoException();

            await _livroRepository.Remover(id);
        }

        public void Dispose()
        {
            _livroRepository?.Dispose(); // garante que n teremos conexoes abertas com o banco
        }
    }
}
