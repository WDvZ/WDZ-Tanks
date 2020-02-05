using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour, ITankHealth {

    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public float m_CurrHP = 1000f;
    private bool isDead = false;

    public void IGainHealth(float health)
    {
        // WIP
    }

    public void ITakeDamage(float damage)
    {
        m_CurrHP -= damage;
        if (m_CurrHP <= 0)
        {
            isDead = true;
            Debug.Log("Dead");
        }
    }
}
