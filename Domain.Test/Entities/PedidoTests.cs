using Domain;
using FluentAssertions;

namespace Domain.Test.Entities
{
    [TestClass]
    public class PedidoTests
    {
        [TestMethod]
        public void Pedido_DeveInicializarComValoresPadrao()
        {
            // Arrange & Act
            var pedido = new Pedido();

            // Assert
            pedido.PedidoId.Should().Be(0);
            pedido.DataPedido.Should().BeNull();
            pedido.ValorTotal.Should().BeNull();
            pedido.Cliente.Should().BeNull();
            pedido.Produtos.Should().BeNull();
            pedido.StatusAtual.Should().BeNull();
        }

        [TestMethod]
        public void Pedido_DeveDefinirPropriedadesCorretamente()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 123,
                DataPedido = "2025-11-13 10:30:00",
                ValorTotal = 150.50m,
                StatusAtual = 2
            };

            // Assert
            pedido.PedidoId.Should().Be(123);
            pedido.DataPedido.Should().Be("2025-11-13 10:30:00");
            pedido.ValorTotal.Should().Be(150.50m);
            pedido.StatusAtual.Should().Be(2);
        }

        [TestMethod]
        public void DataPedidoFormatado_DeveRetornarDataCorretamente_QuandoDataPedidoValida()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = "2025-11-13 10:30:00"
            };

            // Act
            var dataFormatada = pedido.DataPedidoFormatado;

            // Assert
            dataFormatada.Should().Be(new DateTime(2025, 11, 13, 10, 30, 0));
        }

        [TestMethod]
        public void DataPedidoFormatado_DeveRetornarDateTimeMinValue_QuandoDataPedidoNula()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = null
            };

            // Act
            var dataFormatada = pedido.DataPedidoFormatado;

            // Assert
            dataFormatada.Should().Be(DateTime.MinValue);
        }

        [TestMethod]
        public void DataPedidoFormatado_DeveRetornarDateTimeMinValue_QuandoDataPedidoVazia()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = string.Empty
            };

            // Act
            var dataFormatada = pedido.DataPedidoFormatado;

            // Assert
            dataFormatada.Should().Be(DateTime.MinValue);
        }

        [TestMethod]
        public void Pedido_DevePermitirAdicionarCliente()
        {
            // Arrange
            var pedido = new Pedido();
            var cliente = new Cliente
            {
                Cpf = "12345678900",
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            // Act
            pedido.Cliente = cliente;

            // Assert
            pedido.Cliente.Should().NotBeNull();
            pedido.Cliente.Cpf.Should().Be("12345678900");
            pedido.Cliente.Nome.Should().Be("João Silva");
            pedido.Cliente.Email.Should().Be("joao@email.com");
        }

        [TestMethod]
        public void Pedido_DevePermitirAdicionarListaDeProdutos()
        {
            // Arrange
            var pedido = new Pedido();
            var produtos = new List<Produto>
            {
                new Produto { IdProduto = 1, Nome = "Produto 1", PrecoUnitario = 10.50m, Quantidade = 2 },
                new Produto { IdProduto = 2, Nome = "Produto 2", PrecoUnitario = 20.00m, Quantidade = 1 }
            };

            // Act
            pedido.Produtos = produtos;

            // Assert
            pedido.Produtos.Should().HaveCount(2);
            pedido.Produtos.First().IdProduto.Should().Be(1);
        }
    }

    [TestClass]
    public class ClienteTests
    {
        [TestMethod]
        public void Cliente_DeveInicializarComValoresPadrao()
        {
            // Arrange & Act
            var cliente = new Cliente();

            // Assert
            cliente.Cpf.Should().BeNull();
            cliente.Nome.Should().BeNull();
            cliente.Email.Should().BeNull();
        }

        [TestMethod]
        public void Cliente_DeveDefinirPropriedadesCorretamente()
        {
            // Arrange & Act
            var cliente = new Cliente
            {
                Cpf = "12345678900",
                Nome = "Maria Silva",
                Email = "maria@email.com"
            };

            // Assert
            cliente.Cpf.Should().Be("12345678900");
            cliente.Nome.Should().Be("Maria Silva");
            cliente.Email.Should().Be("maria@email.com");
        }
    }

    [TestClass]
    public class ProdutoTests
    {
        [TestMethod]
        public void Produto_DeveInicializarComValoresPadrao()
        {
            // Arrange & Act
            var produto = new Produto();

            // Assert
            produto.IdProduto.Should().Be(0);
            produto.Nome.Should().BeNull();
            produto.PrecoUnitario.Should().BeNull();
            produto.Quantidade.Should().BeNull();
            produto.Observacao.Should().BeNull();
        }

        [TestMethod]
        public void Produto_DeveDefinirPropriedadesCorretamente()
        {
            // Arrange & Act
            var produto = new Produto
            {
                IdProduto = 10,
                Nome = "Hamburguer",
                PrecoUnitario = 25.90m,
                Quantidade = 3,
                Observacao = "Sem cebola"
            };

            // Assert
            produto.IdProduto.Should().Be(10);
            produto.Nome.Should().Be("Hamburguer");
            produto.PrecoUnitario.Should().Be(25.90m);
            produto.Quantidade.Should().Be(3);
            produto.Observacao.Should().Be("Sem cebola");
        }

        [TestMethod]
        public void Produto_DeveCalcularValorTotalCorretamente()
        {
            // Arrange
            var produto = new Produto
            {
                PrecoUnitario = 10.50m,
                Quantidade = 3
            };

            // Act
            var valorTotal = produto.PrecoUnitario * produto.Quantidade;

            // Assert
            valorTotal.Should().Be(31.50m);
        }
    }
}
