//using UnityEngine;
//
//using System.Collections.Generic;
//using System.Linq;
//
//public class LobbyManager : NetworkManager
//{
//    public int minPlayers = 2; // ����������� ���������� ������� ��� ������� ����
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
//        // ���������, ���� �� �������� ������ (������� �� ������)
//        var existingServer = NetworkServer.active;
//
//        if (existingServer)
//        {
//            // ������������ ������� � ������������� �������
//            foreach (var conn in pendingConnections)
//            {
//                conn.Send(new StartGameMessage { isHost = false });
//                conn.Disconnect();
//            }
//        }
//        else
//        {
//            // ������� ����� ������
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
//        // ������� ������ �� ������ ���������, ���� �� �����������
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