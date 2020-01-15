using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine
{
    IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}

public class Aiming : IState
{
    BasePlayer owner;

public Aiming(BasePlayer owner) { this.owner = owner; }

public void Enter()
{
    Debug.Log("[State: Aiming] Starting to Aim: " + owner.playerName);
}

public void Execute()
{
    //Debug.Log("[State: Aiming] Aiming");

        // Turret Angle (https://www.reddit.com/r/Unity3D/comments/7os3vt/how_to_snap_quaternion_rotation/)
        //Set our current rotation to actualRotation to restore any lost values (less than rotationClamp)
        owner.turret.rotation = owner.actualRotation;
        //Get a rotation on the Z axis between -1 and 1. Times -1 so Right is clockwise.
        var rotationInput = new Vector3(0f, 0f, -Input.GetAxis("Horizontal"));
        //Rotate the transform based on the rotationInput
        owner.turret.Rotate(rotationInput);
        //Back up the rotation BEFORE we clamp it.
        owner.actualRotation = owner.turret.rotation;
        //Pull the current euler angle of our rotation
        var currentEulerRotation = owner.turret.rotation.eulerAngles;
        //Clamp it by taking the current euler rotation and subtracting any remainder when we divide each angle by rotationClamp
        owner.turret.rotation = Quaternion.Euler(currentEulerRotation - new Vector3(currentEulerRotation.x % owner.rotationClamp, currentEulerRotation.y % owner.rotationClamp, currentEulerRotation.z % owner.rotationClamp));


        if (Input.GetButtonDown("Fire1"))
        {
            owner.stateMachine.ChangeState(new Fire(owner));
        }
    }

    public void Exit()
{
    Debug.Log("[State: Aiming] Finished aiming: " + owner.playerName);
}
}

public class Fire : IState
{
    BasePlayer owner;

    public Fire(BasePlayer owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("[State: Fire] Firing weapon: " + owner.playerName);
        owner.Shoot(owner);
    }

    public void Execute()
    {
        // Check if we're done firing, then go to the next state
        //Debug.Log("[State: Fire] Waiting for projectile to hit");
        if (owner.doneFiring)
        {
            owner.stateMachine.ChangeState(new Waiting(owner));
            
        }

    }

    public void Exit()
    {
        Debug.Log("[State: Fire] Weapon finished firing: " + owner.playerName);
    }
}

public class Waiting : IState
{
    BasePlayer owner;

    public Waiting(BasePlayer owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("[State: Waiting] Waiting for turn: " + owner.playerName);
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        Debug.Log("[State: Waiting] Starting turn: " + owner.playerName);
    }
}
