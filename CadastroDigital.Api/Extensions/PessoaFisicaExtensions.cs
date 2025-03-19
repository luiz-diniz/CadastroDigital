using CadastroDigital.Api.Dtos;
using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Api.Extensions
{
    public static class PessoaFisicaExtensions
    {
        public static PessoaFisica ToEntity(this PessoaFisicaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new PessoaFisica(
                dto.Id,                    
                dto.Cpf,                   
                dto.Nome,                  
                dto.DataNascimento,        
                dto.Endereco.ToEntity()    
            );
        }

        public static PessoaFisicaDto ToDto(this PessoaFisica entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new PessoaFisicaDto
            {
                Id = entity.Id,
                Cpf = entity.Cpf,
                Nome = entity.Nome,
                DataNascimento = entity.DataNascimento,
                Endereco = entity.Endereco.ToDto()
            };
        }
    }
}
