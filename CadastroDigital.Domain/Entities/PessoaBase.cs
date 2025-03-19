namespace CadastroDigital.Domain.Entities
{
    public abstract class PessoaBase
    {
        public int Id { get; protected set; }
        public Endereco Endereco { get; protected set; }

        public void AtualizarEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }
    }
}