using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CadastroDigital.Infrastructure
{
    public class PessoaFisicaRepository : IPessoaFisicaRepository
    {
        private readonly string _connectionString;

        public PessoaFisicaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AtualizarAsync(PessoaFisica pessoaFisica)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "UPDATE PessoasFisicas SET Nome = @Nome, Cpf = @Cpf, DataNascimento = @DataNascimento WHERE Id = @Id";

            conn.Execute(query, new
            {
                pessoaFisica.Id,
                pessoaFisica.Nome,
                pessoaFisica.Cpf,
                pessoaFisica.DataNascimento
            });            
        }

        public async Task<int> CriarAsync(PessoaFisica pessoaFisica)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "INSERT INTO PessoasFisicas (Nome, Cpf, DataNascimento, EnderecoId) OUTPUT Inserted.Id VALUES (@Nome, @Cpf, @DataNascimento, @EnderecoId)";

            var result = conn.QuerySingle<int>(query, new
            {
                pessoaFisica.Id,
                pessoaFisica.Nome,
                pessoaFisica.Cpf,
                pessoaFisica.DataNascimento,
                EnderecoId = pessoaFisica.Endereco.Id
            });

            return result;
        }

        public async Task ExcluirAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "DELETE FROM PessoasFisicas WHERE Id = @Id";

            conn.Execute(query, new { id });
        }

        public async Task<IEnumerable<PessoaFisica>> ListarAsync()
        {
            await using var conn = new SqlConnection(_connectionString);

            var query = @"SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento, 
                         E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, 
                         E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, 
                         E.Localidade, E.DDD, E.IBGE
                  FROM PessoasFisicas PF
                  LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id";

            var pessoasFisicas = await conn.QueryAsync<PessoaFisica, Endereco, PessoaFisica>(
                query,
                (pessoa, endereco) =>
                {
                    if (endereco != null)
                    {
                        endereco.IncluirDadosComplementares(endereco.Estado, endereco.UF, endereco.Localidade, endereco.Ddd);
                        pessoa.AtualizarEndereco(endereco);
                    }
                    return pessoa;
                },
                splitOn: "EnderecoId"
            );

            return pessoasFisicas;
        }

        public async Task<PessoaFisica?> ObterAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);

            var query = @"SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento, 
                         E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, 
                         E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, 
                         E.Localidade, E.DDD, E.IBGE
                  FROM PessoasFisicas PF
                  LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id
                        WHERE PF.Id = @Id";

            var pessoa = conn.Query<PessoaFisica, Endereco, PessoaFisica>(
                query,
                (pessoa, endereco) =>
                {
                    if (endereco != null)
                    {
                        endereco.IncluirDadosComplementares(endereco.Estado, endereco.UF, endereco.Localidade, endereco.Ddd);
                        pessoa.AtualizarEndereco(endereco);
                    }

                    return pessoa;
                },
                new { Id = id },
                splitOn: "EnderecoId"
            ).FirstOrDefault();

            return pessoa;
        }

        public async Task<bool> VerificarExistenciaRegistro(PessoaFisica pessoa)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("SELECT COUNT(1) FROM PessoasFisicas WHERE Cpf = @Cpf", conn);

            cmd.Parameters.AddWithValue("Cpf", pessoa.Cpf);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result! > 0;
        }
    }
}
