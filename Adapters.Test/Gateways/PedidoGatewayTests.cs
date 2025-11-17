using Adapters.Gateways;
using Adapters.Gateways.Interfaces;
using Domain;
using FluentAssertions;
using Moq;

namespace Adapters.Test.Gateways
{
    [TestClass]
    public class PedidoGatewayTests
    {
        private Mock<IDataSource> _dataSourceMock;
        private PedidoGateway _pedidoGateway;

        [TestInitialize]
        public void Setup()
        {
            _dataSourceMock = new Mock<IDataSource>();
            _pedidoGateway = new PedidoGateway(_dataSourceMock.Object);
        }

        [TestMethod]
        public async Task IniciarPedido_DeveChamarDataSource()
        {
            // Arrange
            var cliente = new Cliente
            {
                Cpf = "12345678900",
                Nome = "João Silva"
            };

            var pedidoEsperado = new Pedido { PedidoId = 1 };
            _dataSourceMock.Setup(x => x.IniciarPedido(cliente))
                .ReturnsAsync(pedidoEsperado);

            // Act
            var resultado = await _pedidoGateway.IniciarPedido(cliente);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
            _dataSourceMock.Verify(x => x.IniciarPedido(cliente), Times.Once);
        }

        [TestMethod]
        public async Task BuscarPedidoPorId_DeveChamarDataSource()
        {
            // Arrange
            var pedidoEsperado = new Pedido { PedidoId = 1 };
            _dataSourceMock.Setup(x => x.BuscarPedidoPorId(1))
                .ReturnsAsync(pedidoEsperado);

            // Act
            var resultado = await _pedidoGateway.BuscarPedidoPorId(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
            _dataSourceMock.Verify(x => x.BuscarPedidoPorId(1), Times.Once);
        }

        [TestMethod]
        public async Task ListarPedidos_DeveChamarDataSource()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1 },
                new Pedido { PedidoId = 2 }
            };

            _dataSourceMock.Setup(x => x.ListarPedidos())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoGateway.ListarPedidos();

            // Assert
            resultado.Should().HaveCount(2);
            _dataSourceMock.Verify(x => x.ListarPedidos(), Times.Once);
        }

        [TestMethod]
        public async Task ListarPedidoClienteStatus_DeveChamarDataSource()
        {
            // Arrange
            var pedidos = new List<Pedido> { new Pedido { PedidoId = 1 } };
            _dataSourceMock.Setup(x => x.ListarPedidoClienteStatus())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoGateway.ListarPedidoClienteStatus();

            // Assert
            resultado.Should().HaveCount(1);
            _dataSourceMock.Verify(x => x.ListarPedidoClienteStatus(), Times.Once);
        }

        [TestMethod]
        public async Task ListarPedidoCozinha_DeveChamarDataSource()
        {
            // Arrange
            var pedidos = new List<Pedido> { new Pedido { PedidoId = 1 } };
            _dataSourceMock.Setup(x => x.ListarPedidoCozinha())
                .ReturnsAsync(pedidos);

            // Act
            var resultado = await _pedidoGateway.ListarPedidoCozinha();

            // Assert
            resultado.Should().HaveCount(1);
            _dataSourceMock.Verify(x => x.ListarPedidoCozinha(), Times.Once);
        }

        [TestMethod]
        public async Task CancelarPedido_DeveChamarDataSource()
        {
            // Arrange
            _dataSourceMock.Setup(x => x.CancelarPedido(1))
                .Returns(Task.CompletedTask);

            // Act
            await _pedidoGateway.CancelarPedido(1);

            // Assert
            _dataSourceMock.Verify(x => x.CancelarPedido(1), Times.Once);
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveChamarDataSource()
        {
            // Arrange
            _dataSourceMock.Setup(x => x.FinalizarPedido(1))
                .Returns(Task.CompletedTask);

            // Act
            await _pedidoGateway.FinalizarPedido(1);

            // Assert
            _dataSourceMock.Verify(x => x.FinalizarPedido(1), Times.Once);
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveChamarDataSource()
        {
            // Arrange
            _dataSourceMock.Setup(x => x.AtualizarStatusPedido(2, 1))
                .Returns(Task.CompletedTask);

            // Act
            await _pedidoGateway.AtualizarStatusPedido(2, 1);

            // Assert
            _dataSourceMock.Verify(x => x.AtualizarStatusPedido(2, 1), Times.Once);
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveChamarDataSource()
        {
            // Arrange
            var produto = new Produto { IdProduto = 10, Quantidade = 2 };
            var pedidoAtualizado = new Pedido { PedidoId = 1 };

            _dataSourceMock.Setup(x => x.AdicionarProduto(1, produto))
                .ReturnsAsync(pedidoAtualizado);

            // Act
            var resultado = await _pedidoGateway.AdicionarProduto(1, produto);

            // Assert
            resultado.Should().NotBeNull();
            _dataSourceMock.Verify(x => x.AdicionarProduto(1, produto), Times.Once);
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveChamarDataSource()
        {
            // Arrange
            var produto = new Produto { IdProduto = 10, Quantidade = 5 };
            var pedidoAtualizado = new Pedido { PedidoId = 1 };

            _dataSourceMock.Setup(x => x.AtualizarProduto(1, produto))
                .ReturnsAsync(pedidoAtualizado);

            // Act
            var resultado = await _pedidoGateway.AtualizarProduto(1, produto);

            // Assert
            resultado.Should().NotBeNull();
            _dataSourceMock.Verify(x => x.AtualizarProduto(1, produto), Times.Once);
        }

        [TestMethod]
        public async Task RemoverProduto_DeveChamarDataSource()
        {
            // Arrange
            var pedidoAtualizado = new Pedido { PedidoId = 1 };

            _dataSourceMock.Setup(x => x.RemoverProduto(1, 10))
                .ReturnsAsync(pedidoAtualizado);

            // Act
            var resultado = await _pedidoGateway.RemoverProduto(1, 10);

            // Assert
            resultado.Should().NotBeNull();
            _dataSourceMock.Verify(x => x.RemoverProduto(1, 10), Times.Once);
        }

        [TestMethod]
        public void RecalcularValorTotal_DeveChamarDataSource()
        {
            // Arrange
            var pedido = new Pedido { PedidoId = 1 };
            _dataSourceMock.Setup(x => x.RecalcularValorTotal(pedido));

            // Act
            _pedidoGateway.RecalcularValorTotal(pedido);

            // Assert
            _dataSourceMock.Verify(x => x.RecalcularValorTotal(pedido), Times.Once);
        }

        [TestMethod]
        public void AtualizarPedido_DeveChamarDataSource()
        {
            // Arrange
            var pedido = new Pedido { PedidoId = 1 };
            _dataSourceMock.Setup(x => x.AtualizarPedido(pedido));

            // Act
            _pedidoGateway.AtualizarPedido(pedido);

            // Assert
            _dataSourceMock.Verify(x => x.AtualizarPedido(pedido), Times.Once);
        }
    }
}
