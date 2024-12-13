using Photon.Pun;
using UnityEngine;
using UnityStandardAssests.Character.FirstPerson;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public PlayerMovement2 movement;
    public GameObject playerCamera;
    public Canvas playerUI;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject characterPreview;
    public Transform handBone;
    public string nickname;

    public void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = nickname;

        if (!FindFirstObjectByType<LightingManager>()) {
            PhotonNetwork.Instantiate("Sun", new Vector3(-2.8f, 42.85f, -0.67f), new Quaternion(172.5f, -10f, 180f, 0f));
        }

        if (photonView.IsMine)
        {
            PlayerManager.RegisterPlayer(PhotonNetwork.LocalPlayer.ActorNumber, gameObject);
        }
        else
        {
            PlayerManager.RegisterPlayer(photonView.Owner.ActorNumber, gameObject);
        }
    }

    public void IsLocalPlayer(string _name) {
        nickname = _name;
        handBone.gameObject.SetActive(false);
        playerCamera.SetActive(true);
        characterPreview.SetActive(true);
        skinnedMeshRenderer.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerUI.enabled = true;
        movement.enabled = true;
        gameObject.GetComponent<PhotonView>().RPC("RenameObject", RpcTarget.AllBuffered, nickname);
        gameObject.GetComponent<PhotonView>().RPC("SetItemName", RpcTarget.AllBuffered, nickname);
        gameObject.GetComponent<InteractableObject>().enabled = false;
    }

    [PunRPC]
    private void RenameObject(string _name)
    {
        gameObject.name = "Player - " + _name;
    }
}
    