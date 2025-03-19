using CadastroDigital.Application.Exceptions;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;

namespace CadastroDigital.Application.Services
{
    public class PessoaJuridicaService : IPessoaJuridicaService
    {
        private readonly ILogger<PessoaJuridicaService> _logger;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public PessoaJuridicaService(ILogger<PessoaJuridicaService> logger, IPessoaJuridicaRepository pessoaJuridicaRepository, IEnderecoRepository enderecoRepository)
        {
            _logger = logger;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task AtualizarAsync(PessoaJuridica pessoaJuridica)
        {
            try
            {
                var pessoa = await _pessoaJuridicaRepository.ObterAsync(pessoaJuridica.Id);

                if (pessoa is null)
                    throw new EntityNotFoundException($"Pessoa jurídica com o Id {pessoaJuridica.Id} não encontrada");

                await _pessoaJuridicaRepository.AtualizarAsync(pessoaJuridica);

                await _enderecoRepository.AtualizarAsync(pessoaJuridica.Endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CriarAsync(PessoaJuridica pessoaJuridica)
        {
            try
            {
                if (await _pessoaJuridicaRepository.VerificarExistenciaRegistro(pessoaJuridica.Cnpj))
                    throw new EntityAlreadyExistsException($"Pessoa jurídica com o CNPJ {pessoaJuridica.Cnpj} já cadastrada");

                var idEndereco = await _enderecoRepository.CriarAsync(pessoaJuridica.Endereco);

                pessoaJuridica.Endereco.AtribuirId(idEndereco);

                var id = await _pessoaJuridicaRepository.CriarAsync(pessoaJuridica);

                return id;
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
                var pessoa = await _pessoaJuridicaRepository.ObterAsync(id);

                if (pessoa is null)
                    throw new EntityNotFoundException($"Pessoa jurídica com o Id {id} não encontrada");

                await _enderecoRepository.ExcluirAsync(pessoa.Endereco.Id);

                await _pessoaJuridicaRepository.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<PessoaJuridica>> ListarAsync()
        {
            try
            {
                return await _pessoaJuridicaRepository.ListarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PessoaJuridica?> ObterAsync(int id)
        {
            try
            {
                return await _pessoaJuridicaRepository.ObterAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
