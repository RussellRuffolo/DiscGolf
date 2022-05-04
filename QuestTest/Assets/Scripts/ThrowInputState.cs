using UnityEngine;
using UnityEngine.UI;

public class ThrowInputState : IInputState
{
    
    public Slider SpeedSlider;

    public Transform RightHand;

    public PlayerManager playerManager;
    
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
        playerManager.CurrentDisc.GetComponent<DiscController>().Throw(SpeedSlider.value, RightHand.forward);
       // playerManager.CurrentDisc = null;
    }
}