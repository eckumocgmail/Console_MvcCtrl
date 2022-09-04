using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

public class ConnectionsContext: StartupAsyncContext
{
    private readonly bool _Logging = true;
    private readonly ILogger<ConnectionsContext> _logger;

    /// <summary>
    /// Операции по наименованиям
    /// </summary>
    private readonly ConcurrentDictionary<string, ConsumerController> Connections;

    public static List<AppManifest> Manifests = new List<AppManifest>();

    public ConnectionsContext( ILogger<ConnectionsContext> logger )
    {
        this._logger = logger;
        this.Connections = new ConcurrentDictionary<string, ConsumerController>();
    }


    /// <summary>
    /// Выполняется поиск служб приложений запущенных приложением RootLaunch
    /// </summary>
    public List<HttpClient> InitNetwork()
    {
        List<HttpClient> consumers = new List<HttpClient>();
        foreach (var manifest in Manifests)
        {            
            var cons = new HttpClient();
            consumers.Add(cons);
        }
        return consumers;
    }

    public ConsumerController[] GetClientConnections()
    {
        return this.Connections.Values.ToArray();
    }


    public void AddClientConnection( string ConnectionId,  Action<string> Request)
    {
        if (_Logging)
            _logger.LogInformation($"AddClientConnection({ConnectionId})");
        Connections[ConnectionId] = new ConsumerController(Request, this) { 
            Token = ConnectionId
        };        
    }


    public void RemoveClientConnection(string ConnectionId)
    {
        if (_Logging)
            _logger.LogInformation($"RemoveClientConnection({ConnectionId})");
        ConsumerController Consumer;
        Connections.TryRemove(ConnectionId, out Consumer);         
    }

    public ConsumerController GetClientConnection(string connectionId)
    {
        if( Connections.ContainsKey(connectionId))
        {
            return Connections[connectionId];
        }
        else
        {
            return null;
        }
    }
}