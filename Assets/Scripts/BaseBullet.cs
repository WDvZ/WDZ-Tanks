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
        Debug.Log("Collided with " + collision.collider.name);

        GameObject newExplosion = GameObject.Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);

        // Check what next state should be
        myOwner.stateMachine.ChangeState(new Aiming(myOwner));

        // Next steps: Have the Shoot method pass the tank that fired to this method so we can update the state
    }

}
