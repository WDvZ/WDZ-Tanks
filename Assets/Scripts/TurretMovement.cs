using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour {

    public HingeJoint2D hingeTurret;
    public bool counterClockWise;
    public bool clockWise;
    public float rotateSpeed = 1f;

    private HingeJoint2D jointRef;
    private JointMotor2D hingeTurretMotor;
    private Vector3 turretAngles;

    // Use this for initialization
    void Start () {
        hingeTurret = GetComponent<HingeJoint2D>();
        hingeTurretMotor = hingeTurret.motor;
	}
	
	// Update is called once per frame
	void Update () {

        if (counterClockWise) {
            hingeTurretMotor.motorSpeed = rotateSpeed;
        }
        if (clockWise)
        {
            hingeTurretMotor.motorSpeed = -rotateSpeed;
        }
        if (!(clockWise || counterClockWise)) {
            hingeTurretMotor.motorSpeed = 0;

            // Make sure the angle gets rounded to the nearest integer degree
            // Potentially need to use Mathf.LerpAngle to make smooth rotations
            turretAngles = hingeTurret.transform.eulerAngles;
            turretAngles.x = Mathf.Round(turretAngles.x);
            turretAngles.y = Mathf.Round(turretAngles.y);
            turretAngles.z = Mathf.Round(turretAngles.z);
            hingeTurret.transform.eulerAngles = turretAngles;
        }

        hingeTurret.motor = hingeTurretMotor;

    }
}
