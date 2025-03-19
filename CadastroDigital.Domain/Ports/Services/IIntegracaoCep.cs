namespace CadastroDigital.Domain.Ports.Services
{
    public interface IIntegracaoCep
    {
        Task<string?> ConsultarEnderecoPorCep(string cep);
    }
}
