using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletBehaviour : MonoBehaviour
{

    private const int waitTime = 2;     // How long we'll wait after the last collision before ending the turn
    public bool exploded;              // Keep track of whether the bullet has exploded yet
    public float lifeLeft;             // How long flight time the bullet has left

    private ExplosionBehaviour newExplosionBehaviour;
    private GameObject explosionPrefab;
    private ScObWeaponProjectile m_Behaviour;
    private ScObExplosion m_Explosion;

    public void Initialize(ScObWeaponProjectile weaponProfile)
    {
        exploded = false;
        m_Behaviour = weaponProfile;
        lifeLeft = m_Behaviour.lifeTime;
        m_Explosion = m_Behaviour.scObExplosion;
        explosionPrefab = m_Explosion.explosionPrefab;
    }

    private IEnumerator WaitForExplosion()
    {
        // Wait for waitTime seconds before finishing the turn
        Debug.Log("Entered coroutine");
        double timeSinceTriggered = (Time.time - newExplosionBehaviour.timeLastTriggered);
        while (timeSinceTriggered < waitTime)
        {
            Debug.Log("Waiting for explosions. Waited for " + timeSinceTriggered + " seconds");
            yield return new WaitForSeconds(1);
            timeSinceTriggered = (Time.time - newExplosionBehaviour.timeLastTriggered);
        }


        Destroy(this.gameObject);

    }

    private void Explosion()
    {
        exploded = true;

        GameObject newExplosion = GameObject.Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        newExplosionBehaviour = newExplosion.GetComponentInChildren<ExplosionBehaviour>();
        newExplosionBehaviour.Initialize(m_Explosion);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(WaitForExplosion());
    }

    private void Update()
    {
        // Decrease bullet's lifetime
        lifeLeft -= Time.deltaTime;
        if (lifeLeft <= 0f & exploded == false)
        {
            Explosion();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the details of what collided with the bullet
        Debug.Log(this.name + " collided with " + collision.collider.name);
        Debug.Log("Creating explosion...");

        // If the collision is with a part of the tank, we need to reference the TankHealth of the BaseTank
        if (collision.collider.name == "Tank" | collision.collider.name == "Turret")
        {
            ITankHealth itd = collision.collider.GetComponentInParent<ITankHealth>();
            if (itd != null)
            {
                itd.ITakeDamage(m_Behaviour.damage);
            }
        }

        if (m_Behaviour.explodeOnCollision)
        {
            Explosion();
        }
    }
}
