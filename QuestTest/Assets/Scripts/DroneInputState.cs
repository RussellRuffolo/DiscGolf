using UnityEngine;

public class DroneInputState : IInputState
{
    
    public GameObject DroneScreen;

    public GameObject Drone;

    public void ApplyInputs(InputStruct playerInputs)
    {
     


        Vector3 droneForward = Drone.transform.forward;
        Vector3 droneRight = Drone.transform.right;

        Drone.transform.position += droneForward * playerInputs.leftStickInput.y;
        Drone.transform.position += droneRight * playerInputs.leftStickInput.x;

        Debug.Log("Right:" + playerInputs.rightStickInput.x + " " + playerInputs.rightStickInput.y + " LeftS: " + playerInputs.leftStickInput.x + " " + playerInputs.leftStickInput.y);


    }

    public InputState CheckInputState(InputStruct playerInputs)
    {

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            return InputState.Empty;
        }


        return InputState.Drone;
    }

    public void Enter()
    {
        DroneScreen.SetActive(true);
    }

    public void Exit()
    {
        DroneScreen.SetActive(false);
    }
}