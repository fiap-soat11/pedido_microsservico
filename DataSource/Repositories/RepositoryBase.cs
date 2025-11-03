using DataSource.Context;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq.Expressions;

namespace DataSource.Repositories
{
    public class RepositoryBase<TEntity, TKey> : IDisposable, IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DynamoDbContext _context;
        protected readonly IDynamoDBContext _dynamoDbContext;

        public RepositoryBase(DynamoDbContext context)
        {
            _context = context;
            _dynamoDbContext = context.Context;
        }

        public virtual IEnumerable<TEntity> ListarTodos()
        {
            var conditions = new List<ScanCondition>();
            return _dynamoDbContext.ScanAsync<TEntity>(conditions).GetRemainingAsync().Result;
        }

        public virtual TEntity BuscarPorId(TKey id)
        {
            return _dynamoDbContext.LoadAsync<TEntity>(id).Result;
        }

        public virtual void Inserir(TEntity entity)
        {
            _dynamoDbContext.SaveAsync(entity).Wait();
        }

        public virtual IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var allItems = ListarTodos();
            return allItems.AsQueryable().Where(predicate);
        }

        public virtual void Atualizar(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            _dynamoDbContext.SaveAsync(entity).Wait();
        }

        public virtual void Excluir(TKey id)
        {
            var entity = BuscarPorId(id);
            if (entity == null)
                throw new Exception("Entidade não existe");
            
            _dynamoDbContext.DeleteAsync<TEntity>(id).Wait();
        }

        public virtual void Excluir(TEntity entity)
        {
            _dynamoDbContext.DeleteAsync(entity).Wait();
        }

        public virtual void Excluir(Expression<Func<TEntity, bool>> where)
        {
            var objects = Buscar(where);
            foreach (TEntity obj in objects)
            {
                _dynamoDbContext.DeleteAsync(obj).Wait();
            }
        }

        public virtual bool Existe(Expression<Func<TEntity, bool>> predicate)
        {
            return Buscar(predicate).Any();
        }

        public void Dispose()
        {
            _dynamoDbContext?.Dispose();
        }
    }
}
