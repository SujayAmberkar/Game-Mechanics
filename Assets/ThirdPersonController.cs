using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // get speed property from animation controller
    private Animator animator;


    void Start(){
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = Input.GetKey(KeyCode.W);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning && isWalking);

    }
}
