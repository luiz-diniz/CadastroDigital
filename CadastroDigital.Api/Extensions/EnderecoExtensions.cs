using CadastroDigital.Api.Dtos;
using CadastroDigital.Domain.Entities;

namespace CadastroDigital.Api.Extensions
{
    public static class EnderecoExtensions
    {
        public static Endereco ToEntity(this EnderecoDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Endereco(
                dto.Id,
                dto.Cep,                     
                dto.Logradouro,              
                dto.Numero,                  
                dto.Complemento,             
                dto.Bairro,                  
                dto.Cidade,                  
                dto.Estado                   
            );
        }

        public static EnderecoDto ToDto(this Endereco entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new EnderecoDto
            {
                Id = entity.Id,
                Cep = entity.Cep,
                Logradouro = entity.Logradouro,
                Numero = entity.Numero,
                Complemento = entity.Complemento,
                Bairro = entity.Bairro,
                Cidade = entity.Cidade,
                Estado = entity.Estado
            };
        }
    }
}
