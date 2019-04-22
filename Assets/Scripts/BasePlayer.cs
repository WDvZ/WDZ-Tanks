using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public float baseHP = 100;
    public float currHP;

    public float prevPower = 500;
    public float currPower;

    public float prevAngle = 60;
    public float currAngle;

    public int prevWeaponIndex = 1;
    public int currWeaponIndex;

    StateMachine stateMachine = new StateMachine();

    void Start()
    {
        stateMachine.ChangeState(new Aiming(this));
    }

    void Update()
    {

        stateMachine.Update();
    }
}
