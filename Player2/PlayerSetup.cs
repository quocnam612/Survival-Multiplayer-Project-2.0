using UnityEngine;
using UnityStandardAssests.Character.FirstPerson;

public class PlayerSetup : MonoBehaviour
{
    public PlayerMovement2 movement;
    public GameObject playerCamera;
    public Canvas playerUI;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject characterPreview;

    public void IsLocalPlayer() {
        playerCamera.SetActive(true);
        characterPreview.SetActive(true);   
        skinnedMeshRenderer.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerUI.enabled = true;
        movement.enabled = true;
    }
}
    