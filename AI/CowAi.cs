using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AI_Movement : MonoBehaviour
{

    Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    public float eatCounter;
    public float cowRotationSPeed = 2f;
    private float cowRotation;

    int WalkDirection;

    public bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //So that all the prefabs don't move/stop at the same time
        walkTime = Random.Range(3, 5);
        waitTime = Random.Range(5, 7);
        eatCounter = Random.Range(1, 9);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    cowRotation = Mathf.SmoothDamp(cowRotation, 0, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    cowRotation = Mathf.SmoothDamp(cowRotation, 45f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    cowRotation = Mathf.SmoothDamp(cowRotation, 90f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    cowRotation = Mathf.SmoothDamp(cowRotation, 135f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 4:
                    cowRotation = Mathf.SmoothDamp(cowRotation, 180f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 5:
                    cowRotation = Mathf.SmoothDamp(cowRotation, -45f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 6:
                    cowRotation = Mathf.SmoothDamp(cowRotation, -90f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 7:
                    cowRotation = Mathf.SmoothDamp(cowRotation, -135f, ref cowRotationSPeed, 0.2f);
                    transform.rotation = Quaternion.Euler(0, cowRotation, 0);
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
            eatCounter -= Time.deltaTime;

            if (eatCounter <= 0) {
                animator.SetBool("EAT", false);
            }
            else if (waitCounter > eatCounter) { 
                animator.SetBool("EAT", true); 
            }

            if (waitCounter <= 0)
            {
                ChooseDirection();
                eatCounter = Random.Range(1, 9);
            }
        }
    }


    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 7);

        isWalking = true;
        walkCounter = walkTime;
    }
}