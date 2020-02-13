using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAim : MonoBehaviour {

    public float m_AimSpeed = 0.5f;              // How fast the tank changes aiming angle.
    public float m_CurrAngle;
    public UIAimWheel m_AimWheel;

    private string m_AimAxisName;               // The name of the input axis for aiming.
    private Transform m_Turret;              // Reference used to move the turret.
    private float m_AimInputValue;         // The current value of the aiming input.

    private void Awake()
    {
        m_Turret = GetComponent<Transform>().Find("Turret");
        m_CurrAngle = (float)System.Math.Round(m_Turret.GetComponent<Transform>().rotation.eulerAngles.z, 1);
    }

    public void Aim(int direction)
    {
        //Rotate the transform based on the direction
        m_Turret.Rotate(new Vector3(0, 0, (-direction*m_AimSpeed)));
        m_CurrAngle = (float)System.Math.Round(m_Turret.GetComponent<Transform>().rotation.eulerAngles.z, 1);
    }


}
