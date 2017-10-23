using Contracts;
using Infrastructure.ServiceBus;
using QuoteEngine.DomainModels;
using QuoteEngine.ResourceAccessors;
using System;
using System.Threading.Tasks;

namespace QuoteEngine.MessageHandlers
{
    public class ThirdPartyRateProcessor : IProcessMessage<ThirdPartyRate>
    {
        private readonly ICommandRA<Quote> commandRA;
        private readonly IPublishMessage<NewQuoteReceived> messagePublisher;

        public ThirdPartyRateProcessor(ICommandRA<Quote> commandRA, IPublishMessage<NewQuoteReceived> messagePublisher)
        {
            this.commandRA = commandRA;
            this.messagePublisher = messagePublisher;
        }

        public async Task ProcessAsync(ThirdPartyRate message)
        {
            var quote = AssembleQuote(message);

            var saveTask = commandRA.SaveAsync(quote);

            var pubTask = messagePublisher.SendAsync(PrepareEventMessage(quote));

            await Task.WhenAll(saveTask, pubTask);
        }


        private Quote AssembleQuote(ThirdPartyRate payload)
        {
            return new Quote
            {
                Id = Guid.NewGuid(),
                BaseCurrency = payload.BaseCurrency,
                TargetCurrency = payload.TradeCurrency,
                Rate = payload.Rate
            };
        }

        private NewQuoteReceived PrepareEventMessage(Quote quote)
        {
            return new NewQuoteReceived
            {
                Id = quote.Id,
                BaseCurrency = quote.BaseCurrency,
                TradeCurrency = quote.TargetCurrency,
                Rate = quote.Rate
            };
        }
    }
}
