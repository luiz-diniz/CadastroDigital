using CadastroDigital.Application.Models;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CadastroDigital.Application.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly ILogger<EnderecoService> _logger;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IIntegracaoCep _integracaoViaCepService;

        public EnderecoService(ILogger<EnderecoService> logger, IEnderecoRepository enderecoRepository, IIntegracaoCep integracaoViaCepService)
        {
            _logger = logger;
            _enderecoRepository = enderecoRepository;
            _integracaoViaCepService = integracaoViaCepService;
        }

        public async Task AtualizarAsync(Endereco endereco)
        {
            try
            {
                await _enderecoRepository.AtualizarAsync(endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CriarAsync(Endereco endereco)
        {
            try
            {
                var dados = JsonSerializer.Deserialize<DadosComplementaresEndereco>(await _integracaoViaCepService.ConsultarEnderecoPorCep(endereco.Cep) ?? string.Empty, 
                    new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dados is not null)
                    endereco.IncluirDadosComplementares(dados.UF, dados.Localidade, dados.DDD, dados.IBGE);

                var idEndereco = await _enderecoRepository.CriarAsync(endereco);

                return idEndereco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ExcluirAsync(int id)
        {
            try
            {
                await _enderecoRepository.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
