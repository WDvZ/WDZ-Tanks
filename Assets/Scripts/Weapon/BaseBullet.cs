using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseBullet : MonoBehaviour {
    private const int waitTime = 5;     // How long we'll wait after the last collision before ending the turn
    public float bulletSpeedMult =1f;   // Bullet speed multiplier
    private float bulletSpeed;          // Bullet base speed

    //public BasePlayer myOwner;          // Player that shot

    /* Explosion stuff */
    public float bulletExplosionForce = 1000f;  // Strenght of explosion
    public float explosionRadius = 4f;          // Size of explosion
    public GameObject explosion;
    public AreaOfEffect newEffect;

    public void ShootBullet(float aPower)
    {
        bulletSpeed = aPower;
        // Launch the bullet forward
        this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Transform>().right * bulletSpeed * bulletSpeedMult, ForceMode2D.Impulse);
    }

    private IEnumerator WaitForExplosion()
    {
        // Wait for waitTime seconds before finishing the turn
        Debug.Log("Entered coroutine");
        double timeSinceTriggered = (DateTime.Now - newEffect.timeLastTriggered).TotalSeconds;
        while (timeSinceTriggered < waitTime)
        {
            Debug.Log("Waiting for explosions. Waited for " + timeSinceTriggered + " seconds");
            yield return new WaitForSeconds(1);
            timeSinceTriggered = (DateTime.Now - newEffect.timeLastTriggered).TotalSeconds;
      }


        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the details of what collided with the bullet
        Vector2 pointOfCollision = collision.GetContact(0).point;
        Debug.Log(this.name + " collided with " + collision.collider.name);
        Debug.Log("Creating explosion...");

        GameObject newExplosion = GameObject.Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        newEffect = newExplosion.GetComponent<AreaOfEffect>();
        //newEffect.myOwner = myOwner;
        // Initialise the timeLastTriggered, because the Start() method doesn't start until this on finishes
        // Should perhaps do this in a create() constructor in AreaOfEffect
        newEffect.timeLastTriggered = DateTime.Now;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(WaitForExplosion());
    }

}
