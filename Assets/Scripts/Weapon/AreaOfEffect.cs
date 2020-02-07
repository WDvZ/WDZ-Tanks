using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AreaOfEffect : MonoBehaviour {

    private float distance;             // Distance from explosion
    public float timeLastTriggered;  // Last time a collision was triggered
    //public BasePlayer myOwner;          // Player that caused the explosion

    // Use this for initialization
    void Start () {

        // This returns the correct owner info, but still gives an error because we're setting it outside(?), which is probably wrong
        // Debug.Log("My owner is " + myOwner.name + ": " + myOwner.playerName);

        // Save the time the explosion began
        timeLastTriggered = Time.time;
        Debug.Log("Explosion started at " + timeLastTriggered.ToString());
        AudioManager.instance.PlaySound("explosion");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Save the last time there was a collision
        timeLastTriggered = Time.time;
        distance = Vector2.Distance(this.transform.position, collision.gameObject.transform.position);

        Debug.Log("triggered by " + collision.gameObject.name);
        Debug.Log("Distance: " + distance);
        Debug.Log("Explosion triggered at " + timeLastTriggered.ToString());

        float damage = distance * 100;

        // If the collision is with a part of the tank, we need to reference the TankHealth of the BaseTank
        if (collision.gameObject.name == "Tank" | collision.gameObject.name == "Turret")
        {
            ITankHealth itd = collision.GetComponentInParent<ITankHealth>();
            if (itd != null)
            {
                itd.ITakeDamage(damage);
            }
        }
    }
}
