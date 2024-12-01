using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PingDisplayer : MonoBehaviour
{
    public TextMeshProUGUI pingText;
    public Color lowPingColor = new Color(255, 0, 0);
    public Color highPingColor = new Color(0, 255, 0);

    private void Start()
    {
        pingText.overrideColorTags = true;
    }

    void Update()
    {
        float ping = Mathf.Ceil(PhotonNetwork.GetPing());
        pingText.color = Color.Lerp(highPingColor, lowPingColor, Mathf.Clamp(ping, 55f, 100f) / 100f);
        pingText.text = "Ping: " + ping.ToString();
    }
}
