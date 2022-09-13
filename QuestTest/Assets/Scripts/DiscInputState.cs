using UnityEngine;
using UnityEngine.UI;

public class DiscInputState : IInputState
{
    public Canvas RightHandCanvas;

    public Transform RightHandTransform;

    public PlayerManager playerManager;     

    public Slider SpeedSlider;

    public Slider SpinSlider;

    public float slideScale;

    public InputState CheckInputState(InputStruct playerInputs)
    {
        if (playerInputs.rightSecTrig)
        {
            return InputState.Disc;
        }

        return InputState.Throw;
    }

    public void Enter()
    {
        lastPosition = RightHandTransform.position;
        RightHandCanvas.enabled = true;
    }

    public void Exit()
    {
        RightHandCanvas.enabled = false;
    }

    private Vector3 lastPosition;
    private Vector3 velocity;
    public void ApplyInputs(InputStruct playerInputs)
    {

        velocity = RightHandTransform.position - lastPosition;
        
        Transform discTransform = playerManager.CurrentDisc.transform;
        discTransform.position = playerInputs.rightHandPosition;
        discTransform.rotation = RightHandTransform.rotation;

        Vector2 rightStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (rightStickInput != Vector2.zero)
        {
            SpeedSlider.value += rightStickInput.y * slideScale;

            SpinSlider.value += rightStickInput.x * slideScale;
        }
        
        lastPosition = RightHandTransform.position;

    }
}