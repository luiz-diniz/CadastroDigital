using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
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

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Id", pessoaFisica.Id);
            cmd.Parameters.AddWithValue("Nome", pessoaFisica.Nome);
            cmd.Parameters.AddWithValue("Cpf", pessoaFisica.Cpf);
            cmd.Parameters.AddWithValue("DataNascimento", pessoaFisica.DataNascimento);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> CriarAsync(PessoaFisica pessoaFisica)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "INSERT INTO PessoasFisicas (Nome, Cpf, DataNascimento, EnderecoId) OUTPUT Inserted.Id VALUES (@Nome, @Cpf, @DataNascimento, @EnderecoId)";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Nome", pessoaFisica.Nome);
            cmd.Parameters.AddWithValue("Cpf", pessoaFisica.Cpf);
            cmd.Parameters.AddWithValue("DataNascimento", pessoaFisica.DataNascimento);
            cmd.Parameters.AddWithValue("EnderecoId", pessoaFisica.Endereco.Id);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result!;
        }

        public async Task ExcluirAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("DELETE FROM PessoasFisicas WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<PessoaFisica>> ListarAsync()
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(@"
                                SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado
                                FROM PessoasFisicas PF
                                LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id", conn);

            var result = await cmd.ExecuteReaderAsync();

            var pessoasFisicas = new List<PessoaFisica>();

            while (await result.ReadAsync())
            {
                var pessoa = new PessoaFisica(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetDateTime(3));

                var endereco = new Endereco(result.GetInt32(4), result.GetString(5), result.GetString(6), result.GetInt32(7), result.GetString(8), result.GetString(9), result.GetString(10), result.GetString(11));

                pessoa.AtualizarEndereco(endereco);

                pessoasFisicas.Add(pessoa);
            }

            return pessoasFisicas;
        }

        public async Task<PessoaFisica?> ObterAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(@"SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado
                                    FROM PessoasFisicas PF
                                    LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id
                                    WHERE PF.Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            var result = await cmd.ExecuteReaderAsync();

            if (result.Read())
            {
                var pessoa = new PessoaFisica(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetDateTime(3));

                var endereco = new Endereco(result.GetInt32(4), result.GetString(5), result.GetString(6), result.GetInt32(7), result.GetString(8), result.GetString(9), result.GetString(10), result.GetString(11));

                pessoa.AtualizarEndereco(endereco);

                return pessoa;
            }

            return null;
        }

        public async Task<bool> VerificarExistenciaRegistro(string cpf)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("SELECT COUNT(1) FROM PessoasFisicas WHERE Cpf = @Cpf", conn);

            cmd.Parameters.AddWithValue("Cpf", cpf);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result! > 0;
        }
    }
}
