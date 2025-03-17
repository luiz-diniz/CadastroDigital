using System.ComponentModel.DataAnnotations;

namespace CadastroDigital.Domain.Entities
{
    public class PessoaJuridica : Pessoa
    {
        public string Cnpj { get; private set; }
        public string RazaoSocial { get; private set; }
        public string NomeFantasia { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public Enum SituacaoCadastral { get; private set; }

        public PessoaJuridica(string cnpj, string razaoSocial, string nomeFantasia, DateTime dataAbertura, Enum situacaoCadastral)
        {
            Validar(cnpj, razaoSocial, nomeFantasia, dataAbertura, situacaoCadastral);

            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            DataAbertura = dataAbertura;
            SituacaoCadastral = situacaoCadastral;
        }

        private void Validar(string cnpj, string razaoSocial, string nomeFantasia, DateTime dataAbertura, Enum situacaoCadastral)
        {
            if (string.IsNullOrEmpty(cnpj))
                throw new ValidationException("CNPJ é obrigatório");

            //TODO: Implementar validação de CNPJ
            if (cnpj.Length != 14)
                throw new ValidationException("CNPJ está inválido");

            if (string.IsNullOrEmpty(razaoSocial))
                throw new ValidationException("Razão Social é obrigatório");

            if (string.IsNullOrEmpty(nomeFantasia))
                throw new ValidationException("Nome Fantasia é obrigatório");

            if (dataAbertura == DateTime.MinValue)
                throw new ValidationException("Data de Abertura é obrigatório");

            if (situacaoCadastral == null)
                throw new ValidationException("Situação Cadastral é obrigatório");
        }
    }
}