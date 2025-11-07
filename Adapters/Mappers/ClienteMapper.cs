using Adapters.Presenters.Pedido;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters.Mappers
{
    public class ClienteMapper
    {
        public static Cliente ToEntity(ClienteRequest clienteRequest)
        {
            return new Cliente
            {
                Cpf = clienteRequest.Cpf,
                Nome = clienteRequest.Nome,
                Email = clienteRequest.Email
            };
        }
    }
}
