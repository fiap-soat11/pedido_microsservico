using Adapters.Presenters.Pedido;

using Domain;
using Domain.Enums;

namespace Adapters.Mappers
{
    public class PedidoMapper
    {
        public static PedidoClienteResponse PedidoClienteToDTO(Pedido pedido)
        {
            return new PedidoClienteResponse
            {
                IdPedido = pedido.PedidoId,
                StatusAtual = ((StatusPedidoEnum)pedido.StatusAtual.Value).ToString()
            };
        }

        /*public static PedidoResponse ToDTO(Pedido pedido)
        {
            return new PedidoResponse
            { 
                IdPedido = pedido.IdPedido,
                Cpf = pedido.Cpf,
                IdStatusAtual = pedido.IdStatusAtual,
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido,
                //CpfNavigation = pedido.CpfNavigation,
                //IdStatusAtualNavigation = pedido.IdStatusAtualNavigation,
                //Pagamentos = pedido.Pagamentos,
                //PedidoProdutos = pedido.PedidoProdutos,
                //Preparos = pedido.Preparos                
            
            };
        }*/
        /*public static PedidoResponse ToResponse(this Pedido pedido)
        {
            return new PedidoResponse
            {
                IdPedido = pedido.IdPedido,
                Cpf = pedido.Cpf,
                IdStatusAtual = pedido.IdStatusAtual,
                DataPedido = pedido.DataPedido ?? DateOnly.MinValue,
                ValorTotal = pedido.ValorTotal ?? 0,
                Produtos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                {
                    IdPedidoProduto = pp.IdPedidoProduto,
                    IdProduto = pp.IdProduto ?? 0,
                    Quantidade = pp.Quantidade ?? 0,
                    Observacao = pp.Observacao,
                    Produto = new ProdutoResponse
                    {
                        IdProduto = pp.IdProdutoNavigation.IdProduto,
                        Descricao = pp.IdProdutoNavigation.Descricao,
                        Preco = pp.IdProdutoNavigation.Preco
                    }
                }).ToList(),
            };
        }*/

        public static PedidoResponse ToResponse(Pedido pedido)
        {
            return new PedidoResponse
            {
                IdPedido = pedido.PedidoId,
                Cpf = pedido.Cliente.Cpf,
                DataPedido = pedido.DataPedidoFormatado,
                StatusAtual = ((StatusPedidoEnum)pedido.StatusAtual.Value).ToString(),
                ValorTotal = pedido.ValorTotal ?? 0m,
                
            };
        }

        public static PedidoCozinhaResponse PedidoCozinhaToDTO(Pedido pedido)
        {
            return new PedidoCozinhaResponse
            {
                IdPedido = pedido.PedidoId,
                StatusAtual = ((StatusPedidoEnum)pedido.StatusAtual.Value).ToString()

            };
        }

        /*public static PedidoProdutoResponse PedidoProdutoToDTO(Pedido p)
        {
            return new PedidoProdutoResponse
            {
                IdPedido = p.IdPedido,
                Cpf = p.Cpf,
                DataPedido = p.DataPedido,
                IdStatusAtual = p.IdStatusAtual,
                ValorTotal = p.ValorTotal,
                Produtos = p.PedidoProdutos.Select(pp => new PedidoProdutoResponse
                {
                    IdPedidoProduto = pp.IdPedidoProduto,
                    IdProduto = pp.IdProduto,
                    Quantidade = pp.Quantidade,
                    Observacao = pp.Observacao,
                    Produto = new ProdutoResponse
                    {
                        IdProduto = pp.Produto.IdProduto,
                        Descricao = pp.Produto.Descricao,
                        Preco = pp.Produto.Preco
                    }
                }).ToList()
            };
        }*/
    }
}
