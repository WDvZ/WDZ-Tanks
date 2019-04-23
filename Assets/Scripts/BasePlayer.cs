using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public Transform turret;

    public float baseHP = 100f;
    public float currHP;

    public float prevPower = 2f;
    public float currPower = 2f;

    public float prevAngle = 60f;
    public float currAngle;

    public int prevWeaponIndex = 1;
    public int currWeaponIndex;

    public float rotationClamp = 0.5f;

    public Quaternion actualRotation;

    public StateMachine stateMachine = new StateMachine();

    public GameObject bullet;
    public Transform spawnPoint;

    void Start()
    {
        stateMachine.ChangeState(new Aiming(this));
        actualRotation = turret.rotation;
    }
    
    void Update()
    {

        stateMachine.Update();
    }

    public void Shoot()
    {
        // Instantiate the projectile rotated the correct way
        GameObject newBullet = GameObject.Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        newBullet.GetComponent<BaseBullet>().ShootBullet(currPower);
    }
}
