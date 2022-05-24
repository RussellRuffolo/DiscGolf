using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchDetector : MonoBehaviour
{

    public MiniGameController mgController;


    private void OnTriggerEnter(Collider other)
    {
   
        mgController.OnScore();
    }
}
