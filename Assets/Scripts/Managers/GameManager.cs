﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;


//=================Credits==================
// Unity Tanks! Tutorial
// https://www.youtube.com/channel/UCjDiDU1hXq31QIr0vN_yacQ xOctoManx Tutorials
// https://www.gamedevelopment.blog/category/tutorial/fu2gt/
// https://www.freemusicarchive.org/
// https://freesound.org/people/SoundCollectah/packs/6951/


public class GameManager : MonoBehaviour {

    public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
    public float m_StartDelay = 0f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 0f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    //public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public TextMeshProUGUI m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
    public TankManager[] m_Tanks;             // A collection of managers for enabling and disabling different aspects of the tanks.
    public UIAimWheel uiAimWheel;           // The UI Aim wheel that controls angle.

    public Transform terrainMin;    // Starting point of terrain
    public Transform terrainMax;    // Ending point of terrain
    public int numTerrain;          // Number of terrain pieces to generate
    public GameObject terrainPrefab;


    private int m_RoundNumber;                  // Which round the game is currently on.
    private int m_CurrentTurn;                  // Player whose turn it is
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
    private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

    [HideInInspector] public float turnEnder;             // Seconds before ending the turn
    public float inactivityTime;        // Seconds to add to turnEnder every time something happens


    private void Start()
    {
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnTerrain();
        SpawnAllTanks();
        //SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }

    public void SpawnTerrain()
    {
        for (int i=0; i < numTerrain; i++)
        {
            float x = UnityEngine.Random.value * (terrainMax.position.x - terrainMin.position.x) + terrainMin.position.x;
            float y = UnityEngine.Random.value * (terrainMax.position.y - terrainMin.position.y) + terrainMin.position.y;
            GameObject newTriangle = GameObject.Instantiate(terrainPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, UnityEngine.Random.value * 360));
            float randomScale = (float)(UnityEngine.Random.value * 0.5 + 0.1);
            newTriangle.transform.localScale = new Vector3(randomScale, randomScale, 1);
        }
    }

    private void SpawnAllTanks()
    {
        // For all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].m_AimWheel = uiAimWheel;
            m_Tanks[i].Setup();
        }
    }

    public TankManager GetCurrentPlayer()
    {
        return m_Tanks[m_CurrentTurn - 1];
    }

    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[m_Tanks.Length];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        //m_CameraControl.m_Targets = targets;
    }


    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(TurnLoop());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (m_GameWinner != null)
        {
            // If there is a game winner, go back to the main menu.
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();
        m_CurrentTurn = 0;      // Initialise to zero, but TurnStarting will increment to 1. Player 1 will start each round

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        //m_CameraControl.SetStartPositionAndSize();

        // Increment the round number and display text showing the players what round it is.
        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }

    private IEnumerator TurnLoop()
    {
        // Check if a round winner has been found.
        if (m_RoundWinner != null)
        {
            // If there is a round winner, don't do the TurnLoop and let the round end.
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        // Start off by running the 'TurnStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(TurnStarting());

        // Once the 'TurnStarting' coroutine is finished, run the 'TurnPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(TurnPlaying());

        // Once execution has returned here, run the 'TurnEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(TurnEnding());

        if (!OneTankLeft())
        {
            yield return StartCoroutine(TurnLoop());
        }
    }

    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();

        // Clear the winner from the previous round.
        m_RoundWinner = null;

        // See if there is a winner now the round is over.
        m_RoundWinner = GetRoundWinner();

        // If there is a winner, increment their score.
        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        // Now the winner's score has been incremented, see if someone has one the game.
        m_GameWinner = GetGameWinner();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }



    private IEnumerator TurnStarting()
    {
        m_CurrentTurn++;
        if (m_CurrentTurn > (m_Tanks.Length))
        {
            m_CurrentTurn = 1;
        }

        m_MessageText.text = "PLAYER " + (m_CurrentTurn) + "'S TURN";

        turnEnder = inactivityTime;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }

    private IEnumerator TurnPlaying()
    {
        m_Tanks[m_CurrentTurn-1].EnableControl();
        // Wait for the specified length of time until yielding control back to the game loop.
        // While there is not one tank left...
        while (turnEnder >= 0)
        {
            if (!m_Tanks[m_CurrentTurn - 1].m_Fire.m_Fired)
            {
                turnEnder = inactivityTime;
            }
            // ... return on the next frame.
            turnEnder -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TurnEnding()
    {
        DisableTankControl();
        // Wait for the specified length of time until yielding control back to the game loop.
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (m_Tanks[i].getHealth() <= 0)
                m_Tanks[i].Die();
        }

        yield return null;
    }

    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OneTankLeft()
    {
        // Start the count of tanks left at zero.
        int numTanksLeft = 0;

        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        // If there are one or fewer tanks remaining return true, otherwise return false.
        return numTanksLeft <= 1;
    }


    // This function is to find out if there is a winner of the round.
    // This function is called with the assumption that 1 or fewer tanks are currently active.
    private TankManager GetRoundWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them is active, it is the winner so return it.
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        // If none of the tanks are active it is a draw so return null.
        return null;
    }


    // This function is to find out if there is a winner of the game.
    private TankManager GetGameWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them has enough rounds to win the game, return it.
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        // If no tanks have enough rounds to win, return null.
        return null;
    }


    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "DRAW!";

        // If there is a winner then change the message to reflect that.
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // Go through all the tanks and add each of their scores to the message.
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        // If there is a game winner, change the entire message to reflect that.
        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    //private void EnableTankControl()
    //{
    //    for (int i = 0; i < m_Tanks.Length; i++)
    //    {
    //        m_Tanks[i].EnableControl();
    //    }
    //}


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}
