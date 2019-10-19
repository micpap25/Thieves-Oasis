using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float moveSpeed;
    public float crouchSpeed;
    public float rollSpeed;

    private float currentSpeed;
    private bool roll;
    private int rollStart;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = moveSpeed;
        roll = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (!roll)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = crouchSpeed;
            }

            else if (Input.GetKeyDown(KeyCode.Tab) && Time.frameCount-rollStart>=60)
            {
                currentSpeed = rollSpeed;
                roll = true;
                rollStart = Time.frameCount;
            }

            else
            {
                currentSpeed = moveSpeed;
            }
        }

        if (roll && Time.frameCount - rollStart >= 10)
        {
            currentSpeed = moveSpeed;
            roll = false;
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
