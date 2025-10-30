namespace Domain;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public string? Cpf { get; set; }

    public int? IdStatusAtual { get; set; }

    public decimal? ValorTotal { get; set; }

    public DateOnly? DataPedido { get; set; }

    //public virtual Cliente? CpfNavigation { get; set; }

    //public virtual Status? IdStatusAtualNavigation { get; set; }

    //public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    //public virtual ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();

    //public virtual ICollection<Preparo> Preparos { get; set; } = new List<Preparo>();
    public string? QRCode { get; set; }

}

/*
 
 {
   "pedido_id":{
      "S":"PED12345"
   },
   "data_pedido":{
      "S":"2025-10-23T18:30:00Z"
   },
   "valor_total":{
      "N":"45.50"
   },
   "cliente":{
      "M":{
         "cpf":{
            "S":"11122233344"
         },
         "nome":{
            "S":"Ana Souza"
         },
         "email":{
            "S":"ana.souza@email.com"
         }
      }
   },
   "produtos":{
      "L":[
         {
            "M":{
               "id_produto":{
                  "S":"PROD001"
               },
               "nome":{
                  "S":"Hambúrguer Duplo"
               },
               "preco_unitario":{
                  "N":"25.00"
               },
               "quantidade":{
                  "N":"1"
               },
               "observacao":{
                  "S":"Sem picles"
               }
            }
         },
         {
            "M":{
               "id_produto":{
                  "S":"PROD009"
               },
               "nome":{
                  "S":"Batata Frita Média"
               },
               "preco_unitario":{
                  "N":"10.25"
               },
               "quantidade":{
                  "N":"2"
               }
            }
         }
      ]
   },
   "status_atual":{
      "S":"Finalizado"
   },
   
}

 */