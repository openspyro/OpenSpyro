using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCameraBehaviour : MonoBehaviour
{
    public SpyroBehaviour target;

    public enum State {followingPlayer, firstPerson}
    private State currentState = State.followingPlayer;

    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();

    private Vector3 targetPos = Vector3.zero;

    //Configuration
    private float followingHeight = 3;
    private float followingMoveSpeed = 10;      //The speed the camera moves at while in following mode.
    private float followingDistance = 10;        //The distance the camera maintains from the target in following mode.

    //Events

    void Awake()
    {
        //Add the state methods.
        stateMethods.Add(State.followingPlayer, WhileFollowingPlayer);

        //Start the targetPos at current pos
        targetPos = transform.position;
    }

    void Update()
    {
        //Finite state machine
        stateMethods[currentState]();
    }



    //Misc methods

    private void MoveToTargetPos(float speed)
    {
        //Moves to the target pos.
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    //State methods

    private void WhileFollowingPlayer()
    {
        //Update the target pos to maintain a constant distance from the player
        Vector3 diff = targetPos - target.transform.position;
        diff.y = 0;
        diff.Normalize();
        diff *= followingDistance;
        diff.y = followingHeight;

        targetPos = target.transform.position + diff;

        //Move towards the target pos
        MoveToTargetPos(followingMoveSpeed);

        //Look at the player
        transform.LookAt(target.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void WhileFirstPerson()
    {
        //TODO: First person controls
    }
}
