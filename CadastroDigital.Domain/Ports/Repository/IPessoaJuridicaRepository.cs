using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Ports.Repository
{
    public interface IPessoaJuridicaRepository
    {
        Task<int> CriarAsync(PessoaJuridica pessoaJuridica);
        Task<PessoaJuridica?> ObterAsync(int id);
        Task<IEnumerable<PessoaJuridica>> ListarAsync();
        Task AtualizarAsync(PessoaJuridica pessoaJuridica);
        Task ExcluirAsync(int id);
        Task<bool> VerificarExistenciaRegistro(string cnpj);
    }
}
