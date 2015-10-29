using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
public class SpyroBehaviour : MonoBehaviour
{
    public Camera myCamera;

    public enum State
    {
        walking,
        charging,
        gliding,
        hovering,
        looking
    }
    private State currentState = State.walking;
    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();

    private CharacterMotor motor;

    //private Vector3 leftStick;

    //Events

    void Awake()
    {
        //Get the motor
        motor = GetComponent<CharacterMotor>();

        //TEMPORARY: Get the camera
        myCamera = Camera.main;

        //Set up the state machine
        stateMethods.Add(State.walking, WhileWalking);
        stateMethods.Add(State.charging, WhileCharging);
        stateMethods.Add(State.gliding, WhileGliding);
        stateMethods.Add(State.hovering, WhileHovering);
        stateMethods.Add(State.looking, WhileLooking);
    }

    void Update()
    {
        //State machine
        stateMethods[currentState]();
    }


    //Misc methods

    private Vector3 GetLeftStick()
    {
        //Returns the rotation the left analog stick is pointing Spyro in, adjusted for the camera angle.

        //Temporarily level the camera
        Vector3 camRot = myCamera.transform.eulerAngles;
        myCamera.transform.eulerAngles = new Vector3(0, camRot.y, 0);

        //Calculate the rotated stick vector
        Vector3 xComponent = Input.GetAxis("LeftStick_x") * myCamera.transform.right;
        Vector3 zComponent = Input.GetAxis("LeftStick_y") * myCamera.transform.forward;
        Vector3 leftStick = xComponent + zComponent;

        //Restore the camera's rotation
        myCamera.transform.eulerAngles = camRot;

        //Return the stick vector
        return leftStick;
    }

    //State methods

    private void WhileWalking()
    {
        //Send the input to the motor
        Vector3 leftStick = GetLeftStick();

        if (leftStick.magnitude > 0.01f)
        {
            transform.forward = leftStick;
        }
    }

    private void WhileCharging()
    {
    }

    private void WhileGliding()
    {
    }

    private void WhileHovering()
    {
    }

    private void WhileLooking()
    {
    }
}
