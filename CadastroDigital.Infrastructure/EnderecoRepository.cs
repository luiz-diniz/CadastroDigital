using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CadastroDigital.Infrastructure
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly DbSession _session;

        public EnderecoRepository(DbSession session)
        {
            _session = session;
        }

        public async Task AtualizarAsync(Endereco endereco)
        {
            var query = @"UPDATE Enderecos SET Cep = @Cep, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, 
                                               Bairro = @Bairro, Cidade = @Cidade, Estado = @Estado WHERE Id = @Id";

            _session.Connection.Execute(query, new
            {
                endereco.Id,
                endereco.Cep,
                endereco.Logradouro,
                endereco.Numero,
                endereco.Complemento,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado,
            }, _session.Transaction);
        }

        public async Task<int> CriarAsync(Endereco endereco)
        {
            var query = @"INSERT INTO Enderecos (Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado, UF, Localidade, DDD, IBGE) OUTPUT Inserted.Id 
                            VALUES (@Cep, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @UF, @Localidade, @DDD, @IBGE)";

            var result = _session.Connection.QuerySingle<int>(query, new
            {
                endereco.Cep,
                endereco.Logradouro,
                endereco.Numero,
                endereco.Complemento,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado,
                endereco.UF,
                endereco.Localidade,
                endereco.Ddd,
                endereco.Ibge,
            }, _session.Transaction);
            
            return result;
        }

        public async Task ExcluirAsync(int id)
        {
            var query = "DELETE FROM Enderecos WHERE Id = @Id";

            _session.Connection.Execute(query, new { id }, _session.Transaction);
        }
    }
}