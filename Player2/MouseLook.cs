using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssests.Character.FirstPerson;

public class MouseLook : MonoBehaviour 
{
    private Vector2 rotation = Vector2.zero;
    private Transform cachedTransform;
    
    void Awake()
    {
        cachedTransform = transform;
        // Cache other frequently accessed components
    }

    void Update()
    {
        if (unlockMouse) return;
        
        rotation.x += Input.GetAxisRaw("Mouse X") * mouseXSensitivity * Time.deltaTime;
        rotation.y = Mathf.Clamp(
            rotation.y - Input.GetAxisRaw("Mouse Y") * mouseYSensitivity * Time.deltaTime,
            -90f, 90f
        );
        
        cachedTransform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
    }
}
