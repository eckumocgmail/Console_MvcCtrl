using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

/// <summary>
/// 
/// </summary>
public class NetworkMonitor          
{

    private static Random r = new Random();

    /// <summary>
    /// Случайнай номер свободного порта
    /// </summary>
    public static int GetFreeTcpPort() => GetFreeTcpPort(49152, 65535);
    public static int GetFreeTcpPort(int min = 13000, int max = 18000)
    {
        var monitor = new NetworkMonitor();
        int port = r.Next(min, max);
        if (monitor.IsFreePort(port) == false)
        {
            do
            {
                port = r.Next(min, max);
            }
            while (monitor.IsFreePort(port) == false);
        }            
        return port;
    }

    [JsonIgnore]
    public TcpConnectionInformation[] tcpConnections { get; private set; }

    [JsonIgnore]
    public IPGlobalProperties properties { get; private set; }

    [JsonIgnore]
    public IPEndPoint[] tcpEndPoints { get; private set; }
    public PortInfo[] tcpPorts { get; private set; }


    /// <summary>
    /// 
    /// </summary>
    public class PortInfo
    {
        public int PortNumber { get; private set; }
        public string Local { get; private set; }
        public string Remote { get; private set; }
        public string State { get; private set; }

        public PortInfo(int i, string local, string remote, string state)
        {
            PortNumber = i;
            Local = local;
            Remote = remote;
            State = state;
        }
    }

    /// <summary>
    /// Сканирование портов 
    /// </summary>
    public void Init()
    {
        this.properties = IPGlobalProperties.GetIPGlobalProperties();
        this.tcpEndPoints = properties.GetActiveTcpListeners();
        this.tcpConnections = properties.GetActiveTcpConnections();
        this.tcpPorts = tcpConnections.Select(p =>
        {
            return new PortInfo(
                i: p.LocalEndPoint.Port,
                    
                local: String.Format("{0}:{1}", p.LocalEndPoint.Address, p.LocalEndPoint.Port),
                remote: String.Format("{0}:{1}", p.RemoteEndPoint.Address, p.RemoteEndPoint.Port),
                state: p.State.ToString());
        }).ToArray();
    }
        


    /// <summary>
    /// Выполняется проверка занят порт или нет
    /// </summary>
    public bool IsNotFree(int n) => this.tcpPorts.Where(p => (p.PortNumber == n)).Any();

    /// <summary>
    /// Выполняется проверка занят порт или нет
    /// </summary>
    private bool IsFreePort(int port)
    {
        Init();
        return tcpPorts.Where(p => p.Local == port.ToString() || p.Remote == port.ToString()).Any() == false;
    }

}
