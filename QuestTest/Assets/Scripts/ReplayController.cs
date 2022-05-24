using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : BasePlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int tickNumber;

    // Update is called once per frame
    void FixedUpdate()
    {
        tickNumber++;
        InputStruct playerInputs = ReplayRecorder.Instance.GetInput(tickNumber);
        
        InputState newState = InputStates[CurrentInputState].CheckInputState(playerInputs);
        if (newState != CurrentInputState)
        {
            InputStates[CurrentInputState].Exit();
            CurrentInputState = newState;
            InputStates[CurrentInputState].Enter();
        }


        //apply inputs takes the inputs for this frame
        InputStates[CurrentInputState].ApplyInputs(playerInputs);
        
    }
}
