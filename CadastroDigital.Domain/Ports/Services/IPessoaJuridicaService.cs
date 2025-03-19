using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Ports.Services
{
    public interface IPessoaJuridicaService
    {
        Task<int> CriarAsync(PessoaJuridica pessoaJuridica);
        Task<PessoaJuridica> ObterAsync(int id);
        Task<IEnumerable<PessoaJuridica>> ListarAsync();
        Task AtualizarAsync(PessoaJuridica pessoaJuridica);
        Task ExcluirAsync(int id);
    }
}
