using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

public class Program
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected...");
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
        {
            while (true)
            {
                string data = reader.ReadLine();
                if (data == null) break;

                MyObject objReceived = JsonSerializer.Deserialize<MyObject>(data);
                Console.WriteLine($"Received: {objReceived}");

                // Modify the object (e.g., increment a field)
                objReceived.Value++;

                string response = JsonSerializer.Serialize(objReceived);
                writer.WriteLine(response);
                writer.Flush();
                Console.WriteLine($"Sent: {objReceived}");
            }
        }

        client.Close();
        Console.WriteLine("Client disconnected...");
    }
}

public class MyObject
{
    public string Name { get; set; }
    public int Value { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Value: {Value}";
    }
}
