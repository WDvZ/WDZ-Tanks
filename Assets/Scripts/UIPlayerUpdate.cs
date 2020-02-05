using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIPlayerUpdate : MonoBehaviour {

    public TextMeshProUGUI angleDisplay;
    public TextMeshProUGUI powerDisplay;
    public TextMeshProUGUI healthDisplay;
    public TankManager tank;
    public GameManager currentGame;

    // TODO Replace with event driven calls when player aims or changes power
	// Update is called once per frame
	void Update () {
        try
        {
            tank = currentGame.GetCurrentPlayer();
            updateAngle(tank.getAngle(), tank.m_PlayerColor);
            updatePower(tank.getPower(), tank.m_PlayerColor);
            updateHealth(tank.getHealth(), tank.m_PlayerColor);
        }
        catch (Exception)
        {
            updateAngle(0f, Color.white);
            updatePower(0f, Color.white);
        }
    }

    private void updateAngle(float angle, Color textColor)
    {
        angleDisplay.color = textColor;
        angleDisplay.text = "Angle: " + angle;
    }

    private void updatePower(float power, Color textColor)
    {
        powerDisplay.color = textColor;
        powerDisplay.text = "Power: " + power;
    }
    private void updateHealth(float health, Color textColor)
    {
        healthDisplay.color = textColor;
        healthDisplay.text = "Health: " + health;
    }

}

