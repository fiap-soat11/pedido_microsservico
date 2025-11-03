using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace DataSource.Helpers
{
    public class DynamoDbHelper
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;

        public DynamoDbHelper(IAmazonDynamoDB client, IDynamoDBContext context)
        {
            _client = client;
            _context = context;
        }

        public async Task<bool> TableExistsAsync(string tableName)
        {
            try
            {
                var response = await _client.DescribeTableAsync(tableName);
                return response.Table.TableStatus == TableStatus.ACTIVE;
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }

        public async Task CreatePedidosTableIfNotExistsAsync()
        {
            var tableName = "Pedidos";
            
            if (await TableExistsAsync(tableName))
            {
                return;
            }

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "IdPedido",
                        AttributeType = ScalarAttributeType.N
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "IdPedido",
                        KeyType = KeyType.HASH
                    }
                },
                BillingMode = BillingMode.PAY_PER_REQUEST // On-demand pricing
            };

            await _client.CreateTableAsync(request);

            // Aguardar até a tabela estar ativa
            await WaitUntilTableActiveAsync(tableName);
        }

        
        private async Task WaitUntilTableActiveAsync(string tableName, int maxAttempts = 10)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    var response = await _client.DescribeTableAsync(tableName);
                    if (response.Table.TableStatus == TableStatus.ACTIVE)
                    {
                        return;
                    }
                }
                catch (ResourceNotFoundException)
                {
                    
                }

                await Task.Delay(1000); 
            }

            throw new TimeoutException($"Tabela {tableName} não ficou ativa após {maxAttempts} tentativas");
        }

        
        public async Task<List<T>> QueryByCpfAsync<T>(string cpf, string indexName = "CpfIndex") where T : class
        {
            #pragma warning disable CS0618
            var search = _context.QueryAsync<T>(
                cpf,
                new DynamoDBOperationConfig
                {
                    IndexName = indexName
                });
            #pragma warning restore CS0618

            return await search.GetRemainingAsync();
        }

        
        public async Task BatchWriteAsync<T>(List<T> items) where T : class
        {
            if (items == null || !items.Any())
                return;

            var batches = items.Chunk(25);

            foreach (var batch in batches)
            {
                var batchWrite = _context.CreateBatchWrite<T>();
                foreach (var item in batch)
                {
                    batchWrite.AddPutItem(item);
                }
                await batchWrite.ExecuteAsync();
            }
        }

       public async Task<List<T>> BatchGetAsync<T>(List<object> ids) where T : class
        {
            if (ids == null || !ids.Any())
                return new List<T>();

            var batchGet = _context.CreateBatchGet<T>();
            foreach (var id in ids)
            {
                batchGet.AddKey(id);
            }

            await batchGet.ExecuteAsync();
            return batchGet.Results;
        }

        public async Task TransactWriteAsync(List<TransactWriteItem> transactItems)
        {
            var request = new TransactWriteItemsRequest
            {
                TransactItems = transactItems
            };

            await _client.TransactWriteItemsAsync(request);
        }

        
        
        public async Task<int> CountItemsAsync(string tableName)
        {
            var request = new ScanRequest
            {
                TableName = tableName,
                Select = Select.COUNT
            };

            var response = await _client.ScanAsync(request);
            return response.Count ?? 0;
        }

        
        public async Task TruncateTableAsync<T>() where T : class
        {
            var items = await _context.ScanAsync<T>(new List<ScanCondition>()).GetRemainingAsync();
            
            foreach (var item in items)
            {
                await _context.DeleteAsync(item);
            }
        }
    }
}
