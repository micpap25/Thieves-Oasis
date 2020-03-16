using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentInfo : MonoBehaviour
{
    public Transform pairedVent;
    public Vector3 distanceFromPoint;

    public Vector3 telePoint(){
        return pairedVent.position + distanceFromPoint;
    }
}
