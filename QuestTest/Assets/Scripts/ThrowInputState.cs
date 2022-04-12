using UnityEngine;
using UnityEngine.UI;

public class ThrowInputState : IInputState
{
    public DiscController Disc;
    
    public Slider SpeedSlider;

    public Transform RightHand;
    
    public InputState CheckInputState()
    {
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
        Disc.Throw(SpeedSlider.value, RightHand.forward);
    }
}