using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFire : MonoBehaviour {

    public float m_PrevPower = 2f;
    public float m_CurrPower = 2f;
    public float m_MaxPower = 1000f;

    public GameObject m_Bullet;
    public Transform m_BulletSpawnPoint;

    //private int prevWeaponIndex = 1;
    //private int currWeaponIndex;

    public bool m_Fired;                       // Whether or not the shell has been launched.

    private void OnEnable()
    {
        m_Fired = false;
    }

    public float Power
    {
        get
        {
            return m_CurrPower;
        }
        set
        {
            m_CurrPower = value;
            // If the max force has been exceeded
            if (m_CurrPower >= m_MaxPower)
            {
                // ... use the max force.
                m_CurrPower = m_MaxPower;
            }
        }
    }
    
    public void Fire()
    {
        if (m_Fired) { return; }
        m_Fired = true;
        m_PrevPower = m_CurrPower;
        WeaponHolder weapon = GetComponent<WeaponHolder>();
        weapon.ShootBullet(m_CurrPower, m_BulletSpawnPoint);
    }

}
