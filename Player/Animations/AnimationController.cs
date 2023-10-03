using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    [Header("Singleton")]
    public static AnimationController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetJumpState()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("GetReady", false);

    }
}
