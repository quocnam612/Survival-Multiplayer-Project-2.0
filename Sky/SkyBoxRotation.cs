using UnityEngine;

public class SkyBoxRotation : MonoBehaviour {
    public float rotateSpeed = 0.3f;    

    void Update() {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
