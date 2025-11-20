using Domain.Enums;
using FluentAssertions;

namespace Domain.Test.Enums
{
    [TestClass]
    public class StatusPedidoEnumTests
    {
        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorIniciado()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.Iniciado;

            // Assert
            ((int)status).Should().Be(1);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorRecebido()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.Recebido;

            // Assert
            ((int)status).Should().Be(2);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorEmPreparacao()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.EmPreparacao;

            // Assert
            ((int)status).Should().Be(3);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorEntregue()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.Entregue;

            // Assert
            ((int)status).Should().Be(4);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorFinalizado()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.Finalizado;

            // Assert
            ((int)status).Should().Be(5);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveTerValorCancelado()
        {
            // Arrange & Act
            var status = StatusPedidoEnum.Cancelado;

            // Assert
            ((int)status).Should().Be(6);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveConverterDeInteiroParaEnum()
        {
            // Arrange
            int statusValue = 3;

            // Act
            var status = (StatusPedidoEnum)statusValue;

            // Assert
            status.Should().Be(StatusPedidoEnum.EmPreparacao);
        }

        [TestMethod]
        public void StatusPedidoEnum_DeveConverterDeEnumParaString()
        {
            // Arrange
            var status = StatusPedidoEnum.Recebido;

            // Act
            var statusString = status.ToString();

            // Assert
            statusString.Should().Be("Recebido");
        }
    }
}
