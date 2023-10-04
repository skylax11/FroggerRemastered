using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    [Header("Singleton")]
    public static InputManager Instance;

    public Vector2 move;
    public bool turning;
    public bool onAir;
    public bool isResetted;
    private float _continuesTime;
    private float _pastTime;
    private PlayerController playerController;
    private Animator animator;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    public void OnJump(InputValue input)
    {
        animator.SetBool("GetReady", true);

        playerController.WillJump = true;
        _continuesTime = Time.time;
        _pastTime = _pastTime == 0 ? Time.time : _pastTime;

        if(input.Get<Vector2>() == Vector2.zero) // second input turns (0,0) when we stopped pushing space bar. 
        {

            float magnitude = (_continuesTime - _pastTime)*20f;

            magnitude = magnitude > 50 ? 50 : magnitude; 

            if (playerController.Situation == PlayerController.State.OnGround)
            {
                playerController.Jump(100 + magnitude);
                animator.SetBool("Jump", true);
            }

            _pastTime = 0f; // reset starting point
        }

    }
    public void OnMove(InputValue input)
    {
        if (input.Get<Vector2>() != Vector2.zero)
        {
            playerController.stayVertical = false;
            SetMove(input.Get<Vector2>());
        }
    }
    public void OnReset()
    {
        playerController.ResetRotation = true;
        playerController.isVertical = true;
        playerController.stayVertical = true;
        SetReset(true);
    }
    public void OnSave()
    {
        PlayerController.instance.Save();
    }
    public void SetMove(Vector2 vector)
    {
        move = vector;  
    }
    public void SetTurning(bool turning)
    {
        this.turning = turning;
    }
    public void SetOnAir(bool onAir)
    {
        this.onAir = onAir;
    }
    public void SetReset(bool isResetted)
    {
        this.isResetted = isResetted;
    }
}
