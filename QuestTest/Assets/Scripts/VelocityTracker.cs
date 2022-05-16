using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    private Vector3[] PositionBuffer = new Vector3[10];

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        for (int i = 0; i < PositionBuffer.Length; i++)
        {
            PositionBuffer[i] = transform.position;
        }
    }


    private Vector3 lastPosition;

    public Vector3 Velocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        Velocity = GetHighestVelocity();

        PositionBuffer[index] = transform.position;
        index = (index + 1) % PositionBuffer.Length;
    }

    private Vector3 GetHighestVelocity()
    {
        Vector3 highestVelocity = Vector3.zero;

        for (int i = 0; i < PositionBuffer.Length; i++)
        {
            Vector3 vel = (transform.position -
                           PositionBuffer[(index + PositionBuffer.Length - i) % PositionBuffer.Length]) /
                          (Time.fixedDeltaTime * (i + 1));

            if (vel.magnitude > highestVelocity.magnitude)
            {
                highestVelocity = vel;
            }
        }

        return highestVelocity;
    }
}