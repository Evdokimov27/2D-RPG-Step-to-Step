//using UnityEngine;
//
//using System.Collections.Generic;
//using System.Linq;
//
//public class LobbyManager : NetworkManager
//{
//    public int minPlayers = 2; // ћинимальное количество игроков дл€ запуска игры
//    [SerializeField]
//    public List<NetworkConnectionToClient> pendingConnections = new List<NetworkConnectionToClient>();
//    public NetworkConnectionToClient pendingConnection;
//
//    public void AddPendingConnection(NetworkConnectionToClient conn)
//    { 
//        pendingConnections.Add(conn);
//        if (pendingConnections.Count >= minPlayers)
//        {
//            StartGame();
//        }
//    }
//    private void StartGame()
//    {
//        // ѕровер€ем, есть ли активный сервер (активен ли сервер)
//        var existingServer = NetworkServer.active;
//
//        if (existingServer)
//        {
//            // ѕрисоедин€ем игроков к существующему серверу
//            foreach (var conn in pendingConnections)
//            {
//                conn.Send(new StartGameMessage { isHost = false });
//                conn.Disconnect();
//            }
//        }
//        else
//        {
//            // —оздаем новый сервер
//            NetworkManager.singleton.StartHost();
//            NetworkManager.singleton.maxConnections = pendingConnections.Count;
//            pendingConnections.Clear();
//        }
//    }
//
//
//    public override void OnServerDisconnect(NetworkConnectionToClient conn)
//    {
//        base.OnServerDisconnect(conn);
//
//        // ”дал€ем игрока из списка ожидающих, если он отключаетс€
//        if (pendingConnections.Contains(conn))
//        {
//            pendingConnections.Remove(conn);
//        }
//    }
//
//
//    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
//    {
//        base.OnServerAddPlayer(conn);
//
//        pendingConnections.Add(conn);
//
//        if (pendingConnections.Count >= minPlayers)
//        {
//            StartGame();
//        }
//    }
//}
//public struct StartGameMessage : NetworkMessage
//{
//    public bool isHost;
//
//    public StartGameMessage(bool isHost)
//    {
//        this.isHost = isHost;
//    }
//}