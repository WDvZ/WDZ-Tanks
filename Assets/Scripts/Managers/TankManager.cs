using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TankManager {

    // This class is to manage various settings on a tank.
    // It works with the GameManager class to control how the tanks behave
    // and whether or not players have control of their tank in the 
    // different phases of the game.

    public Color m_PlayerColor;                             // This is the color this tank will be tinted.
    public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
    [HideInInspector] public int m_PlayerNumber;            // Player number
    [HideInInspector] public string m_ColoredPlayerText;    // Player text and HTML colour
    [HideInInspector] public GameObject m_Instance;         // Reference to instance of tank
    [HideInInspector] public int m_Wins;                    // Number of wins

    private TankAim m_Aim;                        // Reference to tank's movement script, used to disable and enable control.
    private TankFire m_Fire;                        // Reference to tank's shooting script, used to disable and enable control.
    private TankHealth m_Health;
    private WeaponHolder m_Weapon;          // Reference to the tank's weapon holder
    //private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    public void Setup()
    {
        // Get references to the components.
        m_Aim = m_Instance.GetComponent<TankAim>();
        m_Fire = m_Instance.GetComponent<TankFire>();
        m_Health = m_Instance.GetComponent<TankHealth>();
        m_Weapon = m_Instance.GetComponent<WeaponHolder>();

        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        // Set the player numbers to be consistent across the scripts.
        m_Aim.m_PlayerNumber = m_PlayerNumber;
        m_Fire.m_PlayerNumber = m_PlayerNumber;
        m_Health.m_PlayerNumber = m_PlayerNumber;

        // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        // Get all of the renderers of the tank.
        SpriteRenderer[] renderers = m_Instance.GetComponentsInChildren<SpriteRenderer>();

        // Go through all the renderers...
        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this tank.
            renderers[i].color = m_PlayerColor;
        }
    }

    public float getAngle()
    {
        return m_Aim.m_CurrAngle;
    }

    public float getPower()
    {
        return m_Fire.m_CurrPower;
    }

    public float getHealth()
    {
        return m_Health.m_CurrHP;
    }

    public void Fire()
    {
        m_Fire.Fire();
    }

    // Used during the phases of the game where the player shouldn't be able to control their tank.
    public void DisableControl()
    {
        m_Aim.enabled = false;
        m_Fire.enabled = false;

        //m_CanvasGameObject.SetActive(false);
    }


    // Used during the phases of the game where the player should be able to control their tank.
    public void EnableControl()
    {
        m_Aim.enabled = true;
        m_Fire.enabled = true;

        //m_CanvasGameObject.SetActive(true);
    }


    // Used at the start of each round to put the tank into it's default state.
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    public bool ShotFired()
    {
        return m_Fire.m_Fired;
    }

    public void Die()
    {
        Debug.Log(m_Instance.name + ", Player " + m_PlayerNumber + ", has died");
        m_Instance.SetActive(false);
    }
}