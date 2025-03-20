using CadastroDigital.Application.Exceptions;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;

namespace CadastroDigital.Application.Services
{
    public class PessoaService<T> : IPessoaService<T> where T : PessoaBase
    {
        private readonly ILogger<IPessoaService<T>> _logger;
        private readonly IPessoaRepository<T> _pessoaRepository;
        private readonly IEnderecoService _enderecoService;

        public PessoaService(ILogger<IPessoaService<T>> logger, IPessoaRepository<T> pessoaRepository, IEnderecoService enderecoService)
        {
            _logger = logger;
            _pessoaRepository = pessoaRepository;
            _enderecoService = enderecoService;
        }

        public async Task AtualizarAsync(T pessoa)
        {
            try
            {
                var resultado = await _pessoaRepository.ObterAsync(pessoa.Id);

                if (resultado is null)
                    throw new EntityNotFoundException($"Registro com o [{pessoa.Id}] não encontrado");

                await _pessoaRepository.AtualizarAsync(pessoa);

                await _enderecoService.AtualizarAsync(pessoa.Endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> CriarAsync(T pessoa)
        {
            try
            {
                if(await _pessoaRepository.VerificarExistenciaRegistro(pessoa))
                    throw new EntityAlreadyExistsException($"Registro com o documento já encontrado");
                
                var idEndereco = await _enderecoService.CriarAsync(pessoa.Endereco);

                pessoa.Endereco.AtribuirId(idEndereco);

                var id = await _pessoaRepository.CriarAsync(pessoa);

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
                var pessoa = await _pessoaRepository.ObterAsync(id);

                if(pessoa is null)
                    throw new EntityNotFoundException($"Registro com o [{id}] não encontrado");

                await _enderecoService.ExcluirAsync(pessoa.Endereco.Id);

                await _pessoaRepository.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<T>> ListarAsync()
        {
            try
            {
                return await _pessoaRepository.ListarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<T?> ObterAsync(int id)
        {
            try
            {               
                return await _pessoaRepository.ObterAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
