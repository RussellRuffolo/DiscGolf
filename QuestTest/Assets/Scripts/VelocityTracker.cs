using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using VRTK;

public class VelocityTracker : MonoBehaviour
{
    private Vector3[] PositionBuffer = new Vector3[10];

    private int index;

    //Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PositionBuffer.Length; i++)
        {
            PositionBuffer[i] = transform.position;
        }
    }


    public Vector3 Velocity;


    // Update is called once per frame
    void FixedUpdate()
    {
        Velocity = GetAverageVelocity();

        Debug.Log("Velocity is: " + Velocity);


        PositionBuffer[index] = transform.position;
        index = (index + 1) % PositionBuffer.Length;
    }

    // private void Update()
    // {
    //      //  Velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
    //      
    //      // Vector3 _inputVelocity_rightController;
    //      // _device_rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out _inputVelocity_rightController);
    //      // Velocity = _inputVelocity_rightController;
    //      Debug.Log("Velocity is: " + Velocity);
    // }

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

    private Vector3 GetAverageVelocity()
    {
        Vector3 averageVelocity = Vector3.zero;

        for (int i = 0; i < PositionBuffer.Length; i++)
        {
            Vector3 vel = (transform.position -
                           PositionBuffer[(index + PositionBuffer.Length - i) % PositionBuffer.Length]) /
                          (Time.fixedDeltaTime * (i + 1));

            averageVelocity += vel / PositionBuffer.Length;
        }

        return averageVelocity;
    }
    
}