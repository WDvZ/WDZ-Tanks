using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Here we add CreateAssetMenu to allow us to create new Weapons from the menu
 * fileName is the initial name of the file when created
 * menuName is the name used in the menu
 */
 [CreateAssetMenu(fileName ="New Projectile", menuName ="Weapon/Projectile")]
 // An abstract class has functions without implementation - Since our actual weapon will have the implementation
public class ScObWeaponProjectile : ScriptableObject
{
    public ScObExplosion scObExplosion;         // Type of explosion
    public GameObject bulletPrefab;              // Bullet prefab we're using

    public string weaponName;                       // weapon name e.g  mortar
    public string weaponDesc;                       // weapon description e.g. creates a small explosion on impact

    public int ammoMax; // Maximum ammo carried
    public float speedMultiplier; // How fast the bullet flies
    public float damage; // How much damage the bullet inflicts
    public float lifeTime;  // How long the bullet can fly before exploding

    public float windAffect;                        // 0-100%, how badly does wind affect the projectile?

    public bool explodeOnCollision; // e.g. false for grenade, true for missile

    //public ??? soundEffectShoot - Sound when we shoot
    //public ??? soundEffectHit - Sound when bullet collides
    //public ??? soundEffectExplode - Sound of explosion - Explosion could be own scriptable object


}
