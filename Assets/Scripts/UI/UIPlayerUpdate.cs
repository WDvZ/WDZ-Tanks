using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIPlayerUpdate : MonoBehaviour {

    public TextMeshProUGUI angleDisplay;
    public TextMeshProUGUI powerDisplay;
    public TextMeshProUGUI healthDisplay;
    public Image aimWheel;
    public Slider powerSlider;
    public Button fireButton;
    public TankManager tank;
    public GameManager currentGame;
    public float transparencyTime; // How long until the display goes back to being transparent after touch
    [Range(0, 1)] public float transparency; // The default transparency of the UI elements
    public float activeTransparency; // Transparency when player interacting with UI
    private float timer;    // Timer for setting transparency back to normal
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = transparency;
    }

    // TODO Replace with event driven calls when player aims or changes power
    // Update is called once per frame
    void Update () {
        try
        {
            tank = currentGame.GetCurrentPlayer();
            UpdateAngle(tank.getAngle(), tank.m_PlayerColor);
            UpdatePower(tank.m_Fire.Power, tank.m_PlayerColor);
            UpdateHealth(tank.getHealth(), tank.m_PlayerColor);
        }
        catch (Exception)
        {
            angleDisplay.enabled = false;
            powerDisplay.enabled = false;
            healthDisplay.enabled = false;
            aimWheel.enabled = false;
            powerSlider.gameObject.SetActive(false);
            fireButton.gameObject.SetActive(false);
        }

        if (timer > 0) {
            canvasGroup.alpha -= (canvasGroup.alpha - transparency) / timer * Time.deltaTime;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canvasGroup.alpha = transparency; // Just in case transparency hasn't completely reset when timer runs out
            }
        }
    }

    private void UpdateAngle(float angle, Color textColor)
    {
        angleDisplay.color = textColor;
        angleDisplay.text = "Angle: " + angle;
        angleDisplay.enabled = true;
        aimWheel.color = textColor;
        aimWheel.enabled = true;
        fireButton.gameObject.SetActive(true);
    }

    private void UpdatePower(float power, Color textColor)
    {
        powerDisplay.color = textColor;
        powerDisplay.text = "Power:\n" + power;
        powerDisplay.enabled = true;
        ColorBlock tempColors = powerSlider.colors;
        tempColors.normalColor = textColor;
        powerSlider.colors = tempColors;
        powerSlider.value = power;
        powerSlider.gameObject.SetActive(true);
    }
    private void UpdateHealth(float health, Color textColor)
    {
        healthDisplay.color = textColor;
        healthDisplay.text = "Health: " + Math.Round(health);
        healthDisplay.enabled = true;
    }

    public void FireButtonPress()
    {
        InteractedUI();
        tank.m_Fire.Fire();
        canvasGroup.alpha = transparency; // For the fire button we immediately want the UI transparent again
        timer = 0;
    }

    public void AimTick(int direction)
    {
        tank.m_Aim.Aim(direction);
        
    }

    public void PowerSelect()
    {
        tank.m_Fire.Power = powerSlider.value;
    }

    public void InteractedUI()
    {
        timer = transparencyTime;
        canvasGroup.alpha = activeTransparency;
    }

}

