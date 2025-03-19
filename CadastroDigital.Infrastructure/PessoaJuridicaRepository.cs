using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Entities.Enums;
using CadastroDigital.Domain.Ports.Repository;
using Microsoft.Data.SqlClient;

namespace CadastroDigital.Infrastructure
{
    public class PessoaJuridicaRepository : IPessoaJuridicaRepository
    {
        private readonly string _connectionString;

        public PessoaJuridicaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AtualizarAsync(PessoaJuridica pessoaJuridica)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"UPDATE PessoasJuridicas SET Cnpj = @Cnpj, RazaoSocial = @RazaoSocial, NomeFantasia = @NomeFantasia, 
                                DataAbertura = @DataAbertura, SituacaoCadastral = @SituacaoCadastral WHERE Id = @Id";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Id", pessoaJuridica.Id);
            cmd.Parameters.AddWithValue("Cnpj", pessoaJuridica.Cnpj);
            cmd.Parameters.AddWithValue("RazaoSocial", pessoaJuridica.RazaoSocial);
            cmd.Parameters.AddWithValue("NomeFantasia", pessoaJuridica.NomeFantasia);
            cmd.Parameters.AddWithValue("DataAbertura", pessoaJuridica.DataAbertura);
            cmd.Parameters.AddWithValue("SituacaoCadastral", pessoaJuridica.SituacaoCadastral);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> CriarAsync(PessoaJuridica pessoaJuridica)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"INSERT INTO PessoasJuridicas (Cnpj, RazaoSocial, NomeFantasia, DataAbertura, SituacaoCadastral, EnderecoId) 
                            OUTPUT Inserted.Id VALUES (@Cnpj, @RazaoSocial, @NomeFantasia, @DataAbertura, @SituacaoCadastral, @EnderecoId)";

            await using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("Cnpj", pessoaJuridica.Cnpj);
            cmd.Parameters.AddWithValue("RazaoSocial", pessoaJuridica.RazaoSocial);
            cmd.Parameters.AddWithValue("NomeFantasia", pessoaJuridica.NomeFantasia);
            cmd.Parameters.AddWithValue("DataAbertura", pessoaJuridica.DataAbertura);
            cmd.Parameters.AddWithValue("SituacaoCadastral", (int) pessoaJuridica.SituacaoCadastral);
            cmd.Parameters.AddWithValue("EnderecoId", pessoaJuridica.Endereco.Id);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result!;
        }

        public async Task ExcluirAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("DELETE FROM PessoasJuridicas WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<PessoaJuridica>> ListarAsync()
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(@"
                                SELECT PJ.Id, PJ.Cnpj, PJ.RazaoSocial, PJ.NomeFantasia, PJ.DataAbertura, PJ.SituacaoCadastral, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado
                                FROM PessoasJuridicas PJ
                                LEFT JOIN Enderecos E ON PJ.EnderecoId = E.Id", conn);

            var result = await cmd.ExecuteReaderAsync();

            var pessoasJuridicas = new List<PessoaJuridica>();

            while (await result.ReadAsync())
            {
                var pessoa = new PessoaJuridica(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetString(3), result.GetDateTime(4), ObterSituacaoCadastral(result.GetInt32(5)));

                var endereco = new Endereco(result.GetInt32(6), result.GetString(7), result.GetString(8), result.GetInt32(9), result.GetString(10), result.GetString(11), result.GetString(12), result.GetString(13));

                pessoa.AtualizarEndereco(endereco);

                pessoasJuridicas.Add(pessoa);
            }

            return pessoasJuridicas;
        }

        public async Task<PessoaJuridica?> ObterAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(@"
                                SELECT PJ.Id, PJ.Cnpj, PJ.RazaoSocial, PJ.NomeFantasia, PJ.DataAbertura, PJ.SituacaoCadastral, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado
                                FROM PessoasJuridicas PJ
                                LEFT JOIN Enderecos E ON PJ.EnderecoId = E.Id
                                WHERE PJ.Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            var result = await cmd.ExecuteReaderAsync();

            if (result.Read())
            {
                var pessoa = new PessoaJuridica(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetString(3), result.GetDateTime(4), ObterSituacaoCadastral(result.GetInt32(5)));

                var endereco = new Endereco(result.GetInt32(6), result.GetString(7), result.GetString(8), result.GetInt32(9), result.GetString(10), result.GetString(11), result.GetString(12), result.GetString(13));

                pessoa.AtualizarEndereco(endereco);

                return pessoa;
            }

            return null;
        }

        public async Task<bool> VerificarExistenciaRegistro(string cnpj)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("SELECT COUNT(1) FROM PessoasJuridicas WHERE Cnpj = @Cnpj", conn);

            cmd.Parameters.AddWithValue("Cnpj", cnpj);

            var result = await cmd.ExecuteScalarAsync();

            return (int)result! > 0;
        }

        private SituacaoCadastralEnum ObterSituacaoCadastral(int situacao)
        {
            return situacao switch
            {
                1 => SituacaoCadastralEnum.Regular,
                2 => SituacaoCadastralEnum.Ativo,
                3 => SituacaoCadastralEnum.Cancelado,
                4 => SituacaoCadastralEnum.Inativo,
                5 => SituacaoCadastralEnum.Extinto,
                _ => SituacaoCadastralEnum.Desconhecida
            };
        }
    }
}
