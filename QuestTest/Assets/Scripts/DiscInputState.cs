using UnityEngine;
using UnityEngine.UI;

public class DiscInputState : IInputState
{
    public Canvas RightHandCanvas;

    public Transform RightHandTransform;

    public PlayerManager playerManager;     

    public Slider SpeedSlider;

    public float slideScale;

    public InputState CheckInputState()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            return InputState.Disc;
        }

        return InputState.Throw;
    }

    public void Enter()
    {
        RightHandCanvas.enabled = true;
    }

    public void Exit()
    {
        RightHandCanvas.enabled = false;
    }

    public void ApplyInputs()
    {
        Transform discTransform = playerManager.CurrentDisc.transform;
        discTransform.position = RightHandTransform.position;
        discTransform.rotation = RightHandTransform.rotation;

        Vector2 rightStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (rightStickInput != Vector2.zero)
        {
            SpeedSlider.value += rightStickInput.y * slideScale;
        }
    }
}