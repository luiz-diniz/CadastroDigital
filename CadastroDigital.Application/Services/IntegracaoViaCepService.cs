using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace CadastroDigital.Application.Services
{
    public class IntegracaoViaCepService : IIntegracaoCep
    {
        private readonly ILogger<IntegracaoViaCepService> _logger;
        private readonly HttpClient _httpClient;

        public IntegracaoViaCepService(ILogger<IntegracaoViaCepService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string?> ConsultarEnderecoPorCep(string cep)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Erro ao consultar o CEP {cep}", cep);
                return null!;
            }
        }
    }
}