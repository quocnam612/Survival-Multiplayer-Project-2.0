using System.Collections;
using UnityEngine;
using UnityStandardAssests.Character.FirstPerson;

public class WeaponAnimationController : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.069f;
    public float swaySmoothness = 3.456f;

    [Header("Bob Settings")]
    public float bobSpeed = 6.9f;
    public float bobAmount = 0.069f;
    public float jumpBobAmount = 0.0169f;
    public float jumpSmoothness = 1.69f;

    [Header("Idle Settings")]
    public float idleSpeed = 1.69f;
    public float idleAmount = 0.0069f;

    [Header("Swing Settings")]
    [SerializeField] private Quaternion swingAngle;
    [SerializeField] private Vector3 swingPosition;
    public float swingSpeed = 10f;
    public float swingReturnSpeed = 5f;
    public float swingCooldown = 0.2f;

    public bool isSwinging = false;
    public bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private CharacterController characterController {
        get {
            return transform.root.GetComponent<CharacterController>();
        }
    }   

    void Start()
    {
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    public void SwingWeapon()
    {
        transform.root.GetComponent<PlayerMovement2>().animator.SetTrigger("Attack");
        isSwinging = true;
        isOnCooldown = true;
        cooldownTimer = swingCooldown;  
        StartCoroutine(PerformSwing());
    }

    public void HandleCooldown()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    private IEnumerator PerformSwing()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * swingSpeed;
            transform.localPosition = Vector3.Slerp(originalPosition, swingPosition, elapsedTime);
            transform.localRotation = Quaternion.Slerp(originalRotation, swingAngle, elapsedTime);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * swingReturnSpeed;
            transform.localPosition = Vector3.Lerp(swingPosition, originalPosition, elapsedTime);
            transform.localRotation = Quaternion.Lerp(swingAngle, originalRotation, elapsedTime);
            yield return null;
        }

        isSwinging = false;
    }

    public void HandleSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        Vector3 swayTarget = new Vector3(-mouseX * swayAmount, -mouseY * swayAmount, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + swayTarget, Time.deltaTime * swaySmoothness);
    }

    public void HandleBob()
    {
        bobSpeed = Mathf.Clamp(transform.root.GetComponent<PlayerMovement2>().moveSpeed * 1.7f, 5.5f, 11.5f);
        bobAmount = Mathf.Clamp(transform.root.GetComponent<PlayerMovement2>().moveSpeed / 234, 0.01f, 0.069f);

        if (!transform.root.GetComponent<PlayerMovement2>().grounded)
        {
            targetPosition = originalPosition + new Vector3(bobAmount / 10f, jumpBobAmount, -bobAmount / 10f);
        }
        else if (transform.root.GetComponent<PlayerMovement2>().isMoving)
        {
            float bobWaveX = Mathf.Cos(Time.time * bobSpeed) * bobAmount;
            float bobWaveY = Mathf.Abs(Mathf.Sin(Time.time * bobSpeed)) * bobAmount;
            float bobWaveZ = bobWaveX / 2f - bobWaveY / 3f;

            targetPosition = originalPosition + new Vector3(bobWaveX, bobWaveY, bobWaveZ);
        }
        else
        {
            float idleWave = Mathf.Sin(Time.time * idleSpeed) * idleAmount;
            targetPosition = originalPosition + new Vector3(0, idleWave, 0);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * (!transform.root.GetComponent<PlayerMovement2>().grounded ? jumpSmoothness : swaySmoothness));
    }
}
