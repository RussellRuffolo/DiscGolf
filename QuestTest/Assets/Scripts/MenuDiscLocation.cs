using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDiscLocation : MonoBehaviour
{
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;    
    }

    public void Reset()
    {
        transform.position = startPosition;    
    }
}
