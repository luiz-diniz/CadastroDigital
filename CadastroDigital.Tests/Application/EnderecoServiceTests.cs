using CadastroDigital.Application.Services;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CadastroDigital.Tests.Application
{
    public class EnderecoServiceTests
    {
        private readonly Mock<ILogger<EnderecoService>> _loggerMock;
        private readonly Mock<IEnderecoRepository> _enderecoRepositoryMock;
        private readonly Mock<IIntegracaoCep> _integracaoCepServiceMock;

        private readonly Endereco _endereco;

        public EnderecoServiceTests()
        {
            _loggerMock = new Mock<ILogger<EnderecoService>>(MockBehavior.Loose);
            _enderecoRepositoryMock = new Mock<IEnderecoRepository>(MockBehavior.Strict);
            _integracaoCepServiceMock = new Mock<IIntegracaoCep>(MockBehavior.Strict);

            _endereco = new Endereco(1, "12345678", "Rua Teste", 123, "Complemento", "Bairro", "Cidade", "Estado");
        }       

        [Fact]
        public async Task EnderecoService_AtualizarEnderecoComValorNulo_ArgumentNullException()
        {         
            await Assert.ThrowsAsync<ArgumentNullException>(() => ObterEnderecoService().AtualizarAsync(null));

            VerificarMocks();
        }

        public void VerificarMocks()
        {
            _loggerMock.VerifyAll();
            _enderecoRepositoryMock.VerifyAll();
            _integracaoCepServiceMock.VerifyAll();
        }

        public EnderecoService ObterEnderecoService()
        {
            return new EnderecoService(_loggerMock.Object, _enderecoRepositoryMock.Object, _integracaoCepServiceMock.Object);
        }
    }
}
