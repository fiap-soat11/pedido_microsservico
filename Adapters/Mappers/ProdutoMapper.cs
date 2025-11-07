using Adapters.Presenters.Pedido;
using Domain.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters.Mappers
{
    public class ProdutoMapper
    {
        public static Produto ToEntity(AdicionarProdutoPedidoRequest produto)
        {
            return new Produto
            {
                IdProduto = produto.IdProduto,

                Quantidade = produto.Quantidade,
                Observacao = produto.Observacao

            };
        }

        public static Produto ToEntity(AtualizarProdutoPedidoRequest produto)
        {
            return new Produto
            {
                Quantidade = produto.NovaQuantidade,
                Observacao = produto.Observacao
            };
        }
    }
}
