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
    private float followingMoveSpeed = 10;          //The speed the camera moves at while in following mode.
    private float followingDistance = 15;           //The distance the camera maintains from the target in following mode.
    private float followingOrbitSpeed = 360f;       //The speed the camera rotates with the right analog stick.

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

    private void MaintainDistance(float height, float distance)
    {
        //Updates the target pos to maintain a constance distance from the player.

        Vector3 diff = targetPos - target.transform.position;
        diff.y = 0;
        diff.Normalize();
        diff *= distance;
        diff.y = height;

        targetPos = target.transform.position + diff;
    }

    private void OrbitWithStick()
    {
        //Rotates the camera with the stick

        Vector3 diff = targetPos - target.transform.position;
        diff = RotatePoint(diff, Input.GetAxis("RightStick_x") * followingOrbitSpeed * Time.deltaTime);

        targetPos = target.transform.position + diff;
    }

    private Vector3 RotatePoint(Vector3 v, float degrees)
    {
        //Rotates a 3D point around the origin, on the xz plane.
        //Code taken and modified from http://answers.unity3d.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
        //Credit to DDP.

        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) - (sin * tz);
        v.z = (sin * tx) + (cos * tz);

        return v;
    }

    //State methods

    private void WhileFollowingPlayer()
    {

        //Orbit
        OrbitWithStick();

        //Update the target pos to maintain a constant distance from the player
        MaintainDistance(followingHeight, followingDistance);

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
