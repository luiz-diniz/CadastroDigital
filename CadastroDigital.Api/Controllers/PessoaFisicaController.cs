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
    [Route("api/pessoas/fisicas")]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly ILogger<PessoaFisicaController> _logger;
        private readonly IPessoaFisicaService _pessoaFisicaService;

        public PessoaFisicaController(ILogger<PessoaFisicaController> logger, IPessoaFisicaService pessoaFisicaService)
        {
            _logger = logger;
            _pessoaFisicaService = pessoaFisicaService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PessoaFisicaDto pessoaFisica)
        {
            try
            {
                var id = await _pessoaFisicaService.CriarAsync(pessoaFisica.ToEntity());

                return StatusCode((int)HttpStatusCode.Created, new { Id = id });
            }
            catch(ValidationException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar pessoa física");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pessoa física");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var resultado = await _pessoaFisicaService.ListarAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar os dados de pessoa física");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Obter([FromRoute] int id)
        {
            try
            {
                var resultado = await _pessoaFisicaService.ObterAsync(id);

                if(resultado is null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pessoa física");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] int id)
        {
            try
            {
                await _pessoaFisicaService.ExcluirAsync(id);

                return Ok();
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir pessoa física");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] PessoaFisicaDto pessoaFisica)
        {
            try
            {
                await _pessoaFisicaService.AtualizarAsync(pessoaFisica.ToEntity());

                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pessoa física");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
