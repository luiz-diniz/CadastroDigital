using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Interfaces
{
    public interface IPessoaFisicaRepository
    {
        Task<int> CriarAsync(PessoaFisica pessoaFisica);
        Task<PessoaFisica?> ObterAsync(int id);
        Task<IEnumerable<PessoaFisica>> ListarAsync();
        Task AtualizarAsync(PessoaFisica pessoaFisica);
        Task ExcluirAsync(int id);
    }
}
