﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float moveSpeed;
    public float crouchSpeed;
    public float rollSpeed;
    public float rolltime;
    public Vector3 temp;
    private float currentSpeed;
    private bool roll;
    private int rollStart;
    private char currdiV;
    private char currdiH;
    private int objectives;
    public bool dead;
    public bool won;
    public Text text;
    public GameObject stamBar;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = moveSpeed;
        roll = false;
        currdiV = 'w';
        currdiH = 'n';
        Instantiate(stamBar, new Vector3(transform.position.x, transform.position.y - 5, 0), transform.rotation, transform);
        stamBar.transform.localScale = new Vector3 (rolltime, stamBar.transform.localScale.y, stamBar.transform.localScale.z);
        objectives = GameObject.FindGameObjectsWithTag("Objective").Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead && !won)
        {
            if (roll && Time.frameCount - rollStart < rolltime)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            //Destroy(collision.gameObject);
            Debug.Log("Oof you're dead");
            dead = true;
        }
        if (collision.gameObject.tag.Equals("Objective"))
        {
            objectives--;
            Destroy(collision.gameObject);
            if (objectives == 0)
                Debug.Log("You have the diamond! Now run!");
            else
                Debug.Log("You got a diamond!");
        }

        if (collision.gameObject.tag.Equals("Goal"))
        {
            if (objectives == 0)
            {
                won = true;
                Debug.Log("You win!");
            }
            else
                Debug.Log("No! You need the diamond!");
        }
        if (dead)
            text.text = "You're Dead!";
        if (won)
            text.text = "You Win!";

    }

    private IEnumerator Stamina ()
    {

        yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Vent") && Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.position = collision.gameObject.GetComponent<VentInfo>().telePoint;
        }
    }

}