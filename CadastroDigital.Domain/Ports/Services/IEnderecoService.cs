using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Ports.Services
{
    public interface IEnderecoService
    {
        Task<int> CriarAsync(Endereco endereco);
        Task<Endereco> ObterAsync(int id);
        Task AtualizarAsync(Endereco endereco);
        Task ExcluirAsync(int id);
    }
}
