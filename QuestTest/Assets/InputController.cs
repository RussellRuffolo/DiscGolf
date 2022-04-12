using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public Transform RightHand;

    public Transform Disc;

    public Canvas RightHandCanvas;

    public Slider SpeedSlider;

    private Vector2 rightStickInput;
    public Vector3 BoxHalfExtents;
    public float slideScale;

    public InputState CurrentInputState;

    public DiscController DiscController;

    public Dictionary<InputState, IInputState> InputStates = new Dictionary<InputState, IInputState>()
    {
        {InputState.Empty, new EmptyInputState()},
        {InputState.Disc, new DiscInputState()},
        {InputState.Throw, new ThrowInputState()}
    };

    private void Start()
    {
        CurrentInputState = InputState.Empty;

        RightHandCanvas.enabled = false;

        EmptyInputState emptyInputState = (EmptyInputState) InputStates[InputState.Empty];
        emptyInputState.rightHand = RightHand;
        emptyInputState.boxHalfExtents = BoxHalfExtents;
        emptyInputState.disc = Disc;

        DiscInputState discInputState = (DiscInputState) InputStates[InputState.Disc];
        discInputState.DiscTransform = Disc;
        discInputState.RightHandTransform = RightHand;
        discInputState.RightHandCanvas = RightHandCanvas;
        discInputState.slideScale = slideScale;
        discInputState.SpeedSlider = SpeedSlider;

        ThrowInputState throwInputState = (ThrowInputState) InputStates[InputState.Throw];
        throwInputState.Disc = DiscController;
        throwInputState.SpeedSlider = SpeedSlider;
        throwInputState.RightHand = RightHand;
    }


    // Update is called once per frame
    void Update()
    {
        InputState newState = InputStates[CurrentInputState].CheckInputState();
        if (newState != CurrentInputState)
        {
            InputStates[CurrentInputState].Exit();
            CurrentInputState = newState;
            InputStates[CurrentInputState].Enter();
        }

        InputStates[CurrentInputState].ApplyInputs();
    }
}