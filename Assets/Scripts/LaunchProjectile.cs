using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour {

    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            // Instantiate the projectile rotated the correct way
            GameObject newBullet = GameObject.Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
            // Launch forward
            newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.GetComponent<Transform>().up * bulletSpeed, ForceMode2D.Impulse); // Check what happens if we use impulse...
        }
	}
}
