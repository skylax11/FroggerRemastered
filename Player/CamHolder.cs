using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolder : MonoBehaviour
{
    [SerializeField] Transform _camera;
    void Update()
    {
        transform.position = _camera.transform.position;
    }
}
