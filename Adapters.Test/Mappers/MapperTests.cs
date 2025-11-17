using Adapters.Mappers;
using Adapters.Presenters.Pedido;
using Domain;
using Domain.Enums;
using FluentAssertions;

namespace Adapters.Test.Mappers
{
    [TestClass]
    public class ClienteMapperTests
    {
        [TestMethod]
        public void ToEntity_DeveConverterClienteRequestParaCliente()
        {
            // Arrange
            var clienteRequest = new ClienteRequest
            {
                Cpf = "12345678900",
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            // Act
            var cliente = ClienteMapper.ToEntity(clienteRequest);

            // Assert
            cliente.Should().NotBeNull();
            cliente.Cpf.Should().Be("12345678900");
            cliente.Nome.Should().Be("João Silva");
            cliente.Email.Should().Be("joao@email.com");
        }

        [TestMethod]
        public void ToEntity_DeveConverterClienteRequestComValoresNulos()
        {
            // Arrange
            var clienteRequest = new ClienteRequest
            {
                Cpf = null,
                Nome = null,
                Email = null
            };

            // Act
            var cliente = ClienteMapper.ToEntity(clienteRequest);

            // Assert
            cliente.Should().NotBeNull();
            cliente.Cpf.Should().BeNull();
            cliente.Nome.Should().BeNull();
            cliente.Email.Should().BeNull();
        }
    }

    [TestClass]
    public class ProdutoMapperTests
    {
        [TestMethod]
        public void ToEntity_DeveConverterAdicionarProdutoPedidoRequestParaProduto()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 10,
                Quantidade = 3,
                Observacao = "Sem cebola"
            };

            // Act
            var produto = ProdutoMapper.ToEntity(request);

            // Assert
            produto.Should().NotBeNull();
            produto.IdProduto.Should().Be(10);
            produto.Quantidade.Should().Be(3);
            produto.Observacao.Should().Be("Sem cebola");
        }

        [TestMethod]
        public void ToEntity_DeveConverterAtualizarProdutoPedidoRequestParaProduto()
        {
            // Arrange
            var request = new AtualizarProdutoPedidoRequest
            {
                NovaQuantidade = 5,
                Observacao = "Com queijo extra"
            };

            // Act
            var produto = ProdutoMapper.ToEntity(request);

            // Assert
            produto.Should().NotBeNull();
            produto.Quantidade.Should().Be(5);
            produto.Observacao.Should().Be("Com queijo extra");
        }

        [TestMethod]
        public void ToEntity_DeveConverterAdicionarProdutoSemObservacao()
        {
            // Arrange
            var request = new AdicionarProdutoPedidoRequest
            {
                IdProduto = 20,
                Quantidade = 1,
                Observacao = null
            };

            // Act
            var produto = ProdutoMapper.ToEntity(request);

            // Assert
            produto.Should().NotBeNull();
            produto.IdProduto.Should().Be(20);
            produto.Quantidade.Should().Be(1);
            produto.Observacao.Should().BeNull();
        }
    }

    [TestClass]
    public class PedidoMapperTests
    {
        [TestMethod]
        public void PedidoClienteToDTO_DeveConverterPedidoParaPedidoClienteResponse()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                StatusAtual = (int)StatusPedidoEnum.Recebido
            };

            // Act
            var response = PedidoMapper.PedidoClienteToDTO(pedido);

            // Assert
            response.Should().NotBeNull();
            response.IdPedido.Should().Be(1);
            response.StatusAtual.Should().Be("Recebido");
        }

        [TestMethod]
        public void PedidoCozinhaToDTO_DeveConverterPedidoParaPedidoCozinhaResponse()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 2,
                StatusAtual = (int)StatusPedidoEnum.EmPreparacao
            };

            // Act
            var response = PedidoMapper.PedidoCozinhaToDTO(pedido);

            // Assert
            response.Should().NotBeNull();
            response.IdPedido.Should().Be(2);
            response.StatusAtual.Should().Be("EmPreparacao");
        }

        [TestMethod]
        public void ToResponse_DeveConverterPedidoParaPedidoResponse()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 3,
                DataPedido = "2025-11-13 10:30:00",
                StatusAtual = (int)StatusPedidoEnum.Entregue,
                ValorTotal = 150.50m,
                Cliente = new Cliente
                {
                    Cpf = "12345678900",
                    Nome = "João Silva"
                }
            };

            // Act
            var response = PedidoMapper.ToResponse(pedido);

            // Assert
            response.Should().NotBeNull();
            response.IdPedido.Should().Be(3);
            response.Cpf.Should().Be("12345678900");
            response.StatusAtual.Should().Be("Entregue");
            response.ValorTotal.Should().Be(150.50m);
            response.DataPedido.Should().Be(new DateTime(2025, 11, 13, 10, 30, 0));
        }

        [TestMethod]
        public void ToResponse_DeveConverterPedidoComValorTotalNulo()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 4,
                DataPedido = "2025-11-13 10:30:00",
                StatusAtual = (int)StatusPedidoEnum.Iniciado,
                ValorTotal = null,
                Cliente = new Cliente
                {
                    Cpf = "12345678900"
                }
            };

            // Act
            var response = PedidoMapper.ToResponse(pedido);

            // Assert
            response.Should().NotBeNull();
            response.ValorTotal.Should().Be(0m);
        }

        [TestMethod]
        public void PedidoClienteToDTO_DeveConverterTodosStatusCorretamente()
        {
            // Arrange & Act & Assert
            var statusList = new[]
            {
                (StatusPedidoEnum.Iniciado, "Iniciado"),
                (StatusPedidoEnum.Recebido, "Recebido"),
                (StatusPedidoEnum.EmPreparacao, "EmPreparacao"),
                (StatusPedidoEnum.Entregue, "Entregue"),
                (StatusPedidoEnum.Finalizado, "Finalizado"),
                (StatusPedidoEnum.Cancelado, "Cancelado")
            };

            foreach (var (status, expectedString) in statusList)
            {
                var pedido = new Pedido
                {
                    PedidoId = 1,
                    StatusAtual = (int)status
                };

                var response = PedidoMapper.PedidoClienteToDTO(pedido);

                response.StatusAtual.Should().Be(expectedString);
            }
        }
    }
}
