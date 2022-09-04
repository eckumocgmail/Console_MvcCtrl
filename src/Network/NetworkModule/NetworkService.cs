using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


public class NetworkService
{

    public static void Main()
    {
        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (true)
            {
                Console.Write("127.0.0.1 Waiting for a connection... ");
                

                TcpClient client = server.AcceptTcpClient();
         
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();


                int i;
                int next = -2;
                // Loop to receive all the data sent by the client.
                while ((next = stream.ReadByte()) != -1)
                {
                    // Translate data bytes to a ASCII string.
                    Console.Write(System.Text.Encoding.ASCII.GetString(new byte[1] { (byte)next }, 0, 1));
                }
                // Shutdown and end connection
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }








    private Thread ReadingThread;
    private TcpListener TcpListener;

    public string IP { get; }
    public int Port { get; }
    public bool Active { get; private set; }
    public TcpClient Client { get; private set; }

    public NetworkService(string ip, int port)
    {
        this.IP = ip;
        this.Port = port;
        this.TcpListener = new TcpListener(IPAddress.Parse(ip), port);
            
    }

    public void Connect(NetworkService hub)
    {
        hub.OpenSocket();
        hub.Active = true;
        this.Client = new TcpClient(hub.IP, hub.Port);
        using (var stream = this.Client.GetStream())
        {
            Active = true;
            var thread = new Thread(() => {
                byte next;
                while (true)
                {
                    next = (byte)stream.ReadByte();
                    Enqueue(next);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }

    private void Enqueue(byte next)
    {
        throw new NotImplementedException();
    }

    public void OpenSocket()
    {
        var thread = new Thread(() => {
            TcpListener.Start();

            while (true)
            {
                TcpClient client = TcpListener.AcceptTcpClient();
                using (var networkStream = client.GetStream())
                {
                    StartReading(networkStream);
                }
            }
        });
        thread.IsBackground = true;
        thread.Start();
            
    }



    private void StartReading( NetworkStream input )
    {
        ReadingThread = new Thread(() => {
            while (true)
            {
                byte next = (byte)input.ReadByte();
                Enqueue(next);
            }
        });
        ReadingThread.IsBackground = true;
        ReadingThread.Start();
            
    }

     
}
