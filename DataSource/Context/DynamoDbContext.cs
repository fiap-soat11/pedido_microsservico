using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DataSource.Context
{
    public class DynamoDbContext
    {
        public IAmazonDynamoDB DynamoDbClient { get; }
        public IDynamoDBContext Context { get; }

        public DynamoDbContext(IAmazonDynamoDB dynamoDbClient)
        {
            DynamoDbClient = dynamoDbClient;
            
            
            Context = new DynamoDBContext(dynamoDbClient);
            
        }
    }
}
