using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using Dapper;

namespace CadastroDigital.Infrastructure
{
    public class PessoaFisicaRepository : IPessoaFisicaRepository
    {
        private readonly DbSession _session;

        public PessoaFisicaRepository(DbSession session)
        {
            _session = session;
        }

        public async Task AtualizarAsync(PessoaFisica pessoaFisica)
        {
            var query = "UPDATE PessoasFisicas SET Nome = @Nome, Cpf = @Cpf, DataNascimento = @DataNascimento WHERE Id = @Id";

            _session.Connection.Execute(query, new
            {
                pessoaFisica.Id,
                pessoaFisica.Nome,
                pessoaFisica.Cpf,
                pessoaFisica.DataNascimento
            }, _session.Transaction);            
        }

        public async Task<int> CriarAsync(PessoaFisica pessoaFisica)
        {
            var query = "INSERT INTO PessoasFisicas (Nome, Cpf, DataNascimento, EnderecoId) OUTPUT Inserted.Id VALUES (@Nome, @Cpf, @DataNascimento, @EnderecoId)";

            var result = _session.Connection.QuerySingle<int>(query, new
            {
                pessoaFisica.Id,
                pessoaFisica.Nome,
                pessoaFisica.Cpf,
                pessoaFisica.DataNascimento,
                EnderecoId = pessoaFisica.Endereco.Id
            }, _session.Transaction);            

            return result;
        }

        public async Task ExcluirAsync(int id)
        {
            var query = "DELETE FROM PessoasFisicas WHERE Id = @Id";

            _session.Connection.Execute(query, new { id }, _session.Transaction);
        }

        public async Task<IEnumerable<PessoaFisica>> ListarAsync()
        {
            var query = @"SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento,
                         E.Id, E.Cep, E.Logradouro, E.Numero, 
                         E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, 
                         E.Localidade, E.DDD, E.IBGE
                  FROM PessoasFisicas PF
                  LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id";

            var pessoasFisicas = await _session.Connection.QueryAsync<PessoaFisica, Endereco, PessoaFisica>(
                query,
                (pessoa, endereco) =>
                {
                    if (endereco != null)
                    {
                        endereco.IncluirDadosComplementares(endereco.Estado, endereco.UF, endereco.Localidade, endereco.Ddd);
                        pessoa.AtualizarEndereco(endereco);
                    }
                    return pessoa;
                }
            );

            return pessoasFisicas;
        }

        public async Task<PessoaFisica?> ObterAsync(int id)
        {
            var query = @"SELECT PF.Id, PF.Cpf, PF.Nome, PF.DataNascimento, 
                         E.Id, E.Cep, E.Logradouro, E.Numero, 
                         E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, 
                         E.Localidade, E.DDD, E.IBGE
                  FROM PessoasFisicas PF
                  LEFT JOIN Enderecos E ON PF.EnderecoId = E.Id
                        WHERE PF.Id = @Id";

            var pessoa = _session.Connection.Query<PessoaFisica, Endereco, PessoaFisica>(
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
                new { Id = id }
            ).FirstOrDefault();

            return pessoa;
        }

        public async Task<bool> VerificarExistenciaRegistro(PessoaFisica pessoa)
        {
            var query = "SELECT COUNT(1) FROM PessoasFisicas WHERE Cpf = @Cpf";

            var result = _session.Connection.QuerySingle<int>(query, new { pessoa.Cpf });

            return result > 0;
        }
    }
}
