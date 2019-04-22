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
    Debug.Log("entering test state");
}

public void Execute()
{
    Debug.Log("updating test state");
}

public void Exit()
{
    Debug.Log("exiting test state");
}
}

public class Fire : IState
{
    BasePlayer owner;

    public Fire(BasePlayer owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("[State: Fire] Firing weapon");
    }

    public void Execute()
    {
        Debug.Log("[State: Fire] Waiting for projectile to hit");
    }

    public void Exit()
    {
        Debug.Log("[State: Fire] Weapon finished firing");
    }
}
