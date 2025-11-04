using Application.Configurations;
using DataSource.Repositories.Interfaces;
using Domain;
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

        public async Task<Pedido> IniciarPedido(string cpf)
        {
            //Cliente cliente = null;

            //if (!string.IsNullOrWhiteSpace(cpf))
            //{
            //    cliente = await BuscarClientePorCPF(cpf);
            //    if (cliente == null)
            //        throw new BusinessException("Cliente não encontrado.");
            //}

            var pedido = new Pedido
            {
                Cpf = cpf,
                IdStatusAtual = 1,
                ValorTotal = 0
            };

            _pedidoRepository.Inserir(pedido);

            //return pedido;
            return null;
        }

        public async Task AtualizarStatusPedido(int novoStatusId, int idPedido)
        {
            var pedido = _pedidoRepository.BuscarPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            //var status = BuscarStatusPorId(novoStatusId) ?? throw new BusinessException("Status não encontrado.");

            if (pedido.IdStatusAtual == 5 || pedido.IdStatusAtual == 6)
            {
                throw new BusinessException("Não é possível Atualizar um pedido que já está finalizado ou cancelado.");
            }

            pedido.IdStatusAtual = novoStatusId;
            //pedido.IdStatusAtualNavigation = status.Result;
            AtualizarPedido(pedido);           
        }
        public async Task<Pedido> BuscarPedidoPorId(int idPedido)
        {
            var pedido = _pedidoRepository.BuscarPorId(idPedido);
            //pedido.PedidoProdutos = CarregarTodosProdutosPedido(idPedido).Result;

            return pedido;
        }
        public async Task CancelarPedido(int idPedido)
        {
            var pedido = await BuscarPedidoPorId(idPedido)
                         ?? throw new BusinessException("Pedido não encontrado.");

            if (pedido.IdStatusAtual == 5 || pedido.IdStatusAtual == 6)
            {
                throw new BusinessException("Não é possível Cancelar um pedido que já está finalizado ou cancelado.");
            }

            pedido.IdStatusAtual = 6;
            //pedido.IdStatusAtualNavigation = await BuscarStatusPorId(5);

            AtualizarPedido(pedido);
        }
        public async Task FinalizarPedido(int idPedido)
        {
            var pedido = await BuscarPedidoPorId(idPedido)
                         ?? throw new BusinessException("Pedido não encontrado.");

            if (pedido.IdStatusAtual == 5 || pedido.IdStatusAtual == 6)
            {
                throw new BusinessException("Não é possível finalizar um pedido que já está finalizado ou cancelado.");
            }

            pedido.IdStatusAtual = 5;
            //pedido.IdStatusAtualNavigation = await BuscarStatusPorId(5);

            AtualizarPedido(pedido);
        }

        public async Task<IEnumerable<Pedido>> ListarPedidoClienteStatus()
        {
            var filtrados = _pedidoRepository
                .ListarTodos()
                .Where(p => p.IdStatusAtual != 5 && p.IdStatusAtual != 6);

            var ordenados = filtrados
                .OrderBy(p =>
                    p.IdStatusAtual == 4 ? 0 :   // Pronto
                    p.IdStatusAtual == 3 ? 1 :   // Em Preparação
                    p.IdStatusAtual == 2 ? 2 :   // Recebido
                    3                             // Qualquer outro (Aguardando, Cancelado…)
                )
                .ThenBy(p => p.DataPedido ?? DateOnly.MinValue)
                .ToList();

            return await Task.FromResult(ordenados);
        }
        
        public async Task<IEnumerable<Pedido>> ListarPedidoCozinha() 
        {
            return _pedidoRepository.Buscar(x => x.IdStatusAtual.Equals(2)).ToList();
        }

        public async Task<IEnumerable<Pedido>> ListarPedidos()
        {
            var filtrados = _pedidoRepository
                .ListarTodos()
                .Where(p => p.IdStatusAtual != 5 && p.IdStatusAtual != 6);

            var ordenados = filtrados
                .OrderBy(p =>
                    p.IdStatusAtual == 4 ? 0 :   // Pronto
                    p.IdStatusAtual == 3 ? 1 :   // Em Preparação
                    p.IdStatusAtual == 2 ? 2 :   // Recebido
                    3                             // Qualquer outro (Aguardando, Cancelado…)
                )
                .ThenBy(p => p.DataPedido ?? DateOnly.MinValue)
                .ToList();

            //ordenados.ForEach(d=> d.PedidoProdutos = CarregarTodosProdutosPedido(d.IdPedido).Result);

            return await Task.FromResult(ordenados);
        }      

        public async Task<Pedido> AdicionarProduto(int idPedido, int idProduto, int quantidade, string? observacao) 
        {
            //var produto = _produtoRepository.BuscarPorId(idProduto) ?? throw new BusinessException("Produto não encontrado.");
            //if (quantidade <= 0)
            //    throw new BusinessException("Quantidade deve ser maior que zero.");

            //var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            //var itemExistente = pedido.PedidoProdutos.FirstOrDefault(pp => pp.IdProduto == idProduto);

            //if (itemExistente == null)
            //{
            //    var novoItem = new PedidoProduto
            //    {
            //        IdPedido = idPedido,
            //        IdProduto = idProduto,
            //        Quantidade = quantidade,
            //        Observacao = observacao
            //    };
            //    pedido.PedidoProdutos.Add(novoItem);
            //}
            //else
            //{
            //    itemExistente.Quantidade += quantidade;
            //    itemExistente.Observacao = observacao;
            //}
            Pedido pedido = null;
            AtualizarPedido(pedido);

            RecalcularValorTotal(pedido);
            return pedido;
        }

        public async Task<Pedido> AtualizarProduto(int idPedido, int idPedidoProduto, int novaQuantidade, string? observacao) 
        {
            var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            //var produto = _produtoRepository.BuscarPorId(idPedidoProduto) ?? throw new BusinessException("Produto não encontrado.");

            //var itemPedido = pedido.PedidoProdutos.FirstOrDefault(pp => pp.IdPedidoProduto == idPedidoProduto);
            //if (itemPedido == null)
            //    throw new BusinessException("Item do pedido não encontrado.");

            //itemPedido.Quantidade = novaQuantidade;
            //itemPedido.Observacao = observacao;

            AtualizarPedido(pedido);

            RecalcularValorTotal(pedido);
            AtualizarPedido(pedido);

            return pedido;
        }
        public async Task<Pedido> RemoverProduto(int idPedido, int idPedidoProduto)
        {
            var pedido = await BuscarPedidoPorId(idPedido) ?? throw new BusinessException("Pedido não encontrado.");
            //var itemPedido = pedido.PedidoProdutos.FirstOrDefault(pp => pp.IdPedidoProduto == idPedidoProduto);
            //if (itemPedido == null)
            //    throw new BusinessException("Item do pedido não encontrado.");

            //// Remove o item diretamente do contexto
            //_pedidoProdutoRepository.Excluir(itemPedido.IdPedidoProduto);

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
        //    if (pedido == null)
        //        throw new BusinessException("Pedido não informado para recalcular valor.");

        //    // Carrega todos os itens do pedido (deve vir populado via navegação ou carregado antes)
        //    var itens = pedido.PedidoProdutos;
        //    if (itens == null || !itens.Any())
        //    {
        //        pedido.ValorTotal = 0m;
        //    }
        //    else
        //    {
        //        var idsProdutos = itens
        //            .Where(pp => pp.IdProduto.HasValue)
        //            .Select(pp => pp.IdProduto.Value)
        //            .Distinct()
        //            .ToList();

        //        var produtosDoPedido = _produtoRepository
        //            .Buscar(p => idsProdutos.Contains(p.IdProduto))
        //            .ToDictionary(p => p.IdProduto, p => p.Preco);

        //        decimal total = 0m;
        //        foreach (var item in itens)
        //        {
        //            var produtoId = item.IdProduto
        //                            ?? throw new BusinessException("Item do pedido sem produto definido");
        //            if (!produtosDoPedido.TryGetValue(produtoId, out var preco))
        //                throw new BusinessException($"Produto {item.IdProduto} não encontrado.");

        //            total += (item.Quantidade.GetValueOrDefault() * preco);
        //        }

        //        pedido.ValorTotal = total;
        //    }

        //    AtualizarPedido(pedido);
        }

        #endregion


    }
}