using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text.Json.Nodes;

namespace MQTTPublisher
{
    class Publisher
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

            client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connected to the broker successfully !");
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from the server successfully !");
            });

            await client.ConnectAsync(options);

            Console.WriteLine("Please press a key to publish the message");
            Console.ReadLine();

            await PublishMessageAsync(client);

            await client.DisconnectAsync();
        }

        private static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayLoad = "Hello";
            string messagePayLoad2 = "World !";

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("FirstTopic")
                .WithPayload(messagePayLoad)
                .WithAtLeastOnceQoS()
                .Build();

            var message2 = new MqttApplicationMessageBuilder()
                .WithTopic("SecondTopic")
                .WithPayload(messagePayLoad2)
                .WithAtLeastOnceQoS()
                .Build();

            if (client.IsConnected)
            {
                await client.PublishAsync(message);
                await client.PublishAsync(message2);
            }
        }
    }
}