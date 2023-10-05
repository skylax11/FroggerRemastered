using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public State Situation;
    public Vector2 direction;

    public float CurrentSpeed = 0f;
    public const float MoveSpeed = 3.2f;

    public float JumpForce;
    private float _rotation;

    private Rigidbody rigidBody;

    private float _timeMinusBonus;

    public bool WillJump;
    public bool ResetRotation;
    public bool isVertical;
    public bool stayVertical;

    private float _pastTime;
    private float _continuesTime;
    private float _endTime;

    public Image Bar;
    [Header("UI_Handler")]
    [SerializeField] GameObject _handlerGameObject;
    [SerializeField] UI_Handler _handlerParentScript;
    [Header("Singleton")]
    public static PlayerController instance;
    [Header("CameraControl")]
    [SerializeField] CameraControl cameraControl; 
    public enum State
    {
        OnAir,
        OnGround,
    }
    public void AddScore(int score)
    {
        MenuManager.Instance.playerProps.score += score;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform _collision = collision.transform;

        Situation = State.OnGround;
        if (_collision.CompareTag("coin"))
        {
            collision.gameObject.SetActive(false);
            AddScore(30);
        }
        if (_collision.CompareTag("superCoin"))
        {
            collision.gameObject.SetActive(false);
            AddScore(100);
        }
        if (_collision.CompareTag("superCoin2"))
        {
            collision.gameObject.SetActive(false);
            AddScore(300);
        }
        if (_collision.CompareTag("platform"))
        {
            AddScore(10);
        }
        if (_collision.CompareTag("end"))
        {
            EndGame(false);
            
        }
        if (_collision.CompareTag("lastPlatform"))
        {
            EndGame(true); 
        }
        _handlerParentScript.UpdateScore(MenuManager.Instance.playerProps.score);
    }
    public void EndGame(bool win)
    {
        _endTime = Time.time;
        float timeMinusBonus = _endTime * 0.75f;
        MenuManager.Instance.playerProps.score -= (int)timeMinusBonus;
        _handlerGameObject.SetActive(true);

        if (win)
        {
            _handlerParentScript.WinText(MenuManager.Instance.playerProps.score, timeMinusBonus);
        }
        else
        {
            if (MenuManager.Instance.playerProps.score < 0)
            {
                MenuManager.Instance.playerProps.score = 0;
            }
            gameObject.GetComponent<Collider>().enabled = false;
            _handlerParentScript.LostText(MenuManager.Instance.playerProps.score, timeMinusBonus);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Save();
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rigidBody = GetComponent<Rigidbody>();
        Situation = State.OnGround;
    }

    void Update()
    {
        FilledAmountOfBar();
        Move();
        if(Situation == State.OnGround && !WillJump) Rotate(direction);

    }
    public void FilledAmountOfBar()
    {
        if (!WillJump) 
        {
            _pastTime = 0f;
            JumpForce = Mathf.Lerp(JumpForce , 0f , Time.deltaTime*20f);
        }
        else
        {
            _continuesTime = Time.time;
            _pastTime = _pastTime == 0 ? Time.time : _pastTime;
            float tempMagnitude = (_continuesTime - _pastTime) * 20f;  // Same with InputManager. We need that info for a smooth bar
            tempMagnitude = tempMagnitude > 50 ? 50 : tempMagnitude;
            JumpForce = 100 + tempMagnitude;
            
        }
        JumpForce = Mathf.Clamp(JumpForce, 100, 150);
        Bar.fillAmount = Mathf.Lerp(Bar.fillAmount, (JumpForce % 100)/50, Time.deltaTime * 6f);

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
    
    public void Rotate(Vector2 direction)   // COULD BE BETTER
    {
        if ((gameObject.transform.rotation.eulerAngles.y <= 0.1f && gameObject.transform.rotation.eulerAngles.y >=-0.1f) && InputManager.Instance.isResetted)
        {
            InputManager.Instance.turning = false;
            stayVertical = true; 
            ResetRotation = false; 
            InputManager.Instance.isResetted = false;
        }

        if (_rotation == 0f)
        {
            isVertical = true;
            ResetRotation = false;
            _rotation = Mathf.Lerp(_rotation, direction.x * Mathf.Rad2Deg * 0.78f, Time.deltaTime * 2f);
        }
        if ((_rotation != 0f && !ResetRotation) && !stayVertical)
        {
            isVertical = false;
            float oldRotation = _rotation;
            _rotation = Mathf.Lerp(_rotation, oldRotation + direction.x * Mathf.Rad2Deg * 0.78f, Time.deltaTime * 2f);
            _rotation = Mathf.Clamp(_rotation, -90f, 90f);
        }
        if (ResetRotation && stayVertical)
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
    }
    public void Save()
    {
        print("a");
        JSONdatabase.instance.Save(MenuManager.Instance.playerProps);
    }
}
