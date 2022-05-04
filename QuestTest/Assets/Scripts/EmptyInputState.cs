using UnityEngine;

public class EmptyInputState : IInputState
{
    public Vector3 boxHalfExtents;
    public Transform rightHand;

    public PlayerManager playerManager;

    public InputState CheckInputState()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Collider[] colliders = Physics.OverlapBox(rightHand.position, boxHalfExtents);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("disc"))
                {
                    playerManager.CurrentDisc = collider.gameObject;
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

    public void ApplyInputs()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && playerManager.CurrentDisc != null)
        {
            playerManager.CurrentDisc.transform.position = rightHand.position;
            playerManager.CurrentDisc.GetComponent<DiscController>().OnGrab();
        }
    }
}