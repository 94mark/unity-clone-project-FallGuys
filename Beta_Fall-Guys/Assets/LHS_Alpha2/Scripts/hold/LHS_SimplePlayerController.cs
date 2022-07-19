using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_SimplePlayerController : MonoBehaviour
{
    // Animator asset
    public Animator animator;

    void Update()
    {
        // Set both horizontal and vertical axes to the player's input
        animator.SetFloat("h", Input.GetAxis("Horizontal"));
        animator.SetFloat("v", Input.GetAxis("Vertical"));
    }
}
