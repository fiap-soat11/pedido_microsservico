using Amazon.DynamoDBv2.DataModel;

namespace Domain;

[DynamoDBTable("Pedidos")]
public partial class Pedido
{
    [DynamoDBHashKey("IdPedido")]
    public int IdPedido { get; set; }

    [DynamoDBProperty("Cpf")]
    public string? Cpf { get; set; }

    [DynamoDBProperty("IdStatusAtual")]
    public int? IdStatusAtual { get; set; }

    [DynamoDBProperty("ValorTotal")]
    public decimal? ValorTotal { get; set; }

    [DynamoDBProperty("DataPedido")]
    public DateOnly? DataPedido { get; set; }

}
