using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FpsDisplayer : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float deltaTime;
    public Color lowFpsColor = new Color(255, 0, 0);
    public Color highFpsColor = new Color(0, 255, 0);

    private void Start()
    {
        fpsText.overrideColorTags = true;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.color = Color.Lerp(lowFpsColor, highFpsColor, Mathf.Clamp(fps, 60f, 100f) / 100f);
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}
