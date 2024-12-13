using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AI_Movement : MonoBehaviour
{

    public Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    public float eatCounter;
    public float rotationSPeed = 2f;
    private float rotation;
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
        if (FindFirstObjectByType<LightingManager>().TimeOfDay < 4 || FindFirstObjectByType<LightingManager>().TimeOfDay > 20f) {
            animator.SetBool("Sleep", true);
        } 
        else {
            animator.SetBool("Sleep", false);

            if (isWalking)
            {
                animator.SetBool("isWalking", true);
                walkCounter -= Time.deltaTime;

                switch (WalkDirection)
                {
                    case 0:
                        rotation = Mathf.SmoothDamp(rotation, 0, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 1:
                        rotation = Mathf.SmoothDamp(rotation, 45f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 2:
                        rotation = Mathf.SmoothDamp(rotation, 90f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 3:
                        rotation = Mathf.SmoothDamp(rotation, 135f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 4:
                        rotation = Mathf.SmoothDamp(rotation, 180f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 5:
                        rotation = Mathf.SmoothDamp(rotation, -45f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 6:
                        rotation = Mathf.SmoothDamp(rotation, -90f, ref rotationSPeed, 0.2f);
                        transform.rotation = Quaternion.Euler(0, rotation, 0);
                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        break;
                    case 7:
                        rotation = Mathf.SmoothDamp(rotation, -135f, ref rotationSPeed, 0.2f);
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
                eatCounter -= Time.deltaTime;

                if (eatCounter <= 0)
                {
                    animator.SetBool("EAT", false);
                }
                else if (waitCounter > eatCounter)
                {
                    animator.SetBool("EAT", true);
                }

                if (waitCounter <= 0)
                {
                    ChooseDirection();
                    eatCounter = Random.Range(1, 9);
                }
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