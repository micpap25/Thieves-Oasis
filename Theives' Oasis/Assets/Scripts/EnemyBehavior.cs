using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float degreeFacing;
    public float degreeLineOfSight;
    public float distanceLineOfSight;
    public float distanceFollowing;
    private float originalDegreeLineOfSight;
    private float originalDistanceLineOfSight;
    public bool hasGun;
    [SerializeField] private float degreeToPlayer;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float distanceFromNextPoint;
    [SerializeField] private GameObject player;
    public float tempAngle;
    public bool playerSpotted;
    public float playerSpottedSpeed;
    public float playerSpottedDegreeLineOfSight;
    public float playerSpottedDistanceLineOfSight;
    public Transform[] points;
    public int timeBetweenPoints;
    public int curPoint;
    public int nextPoint;
    public Vector2 movementToNextPoint;
    public bool resetting;
    
    //degree facing has up-right as 0
    //always adjust so they face the direction they're moving
    // (unless they're drunk which they very well might be)

    void Start()
    {
        //Code for setting enemy's sight range
        originalDegreeLineOfSight = degreeLineOfSight;
        originalDistanceLineOfSight = distanceLineOfSight;
        player = GameObject.FindGameObjectWithTag("Player");
        if (points[0] != null)
        {
            //Code for setting enemy's movement and direction facing.
            curPoint = 0;
            movementToNextPoint = new Vector2((points[curPoint].position.x - transform.position.x) / timeBetweenPoints, (points[curPoint].position.y - transform.position.y) / timeBetweenPoints);
            //degreeFacing = Vector2.SignedAngle(transform.position, points[curPoint].position - transform.position);
            Vector2 direction = (points[curPoint].position - transform.position).normalized;
            degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);

        }
        //else
        //    degreeFacing = -45;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Code that gets necessary values, like angles and distance from player and important sight/movement variables.
        distanceLineOfSight = playerSpotted ? playerSpottedDistanceLineOfSight : originalDistanceLineOfSight;
        degreeLineOfSight = playerSpotted ? playerSpottedDegreeLineOfSight : originalDegreeLineOfSight;
        distanceFromNextPoint = Mathf.Sqrt(Mathf.Pow(transform.position.x - points[curPoint].transform.position.x, 2) + Mathf.Pow(transform.position.y - points[curPoint].transform.position.y, 2));
        distanceFromPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.y - player.transform.position.y, 2));
        Vector2 direction = (player.transform.position - transform.position).normalized;
        degreeToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //These 2 sections are incorrect
        //Fix angles and stuff

        //Checks if the player has been spotted and adjusts angles.
        if (degreeToPlayer < -180 + degreeLineOfSight && degreeFacing > 180 - degreeLineOfSight)
            degreeToPlayer += 360;
        if (degreeFacing < -180 + degreeLineOfSight && degreeToPlayer > 180 - degreeLineOfSight)
            degreeToPlayer -= 360;
        if (degreeToPlayer > degreeFacing - degreeLineOfSight && degreeToPlayer < degreeFacing + degreeLineOfSight && distanceFromPlayer < distanceLineOfSight && distanceFromNextPoint < distanceFollowing ) {
            playerSpotted = true;
        }
        else
        {
            if (playerSpotted)
                resetting = true;
            playerSpotted = false;
        }
           
        //Chase the player if the player has been spotted.
        if (playerSpotted)
        {
            movementToNextPoint = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degreeToPlayer), Mathf.Sin(Mathf.Deg2Rad * degreeToPlayer));
            direction = (player.transform.position - transform.position).normalized;
            degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
            transform.position = new Vector2(transform.position.x + (movementToNextPoint.x * playerSpottedSpeed), transform.position.y + (movementToNextPoint.y * playerSpottedSpeed));
            //Debug.Log("ALERT ALERT ALERT");
            if (hasGun)
            {

            }

        }

        else if (points[0] != null)
        {
            // 2 reasons to reset; done chasing player or touches wall
            // WORK ON THE SECOND ONE

            //"resetting" is triggered if the enemy hits a wall or stops seeing the player.
            //gets the opponent going to the next point.
            if (resetting)
            {
                nextPoint = (curPoint + 1) % points.Length;
                movementToNextPoint = new Vector2((points[nextPoint].position.x - transform.position.x) / timeBetweenPoints, (points[nextPoint].position.y - transform.position.y) / timeBetweenPoints);
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
                curPoint = nextPoint;
                resetting = false;
            }
            //Movement to next point.
            if (!(this.transform.position.x > (points[curPoint].position.x) - .05 && this.transform.position.x < (points[curPoint].position.x) + .05 && this.transform.position.y > (points[curPoint].position.y) - .05 && this.transform.position.y < (points[curPoint].position.y) + .05))
            {
                //gameObject.transform.Translate(movementToNextPoint.x, movementToNextPoint.y, 0);
                transform.position = new Vector2(transform.position.x + movementToNextPoint.x, transform.position.y + movementToNextPoint.y);
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
            }
            //Sets to next point 
            else
            {
                //factoring the angle it came into the point into its new rotation in some way
                transform.position = points[curPoint].position;
                nextPoint = (curPoint + 1) % points.Length;
                movementToNextPoint = new Vector2((points[nextPoint].position.x - transform.position.x) / timeBetweenPoints, (points[nextPoint].position.y - transform.position.y) / timeBetweenPoints);
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
                curPoint = nextPoint;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Code for when the enemy hits a wall.
        //Debug.Log("inside onCollision");
        if (collision.gameObject.tag.Equals("Wall"))
        {
            Debug.Log("Hit the wall");
            resetting = true;
        }
    }
}
