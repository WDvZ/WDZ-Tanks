using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    public float lifeLeft;             // How long flight time the bullet has left
    private float distance;             // Distance from explosion
    public float timeLastTriggered;  // Last time a collision was triggered

    private Color explosionColor;     // The initial colour of the explosion prefab/sprite
    private float initTransparency; // The initial transparency of the explosion prefab/sprite

    private ScObExplosion m_Explosion;
    private SpriteRenderer explosionRenderer;

    private Collider2D m_collider;

    public void Initialize(ScObExplosion explosionProfile)
    {
        // Save the time the explosion began
        timeLastTriggered = Time.time;
        AudioManager.instance.PlaySound("explosion");

        m_Explosion = explosionProfile;
        lifeLeft = m_Explosion.fadeTime;

        gameObject.GetComponent<Transform>().parent.localScale = Vector3.one * m_Explosion.explosionSize;

        explosionRenderer = this.GetComponentInParent<SpriteRenderer>();
        explosionColor = explosionRenderer.color;
        initTransparency = explosionColor.a;

        m_collider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Save the last time there was a collision
        timeLastTriggered = Time.time;
        distance = Mathf.Max(-collision.Distance(m_collider).distance, 0);

        Debug.Log("triggered by " + collision.gameObject.name);
        Debug.Log("Distance: " + distance);
        Debug.Log("Explosion triggered at " + timeLastTriggered.ToString());

        float damage = distance * m_Explosion.damageMultiplier + m_Explosion.baseDamage;

        // If the collision is with a part of the tank, we need to reference the TankHealth of the BaseTank
        if (collision.gameObject.name == "Tank" | collision.gameObject.name == "Turret")
        {
            ITankHealth itd = collision.GetComponentInParent<ITankHealth>();
            if (itd != null)
            {
                itd.ITakeDamage(damage);
            }
        }

        // Add force to everything triggered by the explosion, at a direction outward from the explosion relative to damage
        if (collision.attachedRigidbody != null)
        {
            collision.attachedRigidbody.AddForce(m_Explosion.explosionForce * distance * (collision.transform.position - gameObject.transform.position).normalized, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        // Decrease explosion's lifetime
        lifeLeft -= Time.deltaTime;
        explosionColor.a = (lifeLeft / m_Explosion.fadeTime) * initTransparency;
        explosionRenderer.color = explosionColor;

        if (lifeLeft <= 0f)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
