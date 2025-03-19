using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CadastroDigital.Infrastructure
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly string _connectionString;

        public EnderecoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AtualizarAsync(Endereco endereco)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"UPDATE Enderecos SET Cep = @Cep, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, Cidade = @Cidade, Estado = @Estado WHERE Id = @Id";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Id", endereco.Id);
            cmd.Parameters.AddWithValue("Cep", endereco.Cep);
            cmd.Parameters.AddWithValue("Logradouro", endereco.Logradouro);
            cmd.Parameters.AddWithValue("Numero", endereco.Numero);
            cmd.Parameters.AddWithValue("Complemento", endereco.Complemento);
            cmd.Parameters.AddWithValue("Bairro", endereco.Bairro);
            cmd.Parameters.AddWithValue("Cidade", endereco.Cidade);
            cmd.Parameters.AddWithValue("Estado", endereco.Estado);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> CriarAsync(Endereco endereco)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"INSERT INTO Enderecos (Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado) OUTPUT Inserted.Id 
                            VALUES (@Cep, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @Estado)";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Cep", endereco.Cep);
            cmd.Parameters.AddWithValue("Logradouro", endereco.Logradouro);
            cmd.Parameters.AddWithValue("Numero", endereco.Numero);
            cmd.Parameters.AddWithValue("Complemento", endereco.Complemento);
            cmd.Parameters.AddWithValue("Bairro", endereco.Bairro);
            cmd.Parameters.AddWithValue("Cidade", endereco.Cidade);
            cmd.Parameters.AddWithValue("Estado", endereco.Estado);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result!;
        }

        public async Task ExcluirAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "DELETE FROM Enderecos WHERE Id = @Id";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}