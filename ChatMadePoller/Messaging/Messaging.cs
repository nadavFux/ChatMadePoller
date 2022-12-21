using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Messaging
{
    public interface IMessageSender
    {
        void SendMessage(string message);
    }

    public class RabbitMQSender : IMessageSender
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;
        private readonly ILogger logger;

        public RabbitMQSender(string hostName, string exchangeName, ILogger logger)
        {
            var factory = new ConnectionFactory { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _exchangeName = exchangeName;

            this.logger = logger;

            // Declare the exchange
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
        }

        public void SendMessage(string message)
        {
            // Make sure the connection and channel are open
            if (!_connection.IsOpen || !_channel.IsOpen)
            {
                throw new ConnectFailureException("RabbitMQ connection or channel is not open.", new Exception("RabbitMQ connection or channel is not open."));
            }

            // Use the connection and channel objects to send the message to the RabbitMQ exchange
            byte[] body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: null, body: body);
        }
    }
}
