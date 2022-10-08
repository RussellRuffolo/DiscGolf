  using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGrabber : MonoBehaviour
{

    public BoxCollider Collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("tr Stay");
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            transform.position = other.gameObject.transform.position;
            transform.rotation = other.gameObject.transform.rotation;

            
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log("Collision Stay");
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            transform.position = collisionInfo.gameObject.transform.position;
            transform.rotation = collisionInfo.gameObject.transform.rotation;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
