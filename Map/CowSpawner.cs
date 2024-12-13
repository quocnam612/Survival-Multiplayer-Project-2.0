using Photon.Pun;
using System.Collections;
using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    public float waitTime;
    public float countDown;
    public int maxCow;
    public string entityPath;

    private void Update()
    {
        if (transform.childCount < maxCow && PhotonNetwork.IsMasterClient && countDown <= 0) {
            countDown = waitTime;
            GameObject cow = PhotonNetwork.Instantiate(entityPath, transform.position, transform.rotation);
            cow.transform.SetParent(transform, true);

        }
        else if (transform.childCount < maxCow && PhotonNetwork.IsMasterClient) {
            countDown -= Time.deltaTime;
        }
    }
}
