using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public State Situation;
    public Vector2 direction;

    public float CurrentSpeed = 0f;
    public const float MoveSpeed = 3.2f;

    public float JumpForce;
    private float _rotation;

    private Rigidbody rigidBody;

    public bool WillJump;
    public bool ResetRotation;
    public bool isVertical;

    [Header("Singleton")]
    public static PlayerController instance;
    [Header("CameraControl")]
    [SerializeField] CameraControl cameraControl; 
    public enum State
    {
        OnAir,
        OnGround,
    }
    private void OnCollisionEnter(Collision collision)
    {
        Situation = State.OnGround;
    }
    private void OnCollisionExit(Collision collision)
    {
        Situation = State.OnAir;
        WillJump = false;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Situation = State.OnGround;
    }

    void Update()
    {
        Move();
        if(Situation == State.OnGround && !WillJump) Rotate(direction);

    }
    public void Move()
    {
        if (Situation == State.OnGround)
        {
            direction = InputManager.Instance.move;

            if (direction == Vector2.zero)   // If no buttons are pressed, return 
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, MoveSpeed, Time.deltaTime*3f); 
                return;
            }
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, MoveSpeed, Time.deltaTime);  // Setting Speed
        }
    }
    
    public void Rotate(Vector2 direction)
    {
        if (_rotation == 0f)
        {
            isVertical = true;
            ResetRotation = false;
            _rotation = Mathf.Lerp(_rotation, direction.x * Mathf.Rad2Deg * 0.78f, Time.deltaTime * 2f);
        }
        else if (_rotation != 0f && !ResetRotation)
        {
            isVertical = false;
            float oldRotation = _rotation;
            _rotation = Mathf.Lerp(_rotation, oldRotation + direction.x * Mathf.Rad2Deg * 0.78f, Time.deltaTime * 2f);
            _rotation = Mathf.Clamp(_rotation, -90f, 90f);
        }
        else if (ResetRotation)
        {
            isVertical = true;
            _rotation = Mathf.Lerp(_rotation, 0f, Time.deltaTime * 2f);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, _rotation ,0.0f), Time.deltaTime*50f);
    }
    public void Jump(float magnitude)
    {
        Vector3 temp = transform.position;

        Vector3 vertical = new Vector3(0f, 1f*magnitude/100, 0f);
        rigidBody.AddForce((transform.forward + vertical) * magnitude);
        JumpForce = magnitude;
    }
}
