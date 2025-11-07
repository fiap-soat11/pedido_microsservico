using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum StatusPedidoEnum
    {
        Iniciado = 1,
        Recebido = 2,
        EmPreparacao = 3,
        Entregue = 4,
        Finalizado = 5,
        Cancelado = 6
    }
}
