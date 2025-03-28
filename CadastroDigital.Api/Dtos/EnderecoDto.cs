﻿namespace CadastroDigital.Api.Dtos
{
    public class EnderecoDto
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string? UF { get; set; }
        public string? Localidade { get; set; }
        public string? Ddd { get; set; }
        public string? Ibge { get; set; }
    }
}