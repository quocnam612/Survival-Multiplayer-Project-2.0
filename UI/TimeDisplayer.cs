using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayer : MonoBehaviour
{
    public Text displayer {
        get {
            return GetComponent<Text>();
        }
    }
    public float time {
        get {
            return FindFirstObjectByType<LightingManager>().TimeOfDay;
        }
    }

    private void LateUpdate()
    {
        int minute = Mathf.FloorToInt((time - Mathf.Floor(time)) * 60);
        if (minute < 10) {
            displayer.text = Mathf.Floor(time) + ":0" + minute;
        }
        else
        {
            displayer.text = Mathf.Floor(time) + ":" + minute;
        }
    }
}
