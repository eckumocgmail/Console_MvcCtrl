

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ValidationAnnotationsNS;

public class NetworkUnit: TestingElement, IDisposable
{



    public Action<byte> OnRead { get; set; }
    public Action<Exception> OnError { get; set; }
    public Func<byte[], byte[]> ToWrite { get; set; }


    private TcpClient client;
    private TcpListener listener;
   
    //private ReadOnlySpan<char> host;

    static void Maasdasdin(string[] args)
    {
      //  Start1();
      //  Start2();
        Thread.Sleep(Timeout.Infinite);
    }


    public static Task Start1()
    {
        using (var server = new NetworkUnit())
        {
            server.Start(
                (oneByte) =>
                {
                    Console.WriteLine(oneByte);
                    throw new Exception();
                },
                (throwableMessage) =>
                {
                    Console.WriteLine("ToWrite");
                },
                (readed) =>
                {
                    Console.WriteLine("ToWrite");
                    return null;
                }
            );
        }
        return Task.CompletedTask;
    }


    public static   Task Start2()
    {
        using (var server = new NetworkUnit())
        {
            server.port = 2013;
            server.Start(
                (oneByte) =>
                {
                    Console.WriteLine(oneByte);
                    throw new Exception();
                },
                (throwableMessage) => { },
                (readed) =>
                {
                    return null;
                }
            );
        }
        return Task.CompletedTask;
    }




    public event EventHandler<SocketAddressEventArgs> OnTcpListenerStarted = (sender, evt) => { };
    public class SocketAddressEventArgs: EventArgs
    {
        public SocketOptions Options { get; private set; }
        public SocketAddressEventArgs(SocketOptions Options)
        {
            this.Options = Options;
        }
    }


    public void Start(Action<byte> onRead,
                    Action<Exception> onError,
                    Func<byte[], byte[]> toWrite)
    {
        this.listener = new TcpListener(null, this.port);
    //    this.listener = new TcpListener(IPAddress.Parse(this.host), this.port);
        this.listener.Start();
      
    //    this.OnTcpListenerStarted.Invoke(this, new SocketAddressEventArgs(this));
        this.client = listener.AcceptTcpClient();

        using (var network = this.client.GetStream())
        {
            /*Console.WriteLine("operations: ");
            foreach (var operation in network.GetOperations())
            {
                Console.WriteLine(operation.Key + " " + operation.Value);
            }*/

            var readed = new List<byte>();
            while(network.DataAvailable)
            {
                byte next = (byte)network.ReadByte();
                readed.Add(next);
                onRead(next);
            }
            byte[] data = toWrite(readed.ToArray());
            foreach (var next in data)
            {
                network.WriteByte(next);
            }

        }


       

    }

    public override void Dispose()
    {
        base.Dispose();
        if (this.listener != null) 
            listener.Stop();
    }

    protected override void OnTest()
    {
        throw new NotImplementedException();
    }
}
