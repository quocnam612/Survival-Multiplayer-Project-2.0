using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringBlood : MonoBehaviour
{
    [SerializeField] private float amplitude, normal, speed = 3f, time = 0f;
    private Image blood {
        get {
            return GetComponent<Image>();
        }
    }

    private void Update()
    {
        blood.color = new Color(blood.color.r, blood.color.r, blood.color.b, normal / 255f + Mathf.Sin(time * speed) * amplitude);
        if (time * Mathf.Rad2Deg < 360) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
        }
    }
}
