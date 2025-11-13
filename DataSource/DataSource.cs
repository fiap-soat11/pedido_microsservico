using Application.Configurations;
using DataSource.Repositories.Interfaces;
using Domain;
using Domain.Enums;
using System.Linq;

namespace DataSource
{
    public class DataSource : Adapters.Gateways.Interfaces.IDataSource
    {
        private readonly IPedidoRepository _pedidoRepository;
        
        public DataSource(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
            
        }

        #region Pedido DataSource

        public async Task<Pedido> IniciarPedido(Cliente cliente)
        {
            
            var pedido = new Pedido
            {
                Cliente = cliente,
                StatusAtual = (int)StatusPedidoEnum.Iniciado,
                ValorTotal = 0,
                DataPedido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };

            _pedidoRepository.Inserir(pedido);

            return pedido;
            
        }

        public async Task AtualizarStatusPedido(int novoStatusId, int idPedido)
        {
            var pedido = _pedidoRepository.BuscarPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            
            if (pedido.StatusAtual == (int)StatusPedidoEnum.Finalizado || pedido.StatusAtual == (int)StatusPedidoEnum.Cancelado)
            {
                throw new BusinessException("Não é possível Atualizar um pedido que já está finalizado ou cancelado.");
            }

            pedido.StatusAtual = (int)(StatusPedidoEnum)Enum.Parse(typeof(StatusPedidoEnum), novoStatusId.ToString());
            AtualizarPedido(pedido);           
        }
        public async Task<Pedido> BuscarPedidoPorId(int idPedido)
        {
            var pedido = _pedidoRepository.BuscarPorId(idPedido);

            return pedido;
        }
        public async Task CancelarPedido(int idPedido)
        {
            var pedido = await BuscarPedidoPorId(idPedido)
                         ?? throw new BusinessException("Pedido não encontrado.");

            if (pedido.StatusAtual == (int)StatusPedidoEnum.Finalizado || pedido.StatusAtual == (int)StatusPedidoEnum.Cancelado)
            {
                throw new BusinessException("Não é possível Cancelar um pedido que já está finalizado ou cancelado.");
            }

            pedido.StatusAtual = (int)StatusPedidoEnum.Cancelado;
            

            AtualizarPedido(pedido);
        }
        public async Task FinalizarPedido(int idPedido)
        {
            var pedido = await BuscarPedidoPorId(idPedido)
                         ?? throw new BusinessException("Pedido não encontrado.");

            if (pedido.StatusAtual == (int)StatusPedidoEnum.Finalizado || pedido.StatusAtual == (int)StatusPedidoEnum.Cancelado)
            {
                throw new BusinessException("Não é possível finalizar um pedido que já está finalizado ou cancelado.");
            }

            pedido.StatusAtual = (int)StatusPedidoEnum.Finalizado;

            AtualizarPedido(pedido);
        }

        public async Task<IEnumerable<Pedido>> ListarPedidoClienteStatus()
        {
            var filtrados = _pedidoRepository
                .ListarTodos()
                .Where(p => p.StatusAtual != (int)StatusPedidoEnum.Finalizado && p.StatusAtual != (int)StatusPedidoEnum.Cancelado);

            var ordenados = filtrados
                .OrderBy(p =>
                    p.StatusAtual == (int)StatusPedidoEnum.Entregue ? "0" :   // Pronto
                    p.StatusAtual == (int)StatusPedidoEnum.Recebido ? "1" :   // Em Preparação
                    p.StatusAtual == (int)StatusPedidoEnum.EmPreparacao ? "2" :   // Recebido
                    "3"                             // Qualquer outro (Aguardando, Cancelado…)
                )
                .ThenBy(p => p.DataPedidoFormatado)
                .ToList();

            return await Task.FromResult(ordenados);
        }
        
        public async Task<IEnumerable<Pedido>> ListarPedidoCozinha() 
        {
            return _pedidoRepository.Buscar(x => x.StatusAtual.Equals("2")).ToList();
        }

        public async Task<IEnumerable<Pedido>> ListarPedidos()
        {
            var filtrados = _pedidoRepository
                .ListarTodos()
                .Where(p => p.StatusAtual != (int)StatusPedidoEnum.Finalizado && p.StatusAtual != (int)StatusPedidoEnum.Cancelado);

            var ordenados = filtrados
                .OrderBy(p =>
                    p.StatusAtual == (int)StatusPedidoEnum.Entregue ? "0" :   // Pronto
                    p.StatusAtual == (int)StatusPedidoEnum.Recebido ? "1" :   // Em Preparação
                    p.StatusAtual == (int)StatusPedidoEnum.EmPreparacao ? "2" :   // Recebido
                    "3"                             // Qualquer outro (Aguardando, Cancelado…)    
                )
                .ThenBy(p => p.DataPedidoFormatado)
                .ToList();

            return await Task.FromResult(ordenados);
        }      

        public async Task<Pedido> AdicionarProduto(int idPedido, Produto produto) 
        {
            var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            var itemExistente = pedido.Produtos.FirstOrDefault(pp => pp.IdProduto == produto.IdProduto);

            if (itemExistente == null)
            {
                pedido.Produtos.Add(produto);
            }
            else
            {
                itemExistente.Quantidade += produto.Quantidade;
                itemExistente.Observacao = produto.Observacao;
            }
            
            AtualizarPedido(pedido);

            RecalcularValorTotal(pedido);
            return pedido;
        }

        public async Task<Pedido> AtualizarProduto(int idPedido, Produto produto) 
        {
            var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
           
            var itemPedido = pedido.Produtos.FirstOrDefault(pp => pp.IdProduto == produto.IdProduto);
            if (itemPedido == null)
                throw new BusinessException("Item do pedido não encontrado.");

            itemPedido.Quantidade = produto.Quantidade;
            itemPedido.Observacao = produto.Observacao;

            AtualizarPedido(pedido);

            RecalcularValorTotal(pedido);
            AtualizarPedido(pedido);

            return pedido;
        }
        public async Task<Pedido> RemoverProduto(int idPedido, int idProduto)
        {
            var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            var itemPedido = pedido.Produtos.FirstOrDefault(pp => pp.IdProduto == idProduto);
            if (itemPedido == null)
                throw new BusinessException("Item do pedido não encontrado.");

            pedido.Produtos.Remove(itemPedido);

            RecalcularValorTotal(pedido);
            AtualizarPedido(pedido);

            return pedido;
        }
        public void AtualizarPedido(Pedido pedido)
        {
            _pedidoRepository.Atualizar(pedido);
        }
        public void RecalcularValorTotal(Pedido pedido)
        {
            if (pedido == null)
                throw new BusinessException("Pedido não informado para recalcular valor.");

            // Carrega todos os itens do pedido (deve vir populado via navegação ou carregado antes)
            var itens = pedido.Produtos;
            if (itens == null || !itens.Any())
            {
                pedido.ValorTotal = 0m;
            }
            else
            {
                var idsProdutos = itens
                    .Select(pp => pp.IdProduto)
                    .Distinct()
                    .ToList();
                            
                decimal total = 0m;
                foreach (var item in itens)
                    total += (item.Quantidade.GetValueOrDefault() * item.PrecoUnitario.GetValueOrDefault());
                
                pedido.ValorTotal = total;
            }

            AtualizarPedido(pedido);
        }

        #endregion


    }
}