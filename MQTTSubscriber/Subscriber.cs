using MQTTnet.Client.Options;
using MQTTnet;
using System;
using MQTTnet.Client;
using System.Text;

namespace MQTTSubscriber
{
    class Subscriber
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithCleanSession()
                .Build();

            client.UseConnectedHandler( async e =>
            {
                Console.WriteLine("Connected to the broker successfully !");
                var topicFilter = new TopicFilterBuilder()
                .WithTopic("FirstTopic")
                .Build();

                await client.SubscribeAsync(topicFilter);
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from the server successfully !");
            });

            client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"Received message : {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} ");
                Console.WriteLine($"Received topic : {(e.ApplicationMessage.Topic)}");
            });

            await client.ConnectAsync(options);

            Console.ReadLine();

            await client.DisconnectAsync();
        }
    }
}