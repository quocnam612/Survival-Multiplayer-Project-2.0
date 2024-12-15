using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks 
{
    private const string ROOM_NAME = "test";
    private readonly RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, roomOptions, TypedLobby.Default);
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomCam.SetActive(false);
        SpawnPlayer();
    }
    
    private void SpawnPlayer()
    {
        var playerInstance = PhotonNetwork.Instantiate(
            player.name,
            spawnPoint.position,
            Quaternion.identity
        );
        playerInstance.GetComponent<PlayerSetup>().IsLocalPlayer(nickname);
    }
}
