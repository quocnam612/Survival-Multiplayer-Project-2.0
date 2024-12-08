using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
{
    private static Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();

    public static void RegisterPlayer(int actorNumber, GameObject playerObject)
    {
        if (!playerObjects.ContainsKey(actorNumber))
        {
            playerObjects[actorNumber] = playerObject;
            Debug.Log($"Registered player {actorNumber} with object {playerObject.name}");
        }
    }

    public static GameObject GetPlayerObject(int actorNumber)
    {
        playerObjects.TryGetValue(actorNumber, out GameObject playerObject);
        return playerObject;
    }

    public static Dictionary<int, GameObject> GetAllRegisteredPlayers()
    {
        return playerObjects;
    }
}
