using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float degreeFacing;
    [SerializeField] private float degreeToPlayer;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private GameObject player;
    void Start()
    {
        degreeFacing = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceFromPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.y - player.transform.position.y, 2));
        degreeToPlayer = Vector3.SignedAngle(player.transform.position - transform.position, transform.position, Vector3.forward);
        if (degreeFacing > 160)
        {
            if (degreeToPlayer > degreeFacing - 20 && degreeToPlayer < degreeFacing - 340 && distanceFromPlayer < 10)
            {
                Debug.Log("ALERT ALERT");
            }
        }
        else if (degreeFacing < -160)
        {
            if (degreeToPlayer > degreeFacing + 340 && degreeToPlayer < degreeFacing + 20 && distanceFromPlayer < 10)
            {
                Debug.Log("ALERT ALERT");
            }
        }

        else
        {
            if (degreeToPlayer > degreeFacing - 20 && degreeToPlayer < degreeFacing + 20 && distanceFromPlayer < 10)
            {
                Debug.Log("ALERT ALERT");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //Destroy(collision.gameObject);
            Debug.Log("Oof you're dead");
        }
    }
}
