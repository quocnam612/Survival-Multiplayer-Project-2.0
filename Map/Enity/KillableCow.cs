using Photon.Pun;
using UnityEngine;

public class KillableCow : MonoBehaviourPunCallbacks
{
    public HealthSystem healthSystem;
    public string prefabPath;
    public bool cowDied = false;

    private InventorySystem inventorySystem;
    private AI_Movement aiMovement;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        aiMovement = GetComponent<AI_Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit range") && inventorySystem.handHold.transform.childCount != 0)
        {
            float damage = inventorySystem.handHold.transform.GetChild(0).gameObject.name.Contains("Pickaxe") ? 15f : 3f;
            healthSystem.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBufferedViaServer, damage);

            aiMovement.animator.SetTrigger("Hurt");

            if (healthSystem.health <= 0 && !cowDied)
            {
                cowDied = true;
                PhotonNetwork.Instantiate(prefabPath, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
