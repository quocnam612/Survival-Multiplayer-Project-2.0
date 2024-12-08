using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AnnoucementDisplayer : MonoBehaviourPunCallbacks
{
    [HideInInspector] public GameObject content {
        get {
            return transform.GetChild(0).gameObject;
        }
    }

    private GameObject nextAction;

    private IEnumerator Wait(int time) {
        gameObject.GetComponent<PhotonView>().RPC("updateActionGobal", RpcTarget.Others, "<i> A player has joined the game!</i>");
        gameObject.transform.GetChild(0).transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(time);
        gameObject.transform.GetChild(0).transform.localScale = Vector3.one;
    }

    private void Start()
    {
        StartCoroutine(Wait(5));
    }

    private void Update() {
        if (content.transform.childCount >= 5) {
            Destroy(content.transform.GetChild(0).gameObject);
        }
    }

    public void updateAction(string action)
    {
        nextAction = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Update Action"), content.transform.position, content.transform.rotation, content.transform);
        action.Replace("\n", "");
        nextAction.name = action;
        nextAction.GetComponent<Text>().text = action;
    }

    [PunRPC]
    public void updateActionGobal(string action)
    {
        nextAction = (GameObject)Instantiate(Resources.Load<GameObject>("UI/Update Action"), content.transform.position, content.transform.rotation, content.transform);
        action.Replace("\n", "");
        nextAction.name = action;
        nextAction.GetComponent<Text>().text = action;
    }
}
