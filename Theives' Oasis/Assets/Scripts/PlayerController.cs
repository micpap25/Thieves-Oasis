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
    public Vector3 temp;

    private float currentSpeed;
    private bool roll;
    private int rollStart;
    private char currdiV;
    private char currdiH;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = moveSpeed;
        roll = false;
        currdiV = 'w';
        currdiH = 'n';
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if (roll && Time.frameCount - rollStart < 10)
        {
            temp.x = 0;

            if (currdiV == 'w')
            {
                transform.Translate(0, rollSpeed, 0);
            }
            else if (currdiV == 's')
            {
                transform.Translate(0, -rollSpeed, 0);
            }

            if (currdiH == 'd')
            {
                transform.Translate(rollSpeed, 0, 0);
            }
            else if (currdiH == 'a')
            {
                transform.Translate(-rollSpeed, 0, 0);
            }

        }
        else
        {

            
            roll = false;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = crouchSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Space) && Time.frameCount - rollStart >= 60)
            {
                rollStart = Time.frameCount;
                roll = true;
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, currentSpeed, 0);
                currdiV = 'w';
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -currentSpeed, 0);
                currdiV = 's';
            }
            else
            {
                currdiV = 'n';
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(currentSpeed, 0, 0);
                currdiH = 'd';
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-currentSpeed, 0, 0);
                currdiH = 'a';
            }
            else
            {
                currdiH = 'n';
            }
        }
       // bar.transform.localScale = temp;
    }
}