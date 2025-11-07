using Adapters.Gateways.Interfaces;
using Application.Configurations;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adapters.Gateways
{
    public class PedidoGateway : IPedidoGateway
    {
        private readonly IDataSource _dataSource;

        public PedidoGateway(IDataSource dataSource)
            => _dataSource = dataSource;

        // Cria e retorna o pedido
        public Task<Pedido> IniciarPedido(Cliente cliente)
            => _dataSource.IniciarPedido(cliente);

        // Busca pelo ID
        public Task<Pedido> BuscarPedidoPorId(int idPedido)
            => _dataSource.BuscarPedidoPorId(idPedido);

        // Listagens
        public Task<IEnumerable<Pedido>> ListarPedidos()
            => _dataSource.ListarPedidos();

        public Task<IEnumerable<Pedido>> ListarPedidoClienteStatus()
            => _dataSource.ListarPedidoClienteStatus();

        public Task<IEnumerable<Pedido>> ListarPedidoCozinha()
            => _dataSource.ListarPedidoCozinha();

        // Cancela e finaliza pedidos
        public Task CancelarPedido(int idPedido)
            => _dataSource.CancelarPedido(idPedido);

        public Task FinalizarPedido(int idPedido)
            => _dataSource.FinalizarPedido(idPedido);

        // Atualiza status e retorna o pedido atualizado
        public Task AtualizarStatusPedido(int idStatusPedido, int idPedido)
            => _dataSource.AtualizarStatusPedido(idStatusPedido, idPedido);

        // CRUD de itens do pedido
        public Task<Pedido> AdicionarProduto(int idPedido, Produto produto)
            => _dataSource.AdicionarProduto(idPedido, produto);

        public Task<Pedido> AtualizarProduto(int idPedido, Produto produto)
            => _dataSource.AtualizarProduto(idPedido, produto);

        public Task<Pedido> RemoverProduto(int idPedido, int idProduto)
            => _dataSource.RemoverProduto(idPedido, idProduto);

        // Recalcula o valor total do pedido em memória
        public void RecalcularValorTotal(Pedido pedido)
            => _dataSource.RecalcularValorTotal(pedido);

        public void AtualizarPedido(Pedido pedido)
            => _dataSource.AtualizarPedido(pedido);
        //public Task<IList<PedidoProduto>> CarregarTodosProdutosPedido(int idPedido)
        //   => _dataSource.CarregarTodosProdutosPedido(idPedido);
    }
}
