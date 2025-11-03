using Domain;
using DataSource.Context;
using DataSource.Repositories.Interfaces;

namespace DataSource.Repositories
{
    public class PedidoRepository : RepositoryBase<Pedido, int>, IPedidoRepository
    {
        public PedidoRepository(DynamoDbContext context) : base(context)
        {

        }
    }
}
