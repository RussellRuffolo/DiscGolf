using System.Collections;
using System.Collections.Generic;

public interface IInputState
{
    InputState CheckInputState();
    
    void Enter();

    void Exit();

    void ApplyInputs();

}