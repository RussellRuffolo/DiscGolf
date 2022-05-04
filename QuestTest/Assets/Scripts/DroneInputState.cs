using UnityEngine;

public class DroneInputState : IInputState
{
    
    public GameObject DroneScreen;

    public GameObject Drone;

    public void ApplyInputs()
    {
        Vector2 rightStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);


        Vector3 droneForward = Drone.transform.forward;
        Vector3 droneRight = Drone.transform.right;

        Drone.transform.position += droneForward * leftStickInput.y;
        Drone.transform.position += droneRight * leftStickInput.x;

        Debug.Log("Right:" + rightStickInput.x + " " + rightStickInput.y + " LeftS: " + leftStickInput.x + " " + leftStickInput.y);


    }

    public InputState CheckInputState()
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