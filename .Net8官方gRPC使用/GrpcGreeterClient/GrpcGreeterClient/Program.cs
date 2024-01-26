using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeterClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7232");
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("请输入消息:");
while (true)
{
    var msg = Console.ReadLine();
    if (msg == "1")
    {
        break;
    }
    var reply = await client.SayHelloAsync(new HelloRequest { Name = msg });
    Console.WriteLine(reply.Message);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();