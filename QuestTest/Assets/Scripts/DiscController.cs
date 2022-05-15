using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    private bool flying = false;

    private Vector3 currentVelocity;
    private float rotationalVelocity;

    public float maxSpeed;
    public float maxRotation;


    public Vector3 halfExtents;
    public float gravAcceleration;
    public float liftModifier;
    public float dragModifier;
    public float spinDrag;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public MiniGameController mgController;

    public void Awake()
    {
        mgController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }


    public void Throw(float speedMod, Vector3 direction)
    {
        currentVelocity = speedMod * direction;
        rotationalVelocity = speedMod * maxRotation;
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
            currentVelocity -= gravAcceleration * Vector3.up * Time.deltaTime;


            currentVelocity += Vector3.up * Time.deltaTime * currentVelocity.magnitude * liftModifier;

            //this should be proportional to surface area of disc cross section in velocity direction
            //     currentVelocity -=  currentVelocity.normalized * currentVelocity.sqrMagnitude * Time.deltaTime * dragModifier;


            //rotation should asymptotically decrease. 

            if (rotationalVelocity > 0)
            {
                rotationalVelocity -= spinDrag * Time.deltaTime;
            }
            else
            {
                rotationalVelocity += spinDrag * Time.deltaTime;
            }


#pragma warning disable CS0618 // Type or member is obsolete
            //     transform.RotateAroundLocal(Vector3.up, rotationalVelocity * Time.deltaTime);
#pragma warning restore CS0618 // Type or member is obsolete

            Vector3 movementVector = currentVelocity * Time.deltaTime;

            RaycastHit[] results = UnityEngine.Physics.BoxCastAll(transform.position, halfExtents, movementVector,
                transform.rotation, movementVector.magnitude);

            foreach (RaycastHit hit in results)
            {
                if (hit.transform.gameObject.CompareTag("ground"))
                {
                    mgController.OnMiss();
                    StartCoroutine(DiscDelay());
                    flying = false;
                }
            }


            transform.position += currentVelocity * Time.deltaTime;
        }
    }

    IEnumerator DiscDelay()
    {
        yield return new WaitForSeconds(1);
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}