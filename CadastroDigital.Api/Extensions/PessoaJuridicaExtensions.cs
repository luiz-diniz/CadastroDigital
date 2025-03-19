using CadastroDigital.Api.Dtos;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Entities.Enums;

namespace CadastroDigital.Api.Extensions
{
    public static class PessoaJuridicaExtensions
    {
        public static PessoaJuridica ToEntity(this PessoaJuridicaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new PessoaJuridica(
                dto.Id,
                dto.Cnpj,                     
                dto.RazaoSocial,              
                dto.NomeFantasia,             
                dto.DataAbertura,             
                dto.SituacaoCadastral,
                dto.Endereco.ToEntity()
            );
        }

        public static PessoaJuridicaDto ToDto(this PessoaJuridica entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new PessoaJuridicaDto
            {
                Id = entity.Id,
                Cnpj = entity.Cnpj,
                RazaoSocial = entity.RazaoSocial,
                NomeFantasia = entity.NomeFantasia,
                DataAbertura = entity.DataAbertura,
                SituacaoCadastral = entity.SituacaoCadastral,
                Endereco = entity.Endereco.ToDto()
            };
        }
    }
}
