using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : MonoBehaviour {

    public BasePlayer Player;
    public BasePlayer Enemy;
    public int turnCounter;

	// Use this for initialization
	void Start () {
        // Initialise the list of players
        IList<BasePlayer> playerList = new List<BasePlayer>();

        // Spawn the tanks
        Player = GameObject.Instantiate(Player);
        Player.transform.localPosition = new Vector3(-8f, -5.2f, 0f);
        Player.playerName = "Player";
        playerList.Add(Player);
        
        Enemy = GameObject.Instantiate(Enemy);
        Enemy.transform.localPosition = new Vector3(0f, -5.2f, 0f);
        Enemy.playerName = "Enemy";
        playerList.Add(Enemy);

        Debug.Log("Number of players: " + playerList.Count);

        // Set the turn
        // First player in the list starts the Aiming state
        turnCounter = 0;
        playerList[turnCounter].stateMachine.ChangeState(new Aiming(playerList[turnCounter]));
        // From player 2 onward get the Waiting state
        for (int i = 1; i<playerList.Count; i++)
        {
            playerList[i].stateMachine.ChangeState(new Waiting(playerList[i]));
        }
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(TurnStart());
        yield return StartCoroutine(TurnPlay());
        yield return StartCoroutine(TurnEnd());

        //if (RoundWinner != null)
        //{
        //    throw new NotImplementedException();
        //}
        //else
        //{
        //    StartCoroutine(GameLoop());
        //}
    }

    private string TurnStart()
    {
        throw new NotImplementedException();
    }

    private string TurnPlay()
    {
        throw new NotImplementedException();
    }
    private string TurnEnd()
    {
        throw new NotImplementedException();
    }



    // Update is called once per frame
    void Update () {
		
	}
}
