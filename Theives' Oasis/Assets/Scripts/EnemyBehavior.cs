using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private float degreeFacing;
    public float degreeLineOfSight;
    public float distanceLineOfSight;
    public float distanceFollowing;
    private float originalDegreeLineOfSight;
    private float originalDistanceLineOfSight;
    public bool hasGun;
    private float degreeToPlayer;
    private float distanceFromPlayer;
    private float distanceFromNextPoint;
    private GameObject player;
    private float tempAngle;
    private bool playerSpotted;
    public float playerSpottedSpeed;
    public float playerSpottedDegreeLineOfSight;
    public float playerSpottedDistanceLineOfSight;
    public Transform[] points;
    public int timeBetweenPoints;
    private int curPoint;
    private int nextPoint;
    private Vector2 movementToNextPoint;
    private float lerpVar;
    public float offset;
    private bool resetting;

    void Start()
    {
        //TODO: Set this from the start!
        transform.position = points[points.Length-1].position;
        //Code for setting enemy's sight range
        originalDegreeLineOfSight = degreeLineOfSight;
        originalDistanceLineOfSight = distanceLineOfSight;
        player = GameObject.FindGameObjectWithTag("Player");
        if (points[0] != null)
        {
            //Code for setting enemy's movement and direction facing.
            curPoint = points.Length - 1;
            nextPoint = 0;
            lerpVar = 0;
            //degreeFacing = Vector2.SignedAngle(transform.position, points[curPoint].position - transform.position);
            Vector2 direction = (points[nextPoint].position - transform.position).normalized;
            degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);

        }
        //else
        //    degreeFacing = -45;
    }

    // Update is called once per frame
    void Update()
    {
        //Code that gets necessary values, like angles and distance from player and important sight/movement variables.
        distanceLineOfSight = playerSpotted ? playerSpottedDistanceLineOfSight : originalDistanceLineOfSight;
        degreeLineOfSight = playerSpotted ? playerSpottedDegreeLineOfSight : originalDegreeLineOfSight;
        distanceFromNextPoint = Mathf.Sqrt(Mathf.Pow(transform.position.x - points[nextPoint].transform.position.x, 2) + Mathf.Pow(transform.position.y - points[nextPoint].transform.position.y, 2));
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
//        if(Raycast(transform.position,Vector3(Mathf.cos(degreeToPlayer),Mathf.sin(degreeToPlayer),0), distanceFromPlayer,int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal))
        if (degreeToPlayer > degreeFacing - degreeLineOfSight && degreeToPlayer < degreeFacing + degreeLineOfSight && distanceFromPlayer < distanceLineOfSight && distanceFromNextPoint < distanceFollowing ) {
            playerSpotted = true;
        }
        /*int environment = LayerMask.GetMask("Environment");

if (!Physics.Linecast(shooter.transform.position, target.transform.position, environment)) {
// Fire!

}*/
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
            transform.position = new Vector2(transform.position.x + (movementToNextPoint.x * playerSpottedSpeed * Time.deltaTime), transform.position.y + (movementToNextPoint.y * playerSpottedSpeed * Time.deltaTime));
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
                nextPoint = (nextPoint + 1) % points.Length;
                lerpVar = 0;
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
                curPoint = (curPoint + 1) % points.Length;
                resetting = false;
            }
            //Movement to next point.
            if (!(this.transform.position.x > (points[nextPoint].position.x) - offset && this.transform.position.x < (points[nextPoint].position.x) + offset && this.transform.position.y > (points[nextPoint].position.y) - offset && this.transform.position.y < (points[nextPoint].position.y) + offset))
            //if(!this.transform.position.Equals(points[nextPoint].position))
            {
                lerpVar += Time.deltaTime/timeBetweenPoints;
                transform.position = Vector3.Lerp(points[curPoint].position, points[nextPoint].position, lerpVar);
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
            }
            //Sets to next point 
            else
            {
                Debug.Log("At next point!");
                //factoring the angle it came into the point into its new rotation in some way
                transform.position = points[nextPoint].position;
                nextPoint = (nextPoint + 1) % points.Length;
                lerpVar = 0;
                direction = (points[nextPoint].position - transform.position).normalized;
                degreeFacing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(degreeFacing - 45, Vector3.forward);
                curPoint = (curPoint + 1) % points.Length;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Code for when the enemy hits a wall.
        //Debug.Log("inside onCollision");
        if (collision.gameObject.tag.Equals("Wall"))
        {
            //Debug.Log("Hit the wall");
            resetting = true;
        }
    }
}
