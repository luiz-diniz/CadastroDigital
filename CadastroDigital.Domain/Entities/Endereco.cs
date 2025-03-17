using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CadastroDigital.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; private set; }
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public int Numero { get; private set; }
        public string? Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }

        public Endereco(string cep, string logradouro, int numero, string complemento, string bairro, string cidade, string estado)
        {
            Validar(cep, logradouro, numero, bairro, cidade, estado);

            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }

        private void Validar(string cep, string logradouro, int numero, string bairro, string cidade, string estado)
        {
            if (string.IsNullOrEmpty(cep))
                throw new ValidationException("CEP é obrigatório");

            if (Regex.IsMatch(cep, "^[0-9]{8}$"))
                throw new ValidationException("CEP deve conter apenas 8 números");

            if (string.IsNullOrEmpty(logradouro))
                throw new ValidationException("Logradouro é obrigatório");

            if (numero < 0)
                throw new ValidationException("Número não pode ser menor do que zero");

            if (string.IsNullOrEmpty(bairro))
                throw new ValidationException("Bairro é obrigatório");

            if (string.IsNullOrEmpty(cidade))
                throw new ValidationException("Cidade é obrigatório");

            if (string.IsNullOrEmpty(estado))
                throw new ValidationException("Estado é obrigatório");
        }
    }
}
