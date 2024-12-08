using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>(); 

    IEnumerator Start()
    {
        // Precaution
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if (cachedRoomList.Count <= 0) {
            cachedRoomList = roomList;
        }
        else {
            foreach (var room in roomList) {
                for (int i = 0; i < cachedRoomList.Count; i++) {
                    if (cachedRoomList[i].Name == room.Name) {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList) {
                            newList.Remove(newList[i]);
                        }
                        else {
                            newList[i] = room; 
                        }

                        cachedRoomList = newList;
                    }
                }
            }
        }

        UpdateUI();
    }

    void UpdateUI() {
        foreach (Transform roomItem in roomListParent) {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList) {
            GameObject rootItem = Instantiate(roomListItemPrefab, roomListParent);

            rootItem.transform.GetChild(0).GetComponent<Text>().text = room.Name;
            rootItem.transform.GetChild(1).GetComponent<Text>().text = room.PlayerCount + "/16";
        }
    }
}
