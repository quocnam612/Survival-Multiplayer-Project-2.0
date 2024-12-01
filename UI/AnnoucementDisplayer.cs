using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AnnoucementDisplayer : MonoBehaviour
{
    [HideInInspector] public GameObject content {
        get {
            return transform.GetChild(0).gameObject;
        }
    }

    private GameObject nextAction;

    public void updateAction(string action) {
        nextAction = (GameObject)Instantiate(Resources.Load<GameObject>("Update Action"), content.transform.position, content.transform.rotation, content.transform);
        nextAction.name = action;
        nextAction.GetComponent<Text>().text = action;
    }

    private void Update() {
        if (content.transform.childCount >= 5) {
            Destroy(content.transform.GetChild(0).gameObject);
        }
    }
}
