using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float moveSpeed;
    public float crouchSpeed;
    public float rollSpeed;
    public int rolltime;
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
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = moveSpeed;
        roll = false;
        dead = false;
        won = false;
        currdiV = 'w';
        currdiH = 'n';
        
        //Instantiate(stamBar, new Vector3(transform.position.x, transform.position.y - 2, 0), transform.rotation, transform);
        objectives = GameObject.FindGameObjectsWithTag("Objective").Length;
    }

    // Update is called once per frame
    void Update()
    {
        //Do this if an end condition hasn't been reached.
        if (!dead && !won)
        {
            //code for rolling
            if (roll && Time.frameCount - rollStart < rolltime)
            {

                temp.x = 0;

                if (currdiV == 'w')
                {
                    transform.Translate(0, rollSpeed * Time.deltaTime, 0);
                }
                else if (currdiV == 's')
                {
                    transform.Translate(0, -rollSpeed * Time.deltaTime, 0);
                }

                if (currdiH == 'd')
                {
                    transform.Translate(rollSpeed * Time.deltaTime, 0, 0);
                }
                else if (currdiH == 'a')
                {
                    transform.Translate(-rollSpeed * Time.deltaTime, 0, 0);
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
                //code for movement
                if (Input.GetKeyDown(KeyCode.Space) && Time.frameCount - rollStart > 60)
                {

                    rollStart = Time.frameCount;
                    roll = true;
                    StartCoroutine(Stamina());
                }

                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(0, currentSpeed * Time.deltaTime, 0);
                    currdiV = 'w';
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(0, -currentSpeed * Time.deltaTime, 0);
                    currdiV = 's';
                }
                else
                {
                    currdiV = 'n';
                }

                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(currentSpeed * Time.deltaTime, 0, 0);
                    currdiH = 'd';
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(-currentSpeed * Time.deltaTime, 0, 0);
                    currdiH = 'a';
                }
                else
                {
                    currdiH = 'n';
                }
            }
            // bar.transform.localScale = temp;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Code for hitting different things
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
        //Thread to work on stamina bar
        Vector3 org = stamBar.transform.localScale;
        /*        for (int i = 0; i < rolltime; i++)
                {
                                Vector3 temp = stamBar.transform.localScale;
                                temp.Set(temp.x - org.x / rolltime, temp.y, temp.z);
                                stamBar.transform.localScale = temp;

                }*/
        while (Time.frameCount-rollStart <= rolltime)
        {
        
            Vector3 temp = stamBar.transform.localScale;

            temp.Set(temp.x - org.x / rolltime, temp.y, temp.z);
            stamBar.transform.localScale = temp;
            yield return null;

        }
        /*        for (int i = 0; i < 60; i++)
                {
                                Vector3 temp = stamBar.transform.localScale;
                                Debug.Log(temp.x);
                                temp.Set(temp.x + org.x / 60, temp.y, temp.z);
                                stamBar.transform.localScale = temp;

                }*/


        while (Time.frameCount-rollStart <= 60)
        {
            Vector3 temp = stamBar.transform.localScale;

            temp.Set(temp.x + org.x / (60-rolltime), temp.y, temp.z);
            stamBar.transform.localScale = temp;
            yield return null;
        }


        stamBar.transform.localScale = new Vector3(org.x,org.y,org.z);
        yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Code for using vents.
        Debug.Log("Collided!");
        if (collision.gameObject.tag.Equals("Vent") && Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = collision.gameObject.GetComponent<VentInfo>().telePoint();
        }
    }

}