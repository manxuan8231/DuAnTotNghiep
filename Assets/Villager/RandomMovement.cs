using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform Point;
    Vector3 dest;

    void Update()
    {
        dest = Point.position;
        ai.destination = dest;
    }
}
