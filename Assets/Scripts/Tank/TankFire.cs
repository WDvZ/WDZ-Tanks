using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFire : MonoBehaviour {

    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public float m_PrevPower = 2f;
    public float m_CurrPower = 2f;
    public float m_MaxPower = 1000f;
    public float m_PowerSpeed = 10f;           // How fast the tank changes shot power.

    public GameObject m_Bullet;
    public Transform m_BulletSpawnPoint;

    //private int prevWeaponIndex = 1;
    //private int currWeaponIndex;

    private string m_FireButton;                // The input axis that is used for launching shells.
    private string m_PowerAxisName;             // The name of the input axis for adjusting power.
    private float m_PowerInputValue;             // The current value of the power input.
    private Transform m_Turret;              // Reference used to move the turret.
    public bool m_Fired;                       // Whether or not the shell has been launched.

    private void Awake()
    {
        m_Turret = GetComponent<Transform>().Find("Turret");
    }

    private void OnEnable()
    {
        // Also reset the input values.
        m_PowerInputValue = m_PrevPower;
        m_Fired = false;
    }

  // Use this for initialization
    void Start () {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;
        m_PowerAxisName = "Vertical" + m_PlayerNumber;
    }


    // Update is called once per frame
    void Update()
    {
        m_PowerInputValue = Input.GetAxis(m_PowerAxisName);
        // Adjust angle and power
        if (m_PowerInputValue < 0)
        {
            m_PowerInputValue = -1;
        }
        else if (m_PowerInputValue > 0)
        {
            m_PowerInputValue = 1;
        }
        m_CurrPower += m_PowerInputValue;

    }
    
    void FixedUpdate()
    {
        Power();
    }

    private void Power()
    {
        // Reserved to set power of shot
        // If the max force has been exceeded
        if (m_CurrPower >= m_MaxPower)
        {
            // ... use the max force.
            m_CurrPower = m_MaxPower;
        }
    }

    public void Fire()
    {
        if (m_Fired) { return; }
        m_Fired = true;
        m_PrevPower = m_CurrPower;
        GetComponent<WeaponHolder>().ShootBullet(m_CurrPower, m_BulletSpawnPoint);
    }

}
