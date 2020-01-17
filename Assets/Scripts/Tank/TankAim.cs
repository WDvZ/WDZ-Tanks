using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAim : MonoBehaviour {

    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_AimSpeed = 0.5f;              // How fast the tank changes aiming angle.
    public float m_PrevAngle = 60f;
    public float m_CurrAngle;

    private string m_AimAxisName;               // The name of the input axis for aiming.
    private Transform m_Turret;              // Reference used to move the turret.
    private float m_AimInputValue;         // The current value of the aiming input.

    private void Awake()
    {
        m_Turret = GetComponent<Transform>().Find("Turret");
    }

    private void OnEnable()
    {
        // Also reset the input values.
        m_AimInputValue = 0f;
    }
    private void OnDisable()
    {
        // Not needed, unless we want to force stop the tanks.
    }

    // Use this for initialization
    void Start () {
        // The axes names are based on player number.
        m_AimAxisName = "Horizontal" + m_PlayerNumber;
    }

    // Update is called once per frame
    void Update () {
        // Store the value of both input axes.
        m_AimInputValue = Input.GetAxis(m_AimAxisName);

        // For aiming: Currently it jumps quite a few degrees each time. Maybe assign a fine-tune button as well
    }

    void FixedUpdate()
    {
        // Adjust angle and power
        if (m_AimInputValue < 0)
        {
            m_AimInputValue = -1;
            Aim();
        }
        else if (m_AimInputValue > 0)
        {
            m_AimInputValue = 1;
            Aim();
        }
    }

    private void Aim()
    {
        // Can perhaps store each keypress as a 0.5 degree increment/decrement and corouting the aim to move toward that angle
        //m_Turret.Rotate(new Vector3(0, 0, -m_AimInputValue));

        //Rotate the transform based on the rotationInput
        m_Turret.Rotate(new Vector3(0, 0, -m_AimInputValue));
        //Back up the rotation BEFORE we clamp it.
        Quaternion actualRotation = m_Turret.rotation;
        //Pull the current euler angle of our rotation
        var currentEulerRotation = m_Turret.rotation.eulerAngles;
        //Clamp it by taking the current euler rotation and subtracting any remainder when we divide each angle by rotationClamp
        m_Turret.rotation = Quaternion.Euler(currentEulerRotation - new Vector3(0, 0, currentEulerRotation.z % m_AimSpeed));
        //var angle : float = Mathf.MoveTowardsAngle(angleA, angleB, turretSpeed * Time.deltaTime);
        //transform.eulerAngles = Vector3(0, angle, 0);
    }


}
