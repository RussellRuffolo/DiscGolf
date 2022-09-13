using UnityEngine;
using UnityEngine.UI;

public class ThrowInputState : IInputState
{
    
    public Slider SpeedSlider;
    public Slider SpinSlider;
    public Transform RightHand;

    public PlayerManager playerManager;

    
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
        Debug.Log("Playermanager disc: " + playerManager.CurrentDisc);
        playerManager.CurrentDisc.GetComponent<DiscController>().Throw(SpeedSlider.value, SpinSlider.value, RightHand.transform.forward);
       // playerManager.CurrentDisc = null;
    }
}