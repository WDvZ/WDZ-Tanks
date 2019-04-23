using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour {

    public float bulletExplosionForce = 1000f;
    public float explosionRadius = 4f;
    public GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 pointOfCollision = collision.GetContact(0).point;
        //Debug.Log("Collided with " + collision.collider.name);

        GameObject newExplosion = GameObject.Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);

        // Next steps: Have the Shoot method pass the tank that fired to this method so we can update the state
    }
}
