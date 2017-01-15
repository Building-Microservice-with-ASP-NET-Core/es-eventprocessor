
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StatlerWaldorfCorp.EventProcessor.Events;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using RabbitMQ.Client;
using System.Text;

namespace StatlerWaldorfCorp.EventProcessor.Queues.AMQP
{
    public class AMQPEventEmitter : AMQPClientBase, IEventEmitter
    {
        private ILogger<AMQPEventEmitter> logger;
       
        
        public AMQPEventEmitter(ILogger<AMQPEventEmitter> logger,
            IOptions<CloudFoundryServicesOptions> cfOptions,
            IOptions<QueueOptions> queueOptions) : base(cfOptions, queueOptions)
        {         
            this.logger = logger;                        

            logger.LogInformation($"Emitting events on queue {this.queueOptions.ProximityDetectedEventQueueName}");
            logger.LogInformation($"AMQP Connection configured for URI : {rabbitServiceBinding.Credentials["uri"].Value}");
        }

        public void EmitProximityDetectedEvent(ProximityDetectedEvent proximityDetectedEvent)
        {
             using (IConnection conn = connectionFactory.CreateConnection()) {
                using (IModel channel = conn.CreateModel()) {
                    channel.QueueDeclare(
                        queue: queueOptions.ProximityDetectedEventQueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    string jsonPayload = proximityDetectedEvent.toJson();
                    var body = Encoding.UTF8.GetBytes(jsonPayload);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueOptions.ProximityDetectedEventQueueName,
                        basicProperties: null,
                        body: body
                    );
                    logger.LogInformation($"Emitted proximity event of {jsonPayload.Length} bytes to queue.");
                }
            }
        }      
    }
}