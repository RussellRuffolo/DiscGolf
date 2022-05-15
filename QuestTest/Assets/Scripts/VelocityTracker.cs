using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }


    private Vector3 lastPosition;

    public Vector3 Velocity;

    // Update is called once per frame
    void Update()
    {
        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        Debug.Log("Velocity is: " + Velocity.magnitude);
        lastPosition = transform.position;
    }
}