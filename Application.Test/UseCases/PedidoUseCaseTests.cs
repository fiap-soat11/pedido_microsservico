using Application.UseCases;
using Domain;
using FluentAssertions;

namespace Application.Test.UseCases
{
    [TestClass]
    public class PedidoUseCaseTests
    {
        private PedidoUseCase _pedidoUseCase;

        [TestInitialize]
        public void Setup()
        {
            _pedidoUseCase = new PedidoUseCase();
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarPedidoVazio_ComCpfValido()
        {
            // Arrange
            var cpf = "12345678900";

            // Act
            var resultado = await _pedidoUseCase.IniciarPedido(cpf);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarPedidoVazio_ComCpfNulo()
        {
            // Arrange
            string cpf = null;

            // Act
            var resultado = await _pedidoUseCase.IniciarPedido(cpf);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }

        [TestMethod]
        public async Task IniciarPedido_DeveRetornarPedidoVazio_ComCpfVazio()
        {
            // Arrange
            var cpf = string.Empty;

            // Act
            var resultado = await _pedidoUseCase.IniciarPedido(cpf);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveRetornarNull()
        {
            // Arrange
            int idPedido = 1;
            int idProduto = 10;
            int quantidade = 2;
            string observacao = "Sem cebola";

            // Act
            var resultado = await _pedidoUseCase.AdicionarProduto(idPedido, idProduto, quantidade, observacao);

            // Assert
            resultado.Should().BeNull();
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveRetornarPedidoVazio()
        {
            // Arrange
            int idPedido = 1;
            int idPedidoProduto = 5;
            int novaQuantidade = 3;
            string observacao = "Com queijo extra";

            // Act
            var resultado = await _pedidoUseCase.AtualizarProduto(idPedido, idPedidoProduto, novaQuantidade, observacao);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }

        [TestMethod]
        public async Task RemoverProduto_DeveRetornarPedidoVazio()
        {
            // Arrange
            int idPedido = 1;
            int idPedidoProduto = 5;

            // Act
            var resultado = await _pedidoUseCase.RemoverProduto(idPedido, idPedidoProduto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarListaVazia()
        {
            // Act
            var resultado = await _pedidoUseCase.ListarPedidos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveRetornarPedidoVazio()
        {
            // Arrange
            int idPedido = 1;
            int novoStatusId = 2;

            // Act
            var resultado = await _pedidoUseCase.AtualizarStatusPedido(idPedido, novoStatusId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<Pedido>();
        }
    }
}
