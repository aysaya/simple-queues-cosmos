using Microsoft.Azure.ServiceBus;

namespace Infrastructure.ServiceBus
{
    public interface IProvideServiceBusConnection<T>
    {
        QueueClient QueueClient();
        TopicClient TopicClient();
        SubscriptionClient SubscriptionClient();
    }

    public class ServiceBusConnectionProvider<T> : IProvideServiceBusConnection<T>
    {
        private readonly QueueClient queueClient;
        private readonly TopicClient topicClient;
        private readonly SubscriptionClient subscription;

        public ServiceBusConnectionProvider(string connectionString, string queueName)
        {
            queueClient = new QueueClient(connectionString, queueName);
        }
        public ServiceBusConnectionProvider(
            string connectionString,
            string topicName = null, string subscriptionName = null)
        {
            if (topicName != null)
                topicClient = new TopicClient(connectionString, topicName);

            if (subscriptionName != null && topicName != null)
                subscription = new SubscriptionClient(connectionString, topicName, subscriptionName);
        }

        public QueueClient QueueClient()
        {
            return queueClient;
        }

        public SubscriptionClient SubscriptionClient()
        {
            return subscription;
        }

        public TopicClient TopicClient()
        {
            return topicClient;
        }
    }

}
