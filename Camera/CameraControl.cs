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
        if ((playerController.Situation == PlayerController.State.OnAir) && playerController.isVertical)
        {
            Vector3 vector = new Vector3(0, 0, 1.3f * (playerController.JumpForce/100) );
            transform.position = Vector3.Lerp(transform.position, transform.position + vector, Time.deltaTime * 2f);
        }
        else if ((playerController.Situation == PlayerController.State.OnAir) && !playerController.isVertical)
        {
            Vector3 vector;
            vector = new Vector3(1.25f * (playerController.JumpForce / 100), 0, 0f);
            if (playerController.transform.rotation.y < 0)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - vector, Time.deltaTime * 2f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + vector, Time.deltaTime * 2f);
            }
        }
    }
}
