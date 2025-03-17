using System.ComponentModel.DataAnnotations;

namespace CadastroDigital.Domain.Entities
{
    public class PessoaFisica : Pessoa
    {
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }

        public PessoaFisica(int id, string cpf, string nome, DateTime dataNascimento)
        {
            if (id <= 0)
                throw new ValidationException("Id deve ser maior que zero");

            Validar(cpf, nome, dataNascimento);

            Id = id;
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        public PessoaFisica(string cpf, string nome, DateTime dataNascimento)
        {
            Validar(cpf, nome, dataNascimento);

            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        private void Validar(string cpf, string nome, DateTime dataNascimento)
        {
            if (string.IsNullOrEmpty(cpf))
                throw new ValidationException("CPF é obrigatório");

            //TODO: Implementar validação de CPF
            if (cpf.Length != 11)
                throw new ValidationException("CPF está inválido");

            if (string.IsNullOrEmpty(nome))
                throw new ValidationException("Nome é obrigatório");

            if (dataNascimento == DateTime.MinValue)
                throw new ValidationException("Data de Nascimento é obrigatório");
        }
    }
}
