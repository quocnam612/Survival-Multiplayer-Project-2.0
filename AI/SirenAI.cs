using UnityEngine;

public class SirenAI : MonoBehaviour
{

    public Animator animator;

    public float moveSpeed = 0.2f;
    private float startSpeed;
    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    public float rotationSPeed = 2f;
    private float rotation;
    private int startDamage;
    int WalkDirection;

    public bool isWalking;
    public GameObject searchZone;
    public GameObject damageZone;
    public LightingManager lightingManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startDamage = damageZone.GetComponent<DamageZoneDrain>().damagePerSec;
        //So that all the prefabs don't move/stop at the same time
        lightingManager = FindFirstObjectByType<LightingManager>();
        walkTime = Random.Range(25, 30);
        waitTime = Random.Range(10, 15);
        waitCounter = waitTime;
        walkCounter = walkTime;
        startSpeed = moveSpeed;
        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

        if (lightingManager.TimeOfDay > 20f || lightingManager.TimeOfDay < 5f)
        {
            damageZone.GetComponent<DamageZoneDrain>().damagePerSec = startDamage;
            searchZone.SetActive(true);
            if (searchZone.GetComponent<SearchZone>().target != null) {
                animator.SetBool("found", true);
                moveSpeed = startSpeed * 2;
                Vector3 targetPostition = new Vector3(searchZone.GetComponent<SearchZone>().target.transform.position.x, transform.position.y, searchZone.GetComponent<SearchZone>().target.transform.position.z);
                transform.LookAt(targetPostition);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else {
                moveSpeed = startSpeed;
                animator.SetBool("found", false);
                Argo();
            }
        }
        else
        {
            moveSpeed = startSpeed;
            animator.SetBool("found", false);
            damageZone.GetComponent<DamageZoneDrain>().damagePerSec = startDamage / 2;
            searchZone.SetActive(false);
            Argo();
        }

    }


    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 7);

        isWalking = true;
        walkCounter = walkTime;
    }

    public void Argo()
    {
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    rotation = Mathf.SmoothDamp(rotation, 0, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    rotation = Mathf.SmoothDamp(rotation, 45f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    rotation = Mathf.SmoothDamp(rotation, 90f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    rotation = Mathf.SmoothDamp(rotation, 135f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 4:
                    rotation = Mathf.SmoothDamp(rotation, 180f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 5:
                    rotation = Mathf.SmoothDamp(rotation, -45f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 6:
                    rotation = Mathf.SmoothDamp(rotation, -90f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 7:
                    rotation = Mathf.SmoothDamp(rotation, -135f, ref rotationSPeed, 5f);
                    transform.rotation = Quaternion.Euler(0, rotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }

            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;

                //stop movement
                transform.position = stopPosition;
                animator.SetBool("isWalking", false);

                //reset the waitCounter
                waitCounter = waitTime;
            }

        }
        else
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }
}