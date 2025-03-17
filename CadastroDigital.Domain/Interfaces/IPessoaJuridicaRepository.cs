using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Interfaces
{
    public interface IPessoaJuridicaRepository
    {
        Task CriarAsync(PessoaJuridica pessoaJuridica);
        Task<PessoaJuridica> ObterAsync(int id);
        Task<IEnumerable<PessoaJuridica>> ListarAsync();
        Task AtualizarAsync(PessoaJuridica pessoaJuridica);
        Task ExcluirAsync(int id);
    }
}
