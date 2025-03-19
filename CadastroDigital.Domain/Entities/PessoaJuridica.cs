using CadastroDigital.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CadastroDigital.Domain.Entities
{
    public class PessoaJuridica : PessoaBase
    {
        public string Cnpj { get; private set; }
        public string RazaoSocial { get; private set; }
        public string NomeFantasia { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public SituacaoCadastralEnum SituacaoCadastral { get; private set; }

        public PessoaJuridica(int id, string cnpj, string razaoSocial, string nomeFantasia, DateTime dataAbertura, SituacaoCadastralEnum situacaoCadastral)
        {
            Id = id;

            Validar(cnpj, razaoSocial, nomeFantasia, dataAbertura, situacaoCadastral);

            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            DataAbertura = dataAbertura;
            SituacaoCadastral = situacaoCadastral;
        }

        public PessoaJuridica(int id, string cnpj, string razaoSocial, string nomeFantasia, DateTime dataAbertura, SituacaoCadastralEnum situacaoCadastral, Endereco endereco)
        {
            Id = id;

            Validar(cnpj, razaoSocial, nomeFantasia, dataAbertura, situacaoCadastral);

            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            DataAbertura = dataAbertura;
            SituacaoCadastral = situacaoCadastral;
            Endereco = endereco;
        }

        private void Validar(string cnpj, string razaoSocial, string nomeFantasia, DateTime dataAbertura, SituacaoCadastralEnum situacaoCadastral)
        {
            if (string.IsNullOrEmpty(cnpj))
                throw new ValidationException("CNPJ é obrigatório");

            //TODO: Implementar validação de CNPJ
            if (cnpj.Length != 18)
                throw new ValidationException("CNPJ está inválido");

            if (string.IsNullOrEmpty(razaoSocial))
                throw new ValidationException("Razão Social é obrigatório");

            if (string.IsNullOrEmpty(nomeFantasia))
                throw new ValidationException("Nome Fantasia é obrigatório");

            if (dataAbertura == DateTime.MinValue)
                throw new ValidationException("Data de Abertura é obrigatório");
        }
    }
}