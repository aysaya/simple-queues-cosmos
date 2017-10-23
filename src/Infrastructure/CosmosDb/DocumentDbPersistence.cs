using System.Collections.Generic;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.CosmosDb
{
    public interface IProvideDocumentRepository<T>
    {
        Task SaveAsync(T rate);

        Task<T[]> GetAllAsync();
    }

    public class DocumentDbRepository<T> : IProvideDocumentRepository<T>
    {
        private readonly IProvideCosmosDbConnection<T> cosmosDbConnection;
        public DocumentDbRepository(IProvideCosmosDbConnection<T> cosmosDbConnection)
        {
            this.cosmosDbConnection = cosmosDbConnection;
        }

        public async Task SaveAsync(T rate)
        {
            await cosmosDbConnection.DocumentClient.CreateDocumentAsync
                (
                    cosmosDbConnection.DocumentCollectionUri,
                    rate
                );
        }

        public async Task<T[]> GetAllAsync()
        {
            var query = cosmosDbConnection.DocumentClient
                .CreateDocumentQuery<T>
                (cosmosDbConnection.DocumentCollectionUri)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results.ToArray();
        }
    }
}
