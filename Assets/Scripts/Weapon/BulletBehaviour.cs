using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletBehaviour : MonoBehaviour
{

    private const int waitTime = 2;     // How long we'll wait after the last collision before ending the turn
    public bool exploded;              // Keep track of whether the bullet has exploded yet
    public float lifeLeft;             // How long flight time the bullet has left
    public float damage;            // How much damage the bullet (on its own) inflicts

    private AreaOfEffect newEffect;
    public GameObject explosionPrefab;
    private ScObWeaponProjectile m_Behaviour;

    public void Initialize(ScObWeaponProjectile weaponProfile)
    {
        exploded = false;
        m_Behaviour = weaponProfile;
        lifeLeft = m_Behaviour.lifeTime;
        explosionPrefab = m_Behaviour.explosionPrefab;
        damage = m_Behaviour.damage;
    }

    private IEnumerator WaitForExplosion()
    {
        // Wait for waitTime seconds before finishing the turn
        Debug.Log("Entered coroutine");
        double timeSinceTriggered = (Time.time - newEffect.timeLastTriggered);
        while (timeSinceTriggered < waitTime)
        {
            Debug.Log("Waiting for explosions. Waited for " + timeSinceTriggered + " seconds");
            yield return new WaitForSeconds(1);
            timeSinceTriggered = (Time.time - newEffect.timeLastTriggered);
        }


        Destroy(this.gameObject);

    }

    private void Explosion()
    {
        exploded = true;

        GameObject newExplosion = GameObject.Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        newEffect = newExplosion.GetComponent<AreaOfEffect>();

        //newEffect.myOwner = myOwner;
        // Initialise the timeLastTriggered, because the Start() method doesn't start until this on finishes
        // Should perhaps do this in a create() constructor in AreaOfEffect
        newEffect.timeLastTriggered = Time.time;

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
        Vector2 pointOfCollision = collision.GetContact(0).point;
        Debug.Log(this.name + " collided with " + collision.collider.name);
        Debug.Log("Creating explosion...");

        // If the collision is with a part of the tank, we need to reference the TankHealth of the BaseTank
        if (collision.gameObject.name == "Tank" | collision.gameObject.name == "Turret")
        {
            ITankHealth itd = collision.gameObject.GetComponent<ITankHealth>();
            if (itd != null)
            {
                itd.ITakeDamage(damage);
            }
        }

        if (m_Behaviour.explodeOnCollision)
        {
            Explosion();
        }
    }
}
