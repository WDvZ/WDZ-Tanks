using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public Transform turret;

    public string playerName;
    public float baseHP = 100f;
    public float currHP;

    public float prevPower = 2f;
    public float currPower = 2f;

    public float prevAngle = 60f;
    public float currAngle;

    public int prevWeaponIndex = 1;
    public int currWeaponIndex;

    public float rotationClamp = 0.5f;

    public bool doneFiring = false;

    public Quaternion actualRotation;

    public StateMachine stateMachine = new StateMachine();

    public GameObject bullet;
    public Transform spawnPoint;

    void Start()
    {
        actualRotation = turret.rotation;
    }
    
    void Update()
    {

        stateMachine.Update();
    }

    public void Shoot(BasePlayer basePlayer)
    {
        doneFiring = false;
        // Instantiate the projectile rotated the correct way
        GameObject newBullet = GameObject.Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        newBullet.GetComponent<BaseBullet>().myOwner = basePlayer;
        newBullet.GetComponent<BaseBullet>().ShootBullet(currPower);
    }
}
