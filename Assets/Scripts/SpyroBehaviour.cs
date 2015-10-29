using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
public class SpyroBehaviour : MonoBehaviour
{
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


    //Events

    void Awake()
    {
        //Get the motor
        motor = GetComponent<CharacterMotor>();

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



    //State methods

    private void WhileWalking()
    {
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
