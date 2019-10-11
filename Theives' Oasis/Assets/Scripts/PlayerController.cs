using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float crouchSpeed;
    public float currentSpeed;
    void Start()
    {
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = crouchSpeed;
        } 
        else
        {
            currentSpeed = moveSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, currentSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -currentSpeed, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(currentSpeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-currentSpeed, 0, 0);
        }
    }
}
