using UnityEngine;

public class EmptyInputState : IInputState
{
    public Vector3 boxHalfExtents;
    public Transform rightHand;
    public Transform disc;
    public DiscController DiscController;

    public InputState CheckInputState()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Collider[] colliders = Physics.OverlapBox(rightHand.position, boxHalfExtents);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("disc"))
                {
                    return InputState.Disc;
                }
            }
            //Disc.position = RightHand.position + Vector3.up * HoverHeight;
            //Disc.rotation = RightHand.rotation;
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
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            disc.position = rightHand.position;
            DiscController.OnGrab();
        }
    }
}