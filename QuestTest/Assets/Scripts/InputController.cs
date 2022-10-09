using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePlayerController : MonoBehaviour
{
    public Transform RightHand;

    public Dictionary<InputState, IInputState> InputStates;
    public Canvas RightHandCanvas;

    public Slider SpeedSlider;
    public Slider SpinSlider;

    private Vector2 rightStickInput;
    public Vector3 BoxHalfExtents;
    public float slideScale;

    public InputState CurrentInputState;

    public PlayerManager playerManager;

    public GameObject DroneScreen;
    public GameObject Drone;
    public int tickNumber;

    void FixedUpdate()
    {
        tickNumber++;

        //get inputs to input struct
        InputStruct newInputs = new InputStruct()
        {
            rightSecTrig = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger),
            rightBut1 = OVRInput.GetDown(OVRInput.Button.One),
            rightBut2 = OVRInput.GetDown(OVRInput.Button.Two),
            rightBut3 = OVRInput.GetDown(OVRInput.Button.Three),
            rightStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick),
            leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick),
            rightHandPosition = RightHand.position,
            rightHandRotation = RightHand.rotation,
        };

        //record inputs
        //ReplayRecorder.Instance.RecordInput(tickNumber, newInputs);


        InputState newState = InputStates[CurrentInputState].CheckInputState(newInputs);
        if (newState != CurrentInputState)
        {
            InputStates[CurrentInputState].Exit();
            CurrentInputState = newState;
            InputStates[CurrentInputState].Enter();
        }


        //apply inputs takes the inputs for this frame
        InputStates[CurrentInputState].ApplyInputs(newInputs);
    }
}

public class InputController : BasePlayerController
{
    public GameObject centerEye;
    private void Start()
    {
        InputStates = new Dictionary<InputState, IInputState>()
        {
            {InputState.Empty, new EmptyInputState()},
            {InputState.Disc, new DiscInputState()},
            {InputState.Throw, new ThrowInputState()},
            {InputState.Drone, new DroneInputState()},
            {InputState.DiscUI, new DiscUIInputState()}
        };
        CurrentInputState = InputState.Empty;

        RightHandCanvas.enabled = false;

        EmptyInputState emptyInputState = (EmptyInputState) InputStates[InputState.Empty];
        emptyInputState.rightHand = RightHand;
        emptyInputState.boxHalfExtents = BoxHalfExtents;
        emptyInputState.playerManager = playerManager;

        DiscUIInputState discUIInputState = (DiscUIInputState) InputStates[InputState.DiscUI];
        discUIInputState.rightHand = RightHand;
        discUIInputState.boxHalfExtents = BoxHalfExtents;
        discUIInputState.playerManager = playerManager;
        discUIInputState.centerEye = centerEye;
        
        DiscInputState discInputState = (DiscInputState) InputStates[InputState.Disc];
        discInputState.playerManager = playerManager;
        discInputState.RightHandTransform = RightHand;
        discInputState.RightHandCanvas = RightHandCanvas;
        discInputState.slideScale = slideScale;
        discInputState.SpeedSlider = SpeedSlider;
        discInputState.SpinSlider = SpinSlider;

        ThrowInputState throwInputState = (ThrowInputState) InputStates[InputState.Throw];
        throwInputState.playerManager = playerManager;
        throwInputState.SpeedSlider = SpeedSlider;
        throwInputState.SpinSlider = SpinSlider;
        throwInputState.RightHand = RightHand;

        DroneInputState droneInputState = (DroneInputState) InputStates[InputState.Drone];
        droneInputState.DroneScreen = DroneScreen;
        droneInputState.Drone = Drone;
    }
}