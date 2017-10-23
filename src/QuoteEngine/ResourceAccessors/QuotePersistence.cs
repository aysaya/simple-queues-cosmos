using Infrastructure.CosmosDb;
using QuoteEngine.DomainModels;
using System.Threading.Tasks;

namespace QuoteEngine.ResourceAccessors
{
    public interface ICommandRA<T>
    {
        Task SaveAsync(T quote);
    }
    public interface IQueryRA<T>
    {
        Task<T[]> GetAllAsync();
    }

    public class QuotePersistence : ICommandRA<Quote>, IQueryRA<Quote>
    {
        private readonly IProvideDocumentRepository<Quote> quoteRepository;

        public QuotePersistence(IProvideDocumentRepository<Quote> quoteRepository)
        {
            this.quoteRepository = quoteRepository;
        }

        public async Task SaveAsync(Quote quote)
        {
            await quoteRepository.SaveAsync(quote);
        }

        public async Task<Quote[]> GetAllAsync()
        {
            return await quoteRepository.GetAllAsync();                
        }
    }
}
