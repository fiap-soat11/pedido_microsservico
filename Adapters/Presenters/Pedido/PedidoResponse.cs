using Domain;

namespace Adapters.Presenters.Pedido
{
    public class PedidoResponse
    {
        public int IdPedido { get; set; }

        public string? Cpf { get; set; }

        public string StatusAtual { get; set; }

        public decimal? ValorTotal { get; set; }

        public DateTime? DataPedido { get; set; }

        //public virtual Cliente? CpfNavigation { get; set; } = null;

       
    }
}
