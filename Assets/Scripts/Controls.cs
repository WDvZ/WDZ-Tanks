using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    private TurretMovement turret;

    private void Start()
    {
        turret = FindObjectOfType<TurretMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turret.counterClockWise = true;
        }
        else
        {
            turret.counterClockWise = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            turret.clockWise = true;
        }
        else
        {
            turret.clockWise = false;
        }

    }

}
