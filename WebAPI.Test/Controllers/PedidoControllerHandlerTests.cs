using Adapters.Controllers.Interfaces;
using Adapters.Presenters.DTOs;
using Adapters.Presenters.Pedido;
using Application.Configurations;
using Domain;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Controllers;

namespace WebAPI.Test.Controllers
{
    [TestClass]
    public class PedidoControllerHandlerTests
    {
        private Mock<ILogger<PedidoControllerHandler>> _loggerMock;
        private Mock<IPedidoController> _pedidoControllerMock;
        private PedidoControllerHandler _controller;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PedidoControllerHandler>>();
            _pedidoControllerMock = new Mock<IPedidoController>();
            _controller = new PedidoControllerHandler(_loggerMock.Object, _pedidoControllerMock.Object);
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarOk_QuandoExistemPedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido
                {
                    PedidoId = 1,
                    StatusAtual = (int)StatusPedidoEnum.Recebido,
                    ValorTotal = 100.50m,
                    DataPedido = "2025-11-13 10:00:00",
                    Cliente = new Cliente { Cpf = "12345678900" }
                },
                new Pedido
                {
                    PedidoId = 2,
                    StatusAtual = (int)StatusPedidoEnum.EmPreparacao,
                    ValorTotal = 75.00m,
                    DataPedido = "2025-11-13 11:00:00",
                    Cliente = new Cliente { Cpf = "98765432100" }
                }
            };

            _pedidoControllerMock.Setup(x => x.ListarPedidos())
                .ReturnsAsync(pedidos);

            // Act
            var result = await _controller.ListarPedidos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().NotBeNull();
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarNotFound_QuandoNaoExistemPedidos()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.ListarPedidos())
                .ReturnsAsync(new List<Pedido>());

            // Act
            var result = await _controller.ListarPedidos();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarNotFound_QuandoPedidosNulo()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.ListarPedidos())
                .ReturnsAsync((IEnumerable<Pedido>)null);

            // Act
            var result = await _controller.ListarPedidos();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ListarPedidosClientes_DeveRetornarOk_QuandoExistemPedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.Recebido }
            };

            _pedidoControllerMock.Setup(x => x.ListarPedidoClienteStatus())
                .ReturnsAsync(pedidos);

            // Act
            var result = await _controller.ListarPedidosClientes();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ListarPedidosClientes_DeveRetornarNotFound_QuandoListaVazia()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.ListarPedidoClienteStatus())
                .ReturnsAsync(new List<Pedido>());

            // Act
            var result = await _controller.ListarPedidosClientes();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ListarPedidosCozinha_DeveRetornarOk_QuandoExistemPedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.EmPreparacao }
            };

            _pedidoControllerMock.Setup(x => x.ListarPedidoCozinha())
                .ReturnsAsync(pedidos);

            // Act
            var result = await _controller.ListarPedidosCozinha();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var clienteRequest = new ClienteRequest
            {
                Cpf = "12345678900",
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Iniciado
            };

            _pedidoControllerMock.Setup(x => x.IniciarPedido(It.IsAny<ClienteRequest>()))
                .ReturnsAsync(pedido);

            // Act
            var result = await _controller.IniciarPedido(clienteRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().NotBeNull();
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.IniciarPedido(It.IsAny<ClienteRequest>()))
                .ThrowsAsync(new Exception("Erro ao iniciar pedido"));

            // Act
            var result = await _controller.IniciarPedido(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var request = new PedidoRequest { StatusId = 2 };

            _pedidoControllerMock.Setup(x => x.AtualizarStatusPedido(2, 1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AtualizarStatusPedido(1, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Status do pedido atualizado com sucesso");
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            var request = new PedidoRequest { StatusId = 2 };

            _pedidoControllerMock.Setup(x => x.AtualizarStatusPedido(2, 999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.AtualizarStatusPedido(999, request);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            var request = new PedidoRequest { StatusId = 2 };

            _pedidoControllerMock.Setup(x => x.AtualizarStatusPedido(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao atualizar"));

            // Act
            var result = await _controller.AtualizarStatusPedido(1, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.FinalizarPedido(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.FinalizarPedido(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Pedido finalizado com sucesso");
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveRetornarNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.FinalizarPedido(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.FinalizarPedido(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.FinalizarPedido(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao finalizar"));

            // Act
            var result = await _controller.FinalizarPedido(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task CancelarPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.CancelarPedido(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CancelarPedido(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Pedido cancelado com sucesso");
        }

        [TestMethod]
        public async Task CancelarPedido_DeveRetornarNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.CancelarPedido(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CancelarPedido(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task CancelarPedido_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.CancelarPedido(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao cancelar"));

            // Act
            var result = await _controller.CancelarPedido(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 10,
                Quantidade = 2,
                Observacao = "Sem cebola"
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido,
                ValorTotal = 50.00m,
                DataPedido = "2025-11-13 10:00:00",
                Cliente = new Cliente { Cpf = "12345678900" }
            };

            _pedidoControllerMock.Setup(x => x.AdicionarProduto(1, request))
                .ReturnsAsync(pedido);

            // Act
            var result = await _controller.AdicionarProduto(1, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveRetornarBadRequest_QuandoBusinessException()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 10,
                Quantidade = 2
            };

            _pedidoControllerMock.Setup(x => x.AdicionarProduto(It.IsAny<int>(), It.IsAny<AdicionarProdutoPedidoRequest>()))
                .ThrowsAsync(new BusinessException("Pedido não encontrado"));

            // Act
            var result = await _controller.AdicionarProduto(999, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var request = new AtualizarProdutoPedidoRequest
            {
                NovaQuantidade = 5,
                Observacao = "Com queijo extra"
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido,
                ValorTotal = 125.00m,
                DataPedido = "2025-11-13 10:00:00",
                Cliente = new Cliente { Cpf = "12345678900" }
            };

            _pedidoControllerMock.Setup(x => x.AtualizarProduto(1, request))
                .ReturnsAsync(pedido);

            // Act
            var result = await _controller.AtualizarProduto(1, 10, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveRetornarBadRequest_QuandoBusinessException()
        {
            // Arrange
            var request = new AtualizarProdutoPedidoRequest
            {
                NovaQuantidade = 5
            };

            _pedidoControllerMock.Setup(x => x.AtualizarProduto(It.IsAny<int>(), It.IsAny<AtualizarProdutoPedidoRequest>()))
                .ThrowsAsync(new BusinessException("Produto não encontrado"));

            // Act
            var result = await _controller.AtualizarProduto(1, 999, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task RemoverProduto_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido,
                ValorTotal = 50.00m,
                DataPedido = "2025-11-13 10:00:00",
                Cliente = new Cliente { Cpf = "12345678900" }
            };

            _pedidoControllerMock.Setup(x => x.RemoverProduto(1, 10))
                .ReturnsAsync(pedido);

            // Act
            var result = await _controller.RemoverProduto(1, 10);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task RemoverProduto_DeveRetornarBadRequest_QuandoBusinessException()
        {
            // Arrange
            _pedidoControllerMock.Setup(x => x.RemoverProduto(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new BusinessException("Produto não encontrado no pedido"));

            // Act
            var result = await _controller.RemoverProduto(1, 999);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
