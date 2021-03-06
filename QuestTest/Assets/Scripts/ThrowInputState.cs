using UnityEngine;
using UnityEngine.UI;

public class ThrowInputState : IInputState
{
    
    public Slider SpeedSlider;

    public Transform RightHand;

    public PlayerManager playerManager;

    public VelocityTracker VelocityTracker;
    
    public InputState CheckInputState(InputStruct playerInputs)
    {
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
        playerManager.CurrentDisc.GetComponent<DiscController>().Throw(VelocityTracker.Velocity.magnitude, VelocityTracker.Velocity.normalized);
       // playerManager.CurrentDisc = null;
    }
}