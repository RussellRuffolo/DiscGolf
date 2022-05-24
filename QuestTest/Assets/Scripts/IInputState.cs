using System.Collections;
using System.Collections.Generic;

public interface IInputState
{
    InputState CheckInputState(InputStruct playerInputs);
    
    void Enter();

    void Exit();

    void ApplyInputs(InputStruct playerInputs);

}