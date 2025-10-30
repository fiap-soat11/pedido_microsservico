using Domain;
using System;
using System.Collections.Generic;


namespace Adapters.Presenters.Pedido
{
    public class PedidoProdutoResponse
    {
        public int IdPedidoProduto { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public string Observacao { get; set; }
    }
}