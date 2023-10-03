using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform _camHolder;
    [SerializeField] Transform _ori;
    public float _sens;
    private float _yRot;
    private float _xRot;
    public float RotClampAddition;


    [Header("Singleton")]
    public static CameraScript instance;
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

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sens;
        _yRot += mouseX;
        _xRot -= mouseY;

        _xRot = Mathf.Clamp(_xRot, -30f,30f);
        _yRot = Mathf.Clamp(_yRot, -90f, 90f);

        _camHolder.rotation = Quaternion.Euler(_xRot, _yRot, 0);
        _ori.rotation = Quaternion.Euler(0, _yRot, 0);
    }
    public void SetAdditions(float value)
    {                       
        RotClampAddition = value;
    }
}
