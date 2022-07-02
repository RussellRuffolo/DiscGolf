using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDiscInputState : IInputState
{
    public Vector3 boxHalfExtents;
    public Transform rightHand;

    public PlayerManager playerManager;

    public Bag bag;

    public void ApplyInputs(InputStruct playerInputs)
    {
        Transform discTransform = playerManager.CurrentDisc.transform;
        discTransform.position = playerInputs.rightHandPosition;
        discTransform.rotation = rightHand.rotation;
    }

    public InputState CheckInputState(InputStruct playerInputs)
    {
        if (playerInputs.rightSecTrig)
        {
            return InputState.Disc;
        }

        Collider[] colliders = Physics.OverlapBox(rightHand.position, boxHalfExtents);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("bag"))
            {
                bag.Discs.Add(bag.DiscMapping.IndexOf(playerManager.CurrentDisc));
                GameObject.Destroy(playerManager.CurrentDisc);
                playerManager.CurrentDisc = null;
                return InputState.Empty;
            }
        }
        playerManager.CurrentDisc.GetComponent<MenuDiscLocation>().Reset();
        playerManager.CurrentDisc = null;
        return InputState.Empty;
    }
    public void Enter()
    {
        
    }

    public void Exit()
    {
       
    }
}
