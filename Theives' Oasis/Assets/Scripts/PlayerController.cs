using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float moveSpeed;
    public float crouchSpeed;
    public float currentSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
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
            transform.Translate(0, moveSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -moveSpeed, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed, 0, 0);
        }
    }
}
