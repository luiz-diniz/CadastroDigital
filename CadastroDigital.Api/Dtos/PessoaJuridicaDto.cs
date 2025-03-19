using CadastroDigital.Domain.Entities.Enums;

namespace CadastroDigital.Api.Dtos
{
    public class PessoaJuridicaDto
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime DataAbertura { get; set; }
        public SituacaoCadastralEnum SituacaoCadastral { get; set; }
        public EnderecoDto Endereco { get; set; }
    }
}
