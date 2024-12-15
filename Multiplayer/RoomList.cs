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
    private List<GameObject> roomListItems = new List<GameObject>();

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
                        if (room.RemovedFromList) {
                            cachedRoomList.RemoveAt(i);
                        } else {
                            cachedRoomList[i] = room; 
                        }
                        break;
                    }
                }
            }
        }

        UpdateUI();
    }

    void UpdateUI() {
        // Deactivate unused room list items
        foreach (GameObject roomItem in roomListItems) {
            roomItem.SetActive(false);
        }

        // Activate and update required room list items
        for (int i = 0; i < cachedRoomList.Count; i++) {
            GameObject roomItem;
            if (i < roomListItems.Count) {
                roomItem = roomListItems[i];
            } else {
                roomItem = Instantiate(roomListItemPrefab, roomListParent);
                roomListItems.Add(roomItem);
            }

            roomItem.transform.GetChild(0).GetComponent<Text>().text = cachedRoomList[i].Name;
            roomItem.transform.GetChild(1).GetComponent<Text>().text = cachedRoomList[i].PlayerCount + "/16";
            roomItem.SetActive(true);
        }
    }
}
