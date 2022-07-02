using System.Collections.Generic;

public class MenuInputController: BasePlayerController
{
    public Bag Bag;
    private void Start()
    {
        InputStates = new Dictionary<InputState, IInputState>()
    {
        {InputState.Empty, new EmptyInputState()},
        {InputState.Disc, new MenuDiscInputState()},
        {InputState.Drone, new EmptyInputState() }
    };
        CurrentInputState = InputState.Empty;

        EmptyInputState emptyInputState = (EmptyInputState)InputStates[InputState.Empty];
        emptyInputState.rightHand = RightHand;
        emptyInputState.boxHalfExtents = BoxHalfExtents;
        emptyInputState.playerManager = playerManager;

        MenuDiscInputState discInputState = (MenuDiscInputState)InputStates[InputState.Disc];
        discInputState.playerManager = playerManager;
        discInputState.rightHand = RightHand;
        discInputState.boxHalfExtents = BoxHalfExtents;
        discInputState.bag = Bag;
    }
}