using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Domain.Ports.Repository
{
    //TODO: Avaliar necessidade de uso do Async
    public interface IPessoaRepository<T> where T : PessoaBase
    {
        Task<int> CriarAsync(T pessoa);
        Task<T> ObterAsync(int id);
        Task<IEnumerable<T>> ListarAsync();
        Task AtualizarAsync(T pessoa);
        Task ExcluirAsync(int id);
        Task<bool> VerificarExistenciaRegistro(T pessoa);
    }
}
