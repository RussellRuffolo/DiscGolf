using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscUIInputState : IInputState
{
    public Vector3 boxHalfExtents;
    public Transform rightHand;
    public GameObject centerEye;
    public PlayerManager playerManager;
    public InputState CheckInputState(InputStruct playerInputs)
    {
        // if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        // {
        //     Debug.Log("Trigger Down");
        //     Collider[] colliders = Physics.OverlapBox(rightHand.position, boxHalfExtents);
        //     foreach (Collider collider in colliders)
        //     {
        //         if (collider.CompareTag("disc") && playerManager.CurrentDisc == collider.gameObject)
        //         {
        //             return InputState.Disc;
        //         }
        //     }
        //     //Disc.position = RightHand.position + Vector3.up * HoverHeight;
        //     //Disc.rotation = RightHand.rotation;
        // }
        
        if (playerInputs.rightBut1)
        {
            return InputState.Disc;
        }

        return InputState.DiscUI;
    }

    public void Enter()
    {
        playerManager.CurrentDisc.GetComponent<DiscController>().ShowCanvas(centerEye);
    }

    public void Exit()
    {
        playerManager.CurrentDisc.GetComponent<DiscController>().HideCanvas();
    }

    public void ApplyInputs(InputStruct playerInputs)
    {
    }
}
