using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;

    [Space] public Transform spawnPoint;

    [Space] public GameObject roomCam;

    [Space] public GameObject nameUI;

    [Space] public GameObject connectingUI;

    private string nickname = "unnamed";

    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickName(string _name) {
        nickname = _name;
    }

    public void JoinRoomButtonPressed() {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("We're in the lobby");

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We're connected in a room");

        roomCam.SetActive(false);
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer(nickname);
    }

    private void LateUpdate()
    {
        foreach (var kvp in PlayerManager.GetAllRegisteredPlayers())
        {
            Debug.Log($"ActorNumber: {kvp.Key}, Object: {kvp.Value}");
        }
    }
}
