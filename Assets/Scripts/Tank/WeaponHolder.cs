using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public ScObWeaponProjectile scObWeaponProjectile; // The scriptable object we're using for this weapon

    private const int waitTime = 2;     // How long we'll wait after the last collision before ending the turn

    private GameObject newBullet;
    private GameObject newExplosion;

    public void ShootBullet(float aPower, Transform spawnpoint)
    {
        // Instantiate the projectile rotated the correct way
        newBullet = GameObject.Instantiate(scObWeaponProjectile.bulletPrefab, spawnpoint.position, spawnpoint.rotation);
        //newBullet.GetComponent<BaseBullet>().myOwner = basePlayer;

        newBullet.GetComponent<BulletBehaviour>().Initialize(scObWeaponProjectile);

        // Launch the bullet forward
        newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.GetComponent<Transform>().right * aPower/1000 * scObWeaponProjectile.speedMultiplier, ForceMode2D.Impulse);
        AudioManager.instance.PlaySound("gunshot");
    }

}
