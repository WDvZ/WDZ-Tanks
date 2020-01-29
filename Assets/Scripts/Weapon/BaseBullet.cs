using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Consider changing this to a scriptable object for easier customisation
// https://www.gamedevelopment.blog/full-unity-2d-game-tutorial-2019-scriptable-objects/

public class BaseBullet : MonoBehaviour {
    private const int waitTime = 2;     // How long we'll wait after the last collision before ending the turn
    public float bulletSpeedMult =1f;   // Bullet speed multiplier
    public float lifeTime = 7f;         // Time in seconds before we trigger explosion (if no collisions have occured)
    private float bulletSpeed;          // Bullet base speed
    private bool exploded;              // Keep track of whether the bullet has exploded yet

    //public BasePlayer myOwner;          // Player that shot

    /* Explosion stuff */
    public float bulletExplosionForce = 1000f;  // Strenght of explosion
    public float explosionRadius = 4f;          // Size of explosion
    public GameObject explosion;
    public AreaOfEffect newEffect;

    public void ShootBullet(float aPower)
    {
        bulletSpeed = aPower;
        exploded = false;
        // Launch the bullet forward
        this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Transform>().right * bulletSpeed * bulletSpeedMult, ForceMode2D.Impulse);
        AudioManager.instance.PlaySound("gunshot");
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

    private void Explosion()
    {
        exploded = true;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the details of what collided with the bullet
        Vector2 pointOfCollision = collision.GetContact(0).point;
        Debug.Log(this.name + " collided with " + collision.collider.name);
        Debug.Log("Creating explosion...");

        Explosion();
    }

    private void Update()
    {
        // Decrease bullet's lifetime
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f & exploded==false)
        {
            Explosion();
        }
    }

}
