using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
public class SpyroBehaviour : MonoBehaviour
{
    public Camera myCamera;

    private float walkRotSpeed = 720f;
    private float walkSpeed = 5f;

    private float chargeRotSpeed = 360f;
    private float glideRotSpeed = 360f;

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

    private Vector3 leftStick;

    private CharacterMotor motor;

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

    private void UpdateLeftStick()
    {
        //Updates the left stick vector, adjusted for the camera angle.

        //Temporarily level the camera
        Vector3 camRot = myCamera.transform.eulerAngles;
        myCamera.transform.eulerAngles = new Vector3(0, camRot.y, 0);

        //Calculate the rotated stick vector
        Vector3 xComponent = Input.GetAxis("LeftStick_x") * myCamera.transform.right;
        Vector3 zComponent = Input.GetAxis("LeftStick_y") * myCamera.transform.forward;
        leftStick = xComponent + zComponent;

        //Restore the camera's rotation
        myCamera.transform.eulerAngles = camRot;
    }

    private void RotateWithStick(float rotSpeed)
    {
        //Rotates Spyro with a given speed towards the direciton pointed to by the left analog stick
        if (leftStick.sqrMagnitude > 0.001)
        {
            Quaternion targetRot = Quaternion.LookRotation(leftStick);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
        }
    }

    //State methods

    private void WhileWalking()
    {
        UpdateLeftStick();
        RotateWithStick(walkRotSpeed);

        //Configure the motor
        motor.movement.maxForwardSpeed = walkSpeed;
        motor.movement.maxSidewaysSpeed = walkSpeed;

        //Send input to the motor
        motor.inputMoveDirection = transform.forward * leftStick.magnitude;
        motor.inputJump = Input.GetButton("Jump");
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
