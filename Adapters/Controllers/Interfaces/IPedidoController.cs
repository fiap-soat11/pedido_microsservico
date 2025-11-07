using Domain;
using Adapters.Presenters.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters.Controllers.Interfaces
{
    public interface IPedidoController
    {
        Task<IEnumerable<Pedido>> ListarPedidos();
        Task<IEnumerable<Pedido>> ListarPedidoClienteStatus();
        Task<IEnumerable<Pedido>> ListarPedidoCozinha();
        Task<bool> AtualizarStatusPedido(int idStatusPedido, int idPedido);
        Task<bool> FinalizarPedido(int pedido);
        Task<Pedido> BuscarPedidoPorId(int idPedido);
        Task<bool> CancelarPedido(int pedido);        
        Task<Pedido> IniciarPedido(ClienteRequest? cliente);
        Task<Pedido> AdicionarProduto(int idPedido, AdicionarProdutoPedidoRequest produto);
        Task<Pedido> AtualizarProduto(int idPedido, AtualizarProdutoPedidoRequest produto);
        Task<Pedido> RemoverProduto(int idPedido, int idProduto);
        //List<PedidoProduto> ListarProdutosDoPedido(int idPedido);
    }
}
