using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam{
        get {
            return FindFirstObjectByType<Camera>().transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
        transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, -20, 20), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
