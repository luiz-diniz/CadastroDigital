using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Ports.Repository
{
    public interface IEnderecoRepository
    {
        Task<int> CriarAsync(Endereco endereco);
        Task AtualizarAsync(Endereco endereco);
        Task ExcluirAsync(int id);
    }
}