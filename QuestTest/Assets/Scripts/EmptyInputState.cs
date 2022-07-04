using UnityEngine;

public class EmptyInputState : IInputState
{
    public Vector3 boxHalfExtents;
    public Transform rightHand;

    public PlayerManager playerManager;

    public InputState CheckInputState(InputStruct playerInputs)
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Trigger Down");
            Collider[] colliders = Physics.OverlapBox(rightHand.position, boxHalfExtents);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("disc"))
                {
                    Debug.Log("Disc Grabbed");
                    playerManager.CurrentDisc = collider.gameObject;
                    playerManager.CurrentDisc.transform.parent = null;
                    return InputState.Disc;
                }
            }
            //Disc.position = RightHand.position + Vector3.up * HoverHeight;
            //Disc.rotation = RightHand.rotation;
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            return InputState.Drone;
        }
 
        return InputState.Empty;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void ApplyInputs(InputStruct playerInputs)
    {
       
    }
}