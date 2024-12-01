using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingRotation : MonoBehaviour
{
    [Header("Loading Icon")]
    public Image loadingIcon;
    [Range(0f, 360f)] public float rotationSpeed;
    private float zRotation;

    [Header("Loading Text")]
    public Text loadingTextUI;
    [Range(0f, 5f)] public float textSpeed;
    public string[] loadingText;
    private float count;

    void Start()
    {
        loadingIcon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        count = 0;
    }

    void Update()
    {
        zRotation += Time.deltaTime * rotationSpeed;
        loadingIcon.transform.localRotation = Quaternion.Euler(0, 0, - zRotation);

        count += textSpeed * Time.deltaTime;
        if (Mathf.FloorToInt(count) > loadingText.Length - 1) {
            count = 0;
        }
        loadingTextUI.text = loadingText[Mathf.FloorToInt(count)];
    }
}
