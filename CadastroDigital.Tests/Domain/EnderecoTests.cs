using CadastroDigital.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CadastroDigital.Tests.Domain
{
    public class EnderecoTests
    {
        [Fact]
        public void Endereco_CriarEnderecoCepNulo_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, null, "Rua Teste", 123, "Complemento", "Bairro Teste", "Cidade Teste", "Estado Teste"));
        }

        [Fact]
        public void Endereco_CriarEnderecoCepVazio_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "", "Rua Teste", 123, "Complemento", "Bairro Teste", "Cidade Teste", "Estado Teste"));
        }

        [Fact]
        public void Endereco_CriarEnderecoCepInvalido_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "0000000a", "Rua Teste", 123, "Complemento", "Bairro Teste", "Cidade Teste", "Estado Teste"));
        }

        [Fact]
        public void Endereco_CriarEnderecoLogradouroNulo_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", null, 123, "Complemento", "Bairro", "Cidade", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoLogradouroVazio_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "", 123, "Complemento", "Bairro", "Cidade", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoNumeroNegativo_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", -1, "Complemento", "Bairro", "Cidade", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoBairroNulo_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", null, "Cidade", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoBairroVazio_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", "", "Cidade", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoCidadeNula_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", "Bairro", null, "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoCidadeVazia_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", "Bairro", "", "Estado"));
        }

        [Fact]
        public void Endereco_CriarEnderecoEstadoNulo_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", "Bairro", "Cidade", null));
        }

        [Fact]
        public void Endereco_CriarEnderecoEstadoVazio_ValidationException()
        {
            Assert.Throws<ValidationException>(() => new Endereco(1, "00000000", "logradouro", 1, "Complemento", "Bairro", "Cidade", ""));
        }

        [Fact]
        public void Endereco_CriarEnderecoValido_EnderecoCriado()
        {
            var endereco = new Endereco(1, "00000000", "logradouro", 1, "Complemento", "Bairro", "Cidade", "Estado");

            Assert.Equal(1, endereco.Id);
            Assert.Equal("00000000", endereco.Cep);
            Assert.Equal("logradouro", endereco.Logradouro);
            Assert.Equal(1, endereco.Numero);
            Assert.Equal("Complemento", endereco.Complemento);
            Assert.Equal("Bairro", endereco.Bairro);
            Assert.Equal("Cidade", endereco.Cidade);
            Assert.Equal("Estado", endereco.Estado);
        }
    }
}