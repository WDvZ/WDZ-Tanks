using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Here we add CreateAssetMenu to allow us to create new Weapons from the menu
 * fileName is the initial name of the file when created
 * menuName is the name used in the menu
 */
 [CreateAssetMenu(fileName ="New Weapon", menuName ="Weapon")]
public class ScObWeapon : ScriptableObject
{
    public GameObject bulletPrefab; // Stores out Bullet Prefab

    public string weaponName; // weapon name e.g  mortar
    public string weaponDesc; // weapon description e.g. creates a small explosion on impact

    public int ammoMax; // Maximum ammo carried
    public int ammoCurr; // Ammo currently carried
    public float bulletSpeed; // How fast the bullet flies
    public float damage; // How much damage the bullet inflicts

}
