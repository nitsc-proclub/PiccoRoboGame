using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviourPunCallbacks
{
    private bool disconnectByScript = false;

    private void OnGUI()
    {
        GUILayout.Window(0, new(10, 10, 200, 1), _ =>
        {
            GUILayout.Label(PhotonNetwork.IsConnected ? "Connected" : "Disconnected");
            GUILayout.Label("Ping: " + PhotonNetwork.GetPing() + ", " + "Loss: " + PhotonNetwork.PacketLossByCrcCheck);
            GUILayout.Label("Room: " + (PhotonNetwork.InRoom
                ? PhotonNetwork.CurrentRoom.Name
                : "---"));
            if (GUILayout.Button("ForceReload"))
            {
                ReloadScene();
            }
        }, "PhotonState");

        if (PhotonNetwork.InRoom)
        {
            var room = PhotonNetwork.CurrentRoom;
            GUILayout.Window(1, new(10, 170, 320, 1), _ =>
            {
                foreach (var (id, player) in room.Players)
                {
                    var dispName = string.IsNullOrEmpty(player.NickName) ? player.UserId : player.NickName;
                    var line = $"[{id}] {dispName}";
                    if (player.IsMasterClient)
                    {
                        line += "(M)";
                    }
                    if (player.IsLocal)
                    {
                        line += "(L)";
                    }

                    GUILayout.Label(line);
                }
            }, $"Players ({room.PlayerCount}/{room.MaxPlayers})");
        }
    }

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions(), TypedLobby.Default);
            return;
        }
        LoadGameManager();
    }

    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions{ PublishUserId = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        LoadGameManager();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (disconnectByScript)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            disconnectByScript = false;
        }
    }

    void ReloadScene()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            disconnectByScript = true;
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadGameManager()
    {
        foreach (var gameObj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (gameObj.name == "GameManager")
            {
                gameObj.SetActive(true);
            }
        }
    }
}
