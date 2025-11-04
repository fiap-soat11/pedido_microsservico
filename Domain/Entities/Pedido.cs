using Amazon.DynamoDBv2.DataModel;

namespace Domain;

[DynamoDBTable("Pedidos")]
public partial class Pedido
{
    [DynamoDBHashKey("pedido_id")]
    public string PedidoId { get; set; } = string.Empty;

    [DynamoDBProperty("data_pedido")]
    public string? DataPedido { get; set; }

    [DynamoDBProperty("valor_total")]
    public decimal? ValorTotal { get; set; }

    [DynamoDBProperty("cliente")]
    public Cliente? Cliente { get; set; }

    [DynamoDBProperty("produtos")]
    public List<Produto>? Produtos { get; set; }

    [DynamoDBProperty("status_atual")]
    public string? StatusAtual { get; set; }

    [DynamoDBProperty("historico_status")]
    public List<HistoricoStatus>? HistoricoStatus { get; set; }

    [DynamoDBProperty("pagamento")]
    public Pagamento? Pagamento { get; set; }
}

public class Cliente
{
    [DynamoDBProperty("cpf")]
    public string? Cpf { get; set; }

    [DynamoDBProperty("nome")]
    public string? Nome { get; set; }

    [DynamoDBProperty("email")]
    public string? Email { get; set; }
}

public class Produto
{
    [DynamoDBProperty("id_produto")]
    public string? IdProduto { get; set; }

    [DynamoDBProperty("nome")]
    public string? Nome { get; set; }

    [DynamoDBProperty("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [DynamoDBProperty("quantidade")]
    public int? Quantidade { get; set; }

    [DynamoDBProperty("observacao")]
    public string? Observacao { get; set; }
}

public class HistoricoStatus
{
    [DynamoDBProperty("status")]
    public string? Status { get; set; }

    [DynamoDBProperty("data")]
    public string? Data { get; set; }
}

public class Pagamento
{
    [DynamoDBProperty("id_pagamento")]
    public string? IdPagamento { get; set; }

    [DynamoDBProperty("forma_pagamento")]
    public string? FormaPagamento { get; set; }

    [DynamoDBProperty("status")]
    public string? Status { get; set; }

    [DynamoDBProperty("data_pagamento")]
    public string? DataPagamento { get; set; }
}
