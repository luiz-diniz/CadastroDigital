namespace CadastroDigital.Domain.Entities
{
    public abstract class Pessoa
    {
        public int Id { get; protected set; }
        public Endereco Endereco { get; protected set; }
    }
}