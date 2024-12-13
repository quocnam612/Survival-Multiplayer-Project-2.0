using UnityEngine;

public class SearchZone : MonoBehaviour
{
    public bool foundTarget;
    public GameObject target;

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerSetup>()) {
            target = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target) {
            target = null;
        }
    }
}
