using Adapters.Controllers;
using Adapters.Gateways.Interfaces;
using Adapters.Presenters.Pedido;
using Application.Configurations;
using Domain;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Adapters.Test.Controllers
{
    [TestClass]
    public class PedidoControllerTests
    {
        private Mock<ILogger<PedidoController>> _loggerMock;
        private Mock<IPedidoGateway> _pedidoGatewayMock;
        private PedidoController _pedidoController;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PedidoController>>();
            _pedidoGatewayMock = new Mock<IPedidoGateway>();
            _pedidoController = new PedidoController(_loggerMock.Object, _pedidoGatewayMock.Object);
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarPedido_QuandoClienteValido()
        {
            // Arrange
            var clienteRequest = new ClienteRequest
            {
                Cpf = "12345678900",
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            var pedidoEsperado = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Iniciado
            };

            _pedidoGatewayMock.Setup(x => x.IniciarPedido(It.IsAny<Cliente>()))
                .ReturnsAsync(pedidoEsperado);

            // Act
            var resultado = await _pedidoController.IniciarPedido(clienteRequest);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
            _pedidoGatewayMock.Verify(x => x.IniciarPedido(It.IsAny<Cliente>()), Times.Once);
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarPedido_QuandoClienteNulo()
        {
            // Arrange
            var pedidoEsperado = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Iniciado
            };

            _pedidoGatewayMock.Setup(x => x.IniciarPedido(null))
                .ReturnsAsync(pedidoEsperado);

            // Act
            var resultado = await _pedidoController.IniciarPedido(null);

            // Assert
            resultado.Should().NotBeNull();
            _pedidoGatewayMock.Verify(x => x.IniciarPedido(null), Times.Once);
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 10,
                Quantidade = 2,
                Observacao = "Sem cebola"
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(It.IsAny<int>()))
                .ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _pedidoController.AdicionarProduto(1, request));
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveRetornarPedidoAtualizado_QuandoSucesso()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 10,
                Quantidade = 2,
                Observacao = "Sem cebola"
            };

            var pedidoExistente = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
            };

            var pedidoAtualizado = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>
                {
                    new Produto { IdProduto = 10, Quantidade = 2 }
                }
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedidoExistente)
                .Callback(() =>
                {
                    _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                        .ReturnsAsync(pedidoAtualizado);
                });

            _pedidoGatewayMock.Setup(x => x.AdicionarProduto(1, It.IsAny<Produto>()))
                .ReturnsAsync(pedidoAtualizado);

            // Act
            var resultado = await _pedidoController.AdicionarProduto(1, request);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            var request = new AtualizarProdutoPedidoRequest
            {
                NovaQuantidade = 5,
                Observacao = "Com queijo extra"
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(It.IsAny<int>()))
                .ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _pedidoController.AtualizarProduto(1, request));
        }

        [TestMethod]
        public async Task RemoverProduto_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(It.IsAny<int>()))
                .ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _pedidoController.RemoverProduto(1, 10));
        }

        [TestMethod]
        public async Task BuscarPedidoPorId_DeveRetornarPedido_QuandoEncontrado()
        {
            // Arrange
            var pedidoEsperado = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedidoEsperado);

            // Act
            var resultado = await _pedidoController.BuscarPedidoPorId(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
        }

        [TestMethod]
        public async Task BuscarPedidoPorId_DeveLancarException_QuandoNaoEncontrado()
        {
            // Arrange
            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(It.IsAny<int>()))
                .ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _pedidoController.BuscarPedidoPorId(999));
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedido);

            _pedidoGatewayMock.Setup(x => x.AtualizarStatusPedido(2, 1))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _pedidoController.AtualizarStatusPedido(2, 1);

            // Assert
            resultado.Should().BeTrue();
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarFalse_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(It.IsAny<int>()))
                .ReturnsAsync((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _pedidoController.AtualizarStatusPedido(2, 999));
        }

        [TestMethod]
        public async Task CancelarPedido_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedido);

            _pedidoGatewayMock.Setup(x => x.CancelarPedido(1))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _pedidoController.CancelarPedido(1);

            // Assert
            resultado.Should().BeTrue();
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Entregue
            };

            _pedidoGatewayMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedido);

            _pedidoGatewayMock.Setup(x => x.FinalizarPedido(1))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _pedidoController.FinalizarPedido(1);

            // Assert
            resultado.Should().BeTrue();
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarListaDePedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1 },
                new Pedido { PedidoId = 2 }
            };

            _pedidoGatewayMock.Setup(x => x.ListarPedidos())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoController.ListarPedidos();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task ListarPedidoClienteStatus_DeveRetornarListaDePedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.Recebido },
                new Pedido { PedidoId = 2, StatusAtual = (int)StatusPedidoEnum.EmPreparacao }
            };

            _pedidoGatewayMock.Setup(x => x.ListarPedidoClienteStatus())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoController.ListarPedidoClienteStatus();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task ListarPedidoCozinha_DeveRetornarListaDePedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.EmPreparacao }
            };

            _pedidoGatewayMock.Setup(x => x.ListarPedidoCozinha())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoController.ListarPedidoCozinha();

            // Assert
            resultado.Should().HaveCount(1);
        }
    }
}
