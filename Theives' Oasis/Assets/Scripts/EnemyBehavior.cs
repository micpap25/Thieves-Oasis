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
    public Transform[] points;
    public int timeBetweenPoints;
    public int curPoint;
    public int nextPoint;
    public Vector2 movementToNextPoint;
    
    //degree facing has up-right as 0
    //always adjust so they face the direction they're moving
    // (unless they're drunk which they very well might be)

    void Start()
    {
        degreeFacing = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        if (points[0] != null)
        {
            curPoint = 0;
            //movementToNextPoint = new Vector2(Mathf.Sqrt(Mathf.Pow(points[curPoint].position.x, 2) - Mathf.Pow(transform.position.x, 2)) / timeBetweenPoints, Mathf.Sqrt(Mathf.Pow(points[curPoint].position.y, 2) - Mathf.Pow(transform.position.y, 2)) / timeBetweenPoints);
            movementToNextPoint = new Vector2((points[curPoint].position.x - transform.position.x) / timeBetweenPoints, (points[curPoint].position.y - transform.position.y) / timeBetweenPoints);
            degreeFacing = Vector3.SignedAngle(transform.position, points[curPoint].position - transform.position, Vector3.forward);
            //degreeFacing = degreeFacing < -135 ? degreeFacing += 315 : degreeFacing -= 45;
            //transform.LookAt(points[curPoint]);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(points[curPoint].position - transform.position), Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(degreeFacing, Vector3.forward);

        }
        //else
        //    degreeFacing = -45;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceFromPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.y - player.transform.position.y, 2));
        degreeToPlayer = Vector3.SignedAngle(transform.position, player.transform.position - transform.position, Vector3.forward);
        //These 2 sections are incorrect
        //Fix angles and stuff
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
        if (points[0] != null)
        {
            if (!(this.transform.position.x > (points[curPoint].position.x) - .05 && this.transform.position.x < (points[curPoint].position.x) + .05 && this.transform.position.y > (points[curPoint].position.y) - .05 && this.transform.position.y < (points[curPoint].position.y) + .05))
            {
                //gameObject.transform.Translate(movementToNextPoint.x, movementToNextPoint.y, 0);
                transform.position = new Vector3(transform.position.x + movementToNextPoint.x, transform.position.y + movementToNextPoint.y, 0);
                degreeFacing = Vector3.SignedAngle(transform.position, points[nextPoint].position - transform.position, Vector3.forward);
                transform.rotation = Quaternion.AngleAxis(degreeFacing, Vector3.forward);
            }
            else
            {
                //factoring the angle it came into the point into its new rotation in some way
                degreeFacing = 0;
                transform.position = points[curPoint].position;
                nextPoint = (curPoint + 1) % points.Length;
                movementToNextPoint = new Vector2((points[nextPoint].position.x - transform.position.x) / timeBetweenPoints, (points[nextPoint].position.y - transform.position.y) / timeBetweenPoints);
                degreeFacing = Vector3.SignedAngle(transform.position, points[nextPoint].position - transform.position, Vector3.forward);
                transform.rotation = Quaternion.AngleAxis(degreeFacing, Vector3.forward);
                curPoint = nextPoint;
            }
        }

    }
}
