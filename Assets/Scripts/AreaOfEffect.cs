using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AreaOfEffect : MonoBehaviour {

    private float distance;
    public DateTime timeSinceTriggered;
    public BasePlayer myOwner;

    // Use this for initialization
    void Start () {

        // This returns the correct info, but still gives an error because we're setting it outside, which is probably wrong
        Debug.Log("My owner is " + myOwner.name + ": " + myOwner.playerName);
        timeSinceTriggered = DateTime.Now;
        Debug.Log("Explosion started at " + timeSinceTriggered.ToString("HH:mm:ss tt"));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        timeSinceTriggered = DateTime.Now;
        float distance2 = Vector2.Distance(this.transform.position, collision.gameObject.transform.position);

        Debug.Log("triggered by " + collision.gameObject.name);
        Debug.Log("Distance: " + distance2);
        Debug.Log("Explosion triggered at " + timeSinceTriggered.ToString("HH:mm:ss tt"));

        // A direct hit should cause more damage
        // Distance is to center of object, so shouldn't use this for direct hits

    }
}
