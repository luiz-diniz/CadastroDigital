using Microsoft.AspNetCore.Mvc;
using CadastroDigital.Domain.Ports.Services;
using CadastroDigital.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;
using CadastroDigital.Api.Dtos;
using CadastroDigital.Api.Extensions;
using CadastroDigital.Application.Exceptions;

namespace CadastroDigital.Api.Controllers
{
    [ApiController]
    [Route("api/pessoas/juridicas")]
    public class PessoaJuridicaController : ControllerBase
    {
        private readonly ILogger<PessoaJuridicaController> _logger;
        private readonly IPessoaService<PessoaJuridica> _pessoaJuridicaService;

        public PessoaJuridicaController(ILogger<PessoaJuridicaController> logger, IPessoaService<PessoaJuridica> pessoaJuridicaService)
        {
            _logger = logger;
            _pessoaJuridicaService = pessoaJuridicaService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PessoaJuridicaDto pessoaJuridica)
        {
            try
            {
                var id = await _pessoaJuridicaService.CriarAsync(pessoaJuridica.ToEntity());

                return StatusCode((int)HttpStatusCode.Created, new { Id = id });
            }
            catch(ValidationException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar pessoa jurídica");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pessoa jurídica");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var resultado = await _pessoaJuridicaService.ListarAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar os dados de pessoa jurídica");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Obter([FromRoute] int id)
        {
            try
            {
                var resultado = await _pessoaJuridicaService.ObterAsync(id);

                if(resultado is null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pessoa jurídica");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] int id)
        {
            try
            {
                await _pessoaJuridicaService.ExcluirAsync(id);

                return Ok();
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir jurídica");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] PessoaJuridicaDto pessoaJuridica)
        {
            try
            {
                await _pessoaJuridicaService.AtualizarAsync(pessoaJuridica.ToEntity());

                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pessoa jurídica");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
