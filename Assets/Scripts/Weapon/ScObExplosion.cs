using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Explosion", menuName = "Weapon/Explosion")]
public class ScObExplosion : ScriptableObject
{
    public GameObject explosionPrefab;              // Stores our Explosion Prefab

    public string explosionName;                       // Explosion name e.g  Small Explosion
    public string explosionDesc;                       // Explosion description e.g. creates a small explosion

    public float explosionSize;     // Explosion size (radius)
    public float baseDamage;        // Base amount of damage dealt
    public float damageMultiplier;   // Damage multiplier
    public float fadeTime;          // How long it takes for the explosion to fade away
    public float explosionForce;    // How much force the explosion applies, relative to distance

    private float timeLastTriggered;
    public void OnExplode()
    {
        // Save the time the explosion began
        timeLastTriggered = Time.time;
        Debug.Log("Explosion started at " + timeLastTriggered);
        AudioManager.instance.PlaySound("explosion");
    }

    public float DamageCalc(float distance)
    {
        return distance * 100;
    }
}
