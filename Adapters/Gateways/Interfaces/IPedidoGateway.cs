using Domain;

namespace Adapters.Gateways.Interfaces
{
    public interface IPedidoGateway
    {
        Task<Pedido> IniciarPedido(Cliente cliente);
        Task AtualizarStatusPedido(int idStatusPedido, int idPedido);
        Task<IEnumerable<Pedido>> ListarPedidos();        
        Task<Pedido> BuscarPedidoPorId(int idPedido);
        Task CancelarPedido(int idPedido);
        Task FinalizarPedido(int idPedido);
        Task<IEnumerable<Pedido>> ListarPedidoClienteStatus();
        Task<IEnumerable<Pedido>> ListarPedidoCozinha();        
        Task<Pedido> AdicionarProduto(int idPedido, Produto produto);
        Task<Pedido> AtualizarProduto(int idPedido, Produto produto);
        Task<Pedido> RemoverProduto(int idPedido, int idProduto);
        void AtualizarPedido(Pedido pedido);
        void RecalcularValorTotal(Pedido pedido);
        //Task<IList<PedidoProduto>> CarregarTodosProdutosPedido(int idPedido);

    }
}
