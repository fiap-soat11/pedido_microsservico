using Application.Interfaces;
using Domain;
using Application.Configurations;

namespace Application.UseCases
{
    public class PedidoUseCase : IPedidoUseCase
    {        
        public async Task<Pedido> IniciarPedido(string cpf)
        {
            // CPF opcional: caso nulo ou vazio, gateway tratará como pedido de convidado
            return new Pedido();
        }



        public async Task<Pedido> AdicionarProduto(int idPedido, int idProduto, int quantidade, string? observacao)
        {
            //PedidoProduto produto = new PedidoProduto() { IdPedido = idPedido, IdProduto = idProduto, Observacao = observacao, Quantidade = quantidade };
            //
            //var pedido = new Pedido() { IdPedido = idPedido };
            //pedido.PedidoProdutos.Add(produto);
            //
            //return pedido;
            return null;
        }

        public async Task<Pedido> AtualizarProduto(int idPedido, int idPedidoProduto, int novaQuantidade, string? observacao)
        {
            return new Pedido();
        }

        public async Task<Pedido> RemoverProduto(int idPedido, int idPedidoProduto)
        {
            return new Pedido();
        }

        public async Task<IEnumerable<Pedido>> ListarPedidos()
        {
            return new List<Pedido>();
        }

        public async Task<Pedido> AtualizarStatusPedido(int idPedido, int novoStatusId)
        {
            

            return new Pedido();
        }
    }
}
