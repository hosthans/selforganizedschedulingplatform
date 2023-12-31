using ServerApplication.modules;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;
/// <summary>
/// Socket, der die Verbindungen der verschiedenen Benutzer überwacht/steuert.
/// </summary>
public class ClientWebsocketBehavior : WebSocketBehavior
{
    protected override void OnOpen()
    {
    }
    
    protected override void OnMessage(MessageEventArgs e)
    {
        var client = SocketMessageHelper.DeserializeFromByteArray<ClientObject>(e.RawData);
        client.MinClientsToStartGames = SocketServerService.objectConfiguration.MinNumberOfConnectedClients;
        if (!UpdateClient(client))
        {
            SocketServerService.Clients.Add(Context, client);
            Logging.LogInformation($"New Client connected Uid: {client.Label} ({client.UniqueId})");
        }
        var clientsObjectByteArray = SocketMessageHelper.SerializeToByteArray(GetClientsList());
        SocketServerService.WebSocketServer.WebSocketServices["/clients"].Sessions.Broadcast(clientsObjectByteArray);
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Logging.LogInformation($"Client disconnected - New Client Count: {SocketServerService.Clients.Count}");
        SocketServerService.Clients.Remove(Context);
        byte[] clientsObjectByteArray = SocketMessageHelper.SerializeToByteArray(GetClientsList());
        SocketServerService.WebSocketServer.WebSocketServices["/clients"].Sessions.Broadcast(clientsObjectByteArray);
    }

    private List<ClientObject> GetClientsList()
    {
        List<ClientObject> clientsList = new List<ClientObject>();
        foreach (var keyValuePair in SocketServerService.Clients)
        {
            clientsList.Add(keyValuePair.Value);
        }

        return clientsList;
    }

    private bool UpdateClient(ClientObject updatedClient)
    {
        foreach (var keyValuePair in SocketServerService.Clients)
        {
            if (keyValuePair.Value.UniqueId == updatedClient.UniqueId)
            {
                SocketServerService.Clients[keyValuePair.Key] = updatedClient;
                return true;
            }
        }

        return false;
    }
}