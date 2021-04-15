using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
class Program
{
    static bool Work;
    private static UdpClient client = new UdpClient();

    static int clientport;

    static async Task Send(string msg)
    {
        // Console.WriteLine("Sending...");
        byte[] Datagram = Encoding.UTF8.GetBytes(msg);
        IPEndPoint ip_broad = new IPEndPoint(IPAddress.Broadcast, clientport);
        await client.SendAsync(Datagram, Datagram.Length, ip_broad);
    }
    static async Task Receive()
    {
        while (Work)
        {
            var ReceiveMsg = await client.ReceiveAsync();
            Console.WriteLine(Encoding.UTF8.GetString(ReceiveMsg.Buffer));
        }
    }
    static async Task Main(string[] args)
    {
        string name;
        string UserMesage;
        string msg;
        clientport = 2365;
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, clientport);
        client.Client.Bind(ip);
        Work = true;


        Console.WriteLine("Hello. Еnter your name (it will be visible to other participants):");
        name = Console.ReadLine();
        Console.WriteLine("You have become a member of the chat. You can send and receive messages");
        Task Rec = Receive();
        while (Work)
        {
            UserMesage = Console.ReadLine();
            if (UserMesage == "exit")
            {
                Console.WriteLine("The chat will be closed, goodbye!");
                Work = false;
                Environment.Exit(0);
            }
            else
            {
                msg = "From " + name + " at " + DateTime.Now.ToString("HH:mm:ss dd MMMM yyyy") + ":\n" + UserMesage;
                await Send(msg);
            }
        }
        await Rec;

    }
}


