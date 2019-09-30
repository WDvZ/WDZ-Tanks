using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour {

    private float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        float distance2 = Vector2.Distance(this.transform.position, collision.gameObject.transform.position);

        Debug.Log("triggered by " + collision.gameObject.name);
        Debug.Log("Distance: " + distance2);

        // A direct hit should cause more damage
        // Distance is to center of object, so shouldn't use this for direct hits

    }
}
