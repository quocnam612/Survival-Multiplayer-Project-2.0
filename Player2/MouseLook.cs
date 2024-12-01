using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssests.Character.FirstPerson;

public class MouseLook : MonoBehaviour
{
    public float mouseXSensitivity = 130f;
    public float mouseYSensitivity = 100f;
    public Transform playerBody;

    [HideInInspector] public float xRotation = 0f;
    [HideInInspector] public bool unlockMouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        unlockMouse = false;
    }

    void Update()
    {
        if (!unlockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
