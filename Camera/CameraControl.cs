using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private PlayerController playerController;
    private void Start()
    {
        playerController = PlayerController.instance;     
    }
    private void Update()
    {
        if((playerController.Situation == PlayerController.State.OnAir) && playerController.isVertical)
        {
            Vector3 vector = new Vector3(0, 0, 0.75f * (playerController.JumpForce/100) );
            transform.position = Vector3.Lerp(transform.position, transform.position + vector, Time.deltaTime * 2f);
        }
    }
}
