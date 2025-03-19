using CadastroDigital.Application.Exceptions;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;

namespace CadastroDigital.Application.Services
{
    public class PessoaFisicaService : IPessoaFisicaService
    {
        private readonly ILogger<PessoaFisicaService> _logger;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public PessoaFisicaService(ILogger<PessoaFisicaService> logger, IPessoaFisicaRepository pessoaFisicaRepository, IEnderecoRepository enderecoRepository)
        {
            _logger = logger;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task AtualizarAsync(PessoaFisica pessoaFisica)
        {
            try
            {
                var pessoa = await _pessoaFisicaRepository.ObterAsync(pessoaFisica.Id);

                if (pessoa is null)
                    throw new EntityNotFoundException($"Pessoa física com o Id {pessoaFisica.Id} não encontrada");

                await _pessoaFisicaRepository.AtualizarAsync(pessoaFisica);

                await _enderecoRepository.AtualizarAsync(pessoaFisica.Endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CriarAsync(PessoaFisica pessoaFisica)
        {
            try
            {
                if(await _pessoaFisicaRepository.VerificarExistenciaRegistro(pessoaFisica.Cpf))
                    throw new EntityAlreadyExistsException($"Pessoa física com o CPF {pessoaFisica.Cpf} já cadastrada");

                var idEndereco = await _enderecoRepository.CriarAsync(pessoaFisica.Endereco);

                pessoaFisica.Endereco.AtribuirId(idEndereco);

                var id = await _pessoaFisicaRepository.CriarAsync(pessoaFisica);

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
                var pessoa = await _pessoaFisicaRepository.ObterAsync(id);

                if(pessoa is null)
                    throw new EntityNotFoundException($"Pessoa física com o Id {id} não encontrada");

                await _enderecoRepository.ExcluirAsync(pessoa.Endereco.Id);

                await _pessoaFisicaRepository.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<PessoaFisica>> ListarAsync()
        {
            try
            {
                return await _pessoaFisicaRepository.ListarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PessoaFisica?> ObterAsync(int id)
        {
            try
            {               
                return await _pessoaFisicaRepository.ObterAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
