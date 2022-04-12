using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    private bool flying = false;

    private Vector3 currentVelocity;

    public float maxSpeed;

    private Rigidbody rb;

    public float gravAcceleration;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(float speedMod, Vector3 direction)
    {
        currentVelocity = speedMod * maxSpeed * direction;
        flying = true;
    }

    public void OnGrab()
    {
        flying = false;
        currentVelocity = Vector3.zero;
    }
  
    private void Update()
    {
        if (flying)
        {
       //     currentVelocity -= gravAcceleration * Vector3.up * Time.deltaTime;
            rb.MovePosition(transform.position + currentVelocity * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            flying = false;
        }
    }
}