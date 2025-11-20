using Application.Configurations;
using DataSource.Repositories.Interfaces;
using Domain;
using Domain.Enums;
using FluentAssertions;
using Moq;

namespace DataSource.Test
{
    [TestClass]
    public class DataSourceTests
    {
        private Mock<IPedidoRepository> _pedidoRepositoryMock;
        private DataSource _dataSource;

        [TestInitialize]
        public void Setup()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _dataSource = new DataSource(_pedidoRepositoryMock.Object);
        }

        [TestMethod]
        public async Task IniciarPedido_DeveCriarPedidoComStatusIniciado()
        {
            // Arrange
            var cliente = new Cliente
            {
                Cpf = "12345678900",
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            // Act
            var resultado = await _dataSource.IniciarPedido(cliente);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Cliente.Should().Be(cliente);
            resultado.StatusAtual.Should().Be((int)StatusPedidoEnum.Iniciado);
            resultado.ValorTotal.Should().Be(0);
            _pedidoRepositoryMock.Verify(x => x.Inserir(It.IsAny<Pedido>()), Times.Once);
        }

        [TestMethod]
        public async Task IniciarPedido_DeveCriarPedidoSemCliente()
        {
            // Act
            var resultado = await _dataSource.IniciarPedido(null);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Cliente.Should().BeNull();
            resultado.StatusAtual.Should().Be((int)StatusPedidoEnum.Iniciado);
        }

        [TestMethod]
        public async Task BuscarPedidoPorId_DeveRetornarPedido()
        {
            // Arrange
            var pedido = new Pedido { PedidoId = 1 };
            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            var resultado = await _dataSource.BuscarPedidoPorId(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PedidoId.Should().Be(1);
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveAtualizarStatusComSucesso()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Iniciado
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            await _dataSource.AtualizarStatusPedido((int)StatusPedidoEnum.Recebido, 1);

            // Assert
            _pedidoRepositoryMock.Verify(x => x.Atualizar(It.IsAny<Pedido>()), Times.Once);
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(It.IsAny<int>()))
                .Returns((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.AtualizarStatusPedido(2, 999));
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveLancarException_QuandoPedidoFinalizado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Finalizado
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.AtualizarStatusPedido(2, 1));
        }

        [TestMethod]
        public async Task AtualizarStatusPedido_DeveLancarException_QuandoPedidoCancelado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Cancelado
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.AtualizarStatusPedido(2, 1));
        }

        [TestMethod]
        public async Task CancelarPedido_DeveAtualizarStatusParaCancelado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            await _dataSource.CancelarPedido(1);

            // Assert
            pedido.StatusAtual.Should().Be((int)StatusPedidoEnum.Cancelado);
            _pedidoRepositoryMock.Verify(x => x.Atualizar(pedido), Times.Once);
        }

        [TestMethod]
        public async Task CancelarPedido_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(It.IsAny<int>()))
                .Returns((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.CancelarPedido(999));
        }

        [TestMethod]
        public async Task CancelarPedido_DeveLancarException_QuandoPedidoJaFinalizado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Finalizado
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.CancelarPedido(1));
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveAtualizarStatusParaFinalizado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Entregue
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            await _dataSource.FinalizarPedido(1);

            // Assert
            pedido.StatusAtual.Should().Be((int)StatusPedidoEnum.Finalizado);
            _pedidoRepositoryMock.Verify(x => x.Atualizar(pedido), Times.Once);
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(It.IsAny<int>()))
                .Returns((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.FinalizarPedido(999));
        }

        [TestMethod]
        public async Task FinalizarPedido_DeveLancarException_QuandoPedidoJaCancelado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Cancelado
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.FinalizarPedido(1));
        }

        [TestMethod]
        public async Task ListarPedidos_DeveRetornarPedidosOrdenados()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.Recebido, DataPedido = "2025-11-13 10:00:00" },
                new Pedido { PedidoId = 2, StatusAtual = (int)StatusPedidoEnum.Entregue, DataPedido = "2025-11-13 09:00:00" },
                new Pedido { PedidoId = 3, StatusAtual = (int)StatusPedidoEnum.EmPreparacao, DataPedido = "2025-11-13 08:00:00" }
            };

            _pedidoRepositoryMock.Setup(x => x.ListarTodos())
                .Returns(pedidos);

            // Act
            var resultado = await _dataSource.ListarPedidos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.First().StatusAtual.Should().Be((int)StatusPedidoEnum.Entregue);
        }

        [TestMethod]
        public async Task ListarPedidos_NaoDeveRetornarPedidosFinalizados()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, StatusAtual = (int)StatusPedidoEnum.Recebido, DataPedido = "2025-11-13 10:00:00" },
                new Pedido { PedidoId = 2, StatusAtual = (int)StatusPedidoEnum.Finalizado, DataPedido = "2025-11-13 09:00:00" },
                new Pedido { PedidoId = 3, StatusAtual = (int)StatusPedidoEnum.Cancelado, DataPedido = "2025-11-13 08:00:00" }
            };

            _pedidoRepositoryMock.Setup(x => x.ListarTodos())
                .Returns(pedidos);

            // Act
            var resultado = await _dataSource.ListarPedidos();

            // Assert
            resultado.Should().HaveCount(1);
            resultado.First().PedidoId.Should().Be(1);
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveAdicionarNovoProduto()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
            };

            var produto = new Produto
            {
                IdProduto = 10,
                Quantidade = 2,
                PrecoUnitario = 25.50m,
                Observacao = "Sem cebola"
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            var resultado = await _dataSource.AdicionarProduto(1, produto);

            // Assert
            resultado.Produtos.Should().HaveCount(1);
            resultado.Produtos.First().IdProduto.Should().Be(10);
            _pedidoRepositoryMock.Verify(x => x.Atualizar(It.IsAny<Pedido>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveAtualizarQuantidadeSeJaExiste()
        {
            // Arrange
            var produtoExistente = new Produto
            {
                IdProduto = 10,
                Quantidade = 2,
                PrecoUnitario = 25.50m
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto> { produtoExistente }
            };

            var novoProduto = new Produto
            {
                IdProduto = 10,
                Quantidade = 3,
                Observacao = "Sem tomate"
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            var resultado = await _dataSource.AdicionarProduto(1, novoProduto);

            // Assert
            resultado.Produtos.Should().HaveCount(1);
            resultado.Produtos.First().Quantidade.Should().Be(5);
            resultado.Produtos.First().Observacao.Should().Be("Sem tomate");
        }

        [TestMethod]
        public async Task AdicionarProduto_DeveLancarException_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            var produto = new Produto { IdProduto = 10, Quantidade = 2 };
            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(It.IsAny<int>()))
                .Returns((Pedido)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.AdicionarProduto(999, produto));
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveAtualizarQuantidadeEObservacao()
        {
            // Arrange
            var produtoExistente = new Produto
            {
                IdProduto = 10,
                Quantidade = 2,
                PrecoUnitario = 25.50m,
                Observacao = "Sem cebola"
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto> { produtoExistente }
            };

            var produtoAtualizado = new Produto
            {
                IdProduto = 10,
                Quantidade = 5,
                Observacao = "Com queijo extra"
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            var resultado = await _dataSource.AtualizarProduto(1, produtoAtualizado);

            // Assert
            resultado.Produtos.First().Quantidade.Should().Be(5);
            resultado.Produtos.First().Observacao.Should().Be("Com queijo extra");
        }

        [TestMethod]
        public async Task AtualizarProduto_DeveLancarException_QuandoProdutoNaoEncontrado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
            };

            var produto = new Produto { IdProduto = 999, Quantidade = 5 };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.AtualizarProduto(1, produto));
        }

        [TestMethod]
        public async Task RemoverProduto_DeveRemoverProdutoComSucesso()
        {
            // Arrange
            var produto = new Produto
            {
                IdProduto = 10,
                Quantidade = 2,
                PrecoUnitario = 25.50m
            };

            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto> { produto }
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act
            var resultado = await _dataSource.RemoverProduto(1, 10);

            // Assert
            resultado.Produtos.Should().BeEmpty();
        }

        [TestMethod]
        public async Task RemoverProduto_DeveLancarException_QuandoProdutoNaoEncontrado()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
            };

            _pedidoRepositoryMock.Setup(x => x.BuscarPorId(1))
                .Returns(pedido);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessException>(
                async () => await _dataSource.RemoverProduto(1, 999));
        }

        [TestMethod]
        public void RecalcularValorTotal_DeveCalcularCorretamente()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>
                {
                    new Produto { IdProduto = 1, Quantidade = 2, PrecoUnitario = 10.50m },
                    new Produto { IdProduto = 2, Quantidade = 3, PrecoUnitario = 15.00m }
                }
            };

            // Act
            _dataSource.RecalcularValorTotal(pedido);

            // Assert
            pedido.ValorTotal.Should().Be(66.00m);
        }

        [TestMethod]
        public void RecalcularValorTotal_DeveRetornarZero_QuandoSemProdutos()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
            };

            // Act
            _dataSource.RecalcularValorTotal(pedido);

            // Assert
            pedido.ValorTotal.Should().Be(0m);
        }

        [TestMethod]
        public void RecalcularValorTotal_DeveLancarException_QuandoPedidoNulo()
        {
            // Act & Assert
            Assert.ThrowsException<BusinessException>(
                () => _dataSource.RecalcularValorTotal(null));
        }

        [TestMethod]
        public void AtualizarPedido_DeveChamarRepositorio()
        {
            // Arrange
            var pedido = new Pedido { PedidoId = 1 };

            // Act
            _dataSource.AtualizarPedido(pedido);

            // Assert
            _pedidoRepositoryMock.Verify(x => x.Atualizar(pedido), Times.Once);
        }
    }
}
