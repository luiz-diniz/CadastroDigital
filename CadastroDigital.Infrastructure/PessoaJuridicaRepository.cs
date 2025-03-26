using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using Dapper;

namespace CadastroDigital.Infrastructure
{
    public class PessoaJuridicaRepository : IPessoaJuridicaRepository
    {
        private readonly DbSession _session;

        public PessoaJuridicaRepository(DbSession session)
        {
            _session = session;
        }

        public async Task AtualizarAsync(PessoaJuridica pessoaJuridica)
        {
            var query = @"UPDATE PessoasJuridicas SET Cnpj = @Cnpj, RazaoSocial = @RazaoSocial, NomeFantasia = @NomeFantasia, 
                                DataAbertura = @DataAbertura, SituacaoCadastral = @SituacaoCadastral WHERE Id = @Id";

            _session.Connection.Execute(query, new
            {
                pessoaJuridica.Id,
                pessoaJuridica.Cnpj,
                pessoaJuridica.RazaoSocial,
                pessoaJuridica.NomeFantasia,
                pessoaJuridica.DataAbertura,
                pessoaJuridica.SituacaoCadastral
            });
        }

        public async Task<int> CriarAsync(PessoaJuridica pessoaJuridica)
        {
            var query = @"INSERT INTO PessoasJuridicas (Cnpj, RazaoSocial, NomeFantasia, DataAbertura, SituacaoCadastral, EnderecoId) 
                            OUTPUT Inserted.Id VALUES (@Cnpj, @RazaoSocial, @NomeFantasia, @DataAbertura, @SituacaoCadastral, @EnderecoId)";

            var result = _session.Connection.QuerySingle<int>(query, new
            {
                pessoaJuridica.Cnpj,
                pessoaJuridica.RazaoSocial,
                pessoaJuridica.NomeFantasia,
                pessoaJuridica.DataAbertura,
                pessoaJuridica.SituacaoCadastral,
                EnderecoId = pessoaJuridica.Endereco.Id
            });

            return result;
        }

        public async Task ExcluirAsync(int id)
        {
            var query = "DELETE FROM PessoasJuridicas WHERE Id = @Id";

            _session.Connection.Execute(query, new { id }, _session.Transaction);
        }

        public async Task<IEnumerable<PessoaJuridica>> ListarAsync()
        {
            var query = @"SELECT PJ.Id, PJ.Cnpj, PJ.RazaoSocial, PJ.NomeFantasia, PJ.DataAbertura, PJ.SituacaoCadastral, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, E.Localidade, E.DDD, E.IBGE
                                FROM PessoasJuridicas PJ
                                LEFT JOIN Enderecos E ON PJ.EnderecoId = E.Id";

            var pessoasFisicas = await _session.Connection.QueryAsync<PessoaJuridica, Endereco, PessoaJuridica>(
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

        public async Task<PessoaJuridica?> ObterAsync(int id)
        {
            var query = @"SELECT PJ.Id, PJ.Cnpj, PJ.RazaoSocial, PJ.NomeFantasia, PJ.DataAbertura, PJ.SituacaoCadastral, 
                                        E.Id AS EnderecoId, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Cidade, E.Estado, E.UF, E.Localidade, E.DDD, E.IBGE
                                FROM PessoasJuridicas PJ
                                LEFT JOIN Enderecos E ON PJ.EnderecoId = E.Id
                                WHERE PJ.Id = @Id";

            var pessoa = _session.Connection.Query<PessoaJuridica, Endereco, PessoaJuridica>(
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

        public async Task<bool> VerificarExistenciaRegistro(PessoaJuridica pessoa)
        {
            var query = "SELECT COUNT(1) FROM PessoasJuridicas WHERE Cnpj = @Cnpj";

            var result = _session.Connection.QuerySingle<int>(query, new { pessoa.Cnpj });

            return result > 0;
        }
    }
}
