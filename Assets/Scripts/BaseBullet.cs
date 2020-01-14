using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    public float bulletSpeedMult =1f;
    private float bulletSpeed;

    public BasePlayer myOwner;

    /* Explosion stuff */
    public float bulletExplosionForce = 1000f;
    public float explosionRadius = 4f;
    public GameObject explosion;

    public void ShootBullet(float aPower)
    {
        bulletSpeed = aPower;
        // Launch forward
        this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Transform>().right * bulletSpeed * bulletSpeedMult, ForceMode2D.Impulse);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 pointOfCollision = collision.GetContact(0).point;
        Debug.Log(this.name + " collided with " + collision.collider.name);
        Debug.Log("Creating explosion...");

        GameObject newExplosion = GameObject.Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

        // Check what next state should be
        // May need to add a check for whether the shooting and damage calculations are finished. e.g. have the bullet or area of effect log the last time it was triggered and wait until nothing happens for 10 seconds.
        myOwner.stateMachine.ChangeState(new Aiming(myOwner));

        Destroy(gameObject);

        // The Shoot method passes the tank that fired to this method so we can update the state
    }

}
