using Photon.Pun;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviourPunCallbacks, IPunObservable
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField, Range(-10, 10)] private float speedMultiplier;  // used to adjust the cycle time. Note that values < 0 will reverse it!
    [SerializeField, Range(0, 10)] private float nightSpeed; // how much to speed up late-night hours
    [SerializeField] public float maxIntensity = 1.5f;
    private float baseIntensity = 0f;
    [SerializeField] private float maxShadowStrength = 0.9f;
    [SerializeField] private float minShadowStrength = 0.2f;
    private float nightSpeedUpStart = 20f;
    private float nightSpeedUpEnd = 4f;
    private float dawn = 6f;
    private float dusk = 18f;
    private float noon = 12f;

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        RenderSettings.fogDensity = Preset.FogColor.Evaluate(timePercent).a / 69f;
        RenderSettings.skybox.SetColor("_Tint", Preset.SkyColor.Evaluate(timePercent));

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void Start()
    {
        baseIntensity = maxIntensity / 2f;
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (TimeOfDay > nightSpeedUpStart || TimeOfDay < nightSpeedUpEnd)
                {
                    TimeOfDay += Time.deltaTime * nightSpeed;
                }
                else
                {
                    TimeOfDay += Time.deltaTime * speedMultiplier;

                    if (TimeOfDay >= dawn && TimeOfDay <= noon)
                    {
                        DirectionalLight.intensity = baseIntensity + (baseIntensity / (noon - dawn)) * (TimeOfDay - dawn);
                        DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (noon - dawn)) * (TimeOfDay - dawn);
                    }
                    else if (TimeOfDay > noon && TimeOfDay <= dusk)
                    {
                        DirectionalLight.intensity = baseIntensity + (baseIntensity / (dusk - noon)) * (dusk - TimeOfDay);
                        DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (dusk - noon)) * (dusk - TimeOfDay);
                    }
                    else
                    {
                        DirectionalLight.intensity = baseIntensity;
                        DirectionalLight.shadowStrength = minShadowStrength;
                    }
                }
                TimeOfDay %= 24;

                photonView.RPC("SyncTimeOfDay", RpcTarget.Others, TimeOfDay);
            }
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    [PunRPC]
    private void SyncTimeOfDay(float newTimeOfDay)
    {
        TimeOfDay = newTimeOfDay;
        UpdateLighting(TimeOfDay / 24f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(TimeOfDay);
        }
        else
        {
            TimeOfDay = (float)stream.ReceiveNext();
        }
    }
}
