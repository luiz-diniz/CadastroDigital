namespace CadastroDigital.Api.Dtos
{
    public class PessoaFisicaDto
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public EnderecoDto Endereco { get; set; }
    }
}
