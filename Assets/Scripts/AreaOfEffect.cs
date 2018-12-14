using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Vector2 pointOfCollision = collision.get .GetContact(0).point;
        Debug.Log("triggered by " + collision.GetComponent<Collider2D>().name);
        Debug.Log("Distance: " + "???");

        ColliderDistance2D distance_struct = new ColliderDistance2D();
        distance_struct = collision.Distance(GetComponent<Collider2D>());

        // distance_struct has two points, vector2 I think. Use "Attach to Unity" then click play in Unity and add a breakpoint


    }
}
