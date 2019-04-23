using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    public float bulletSpeedMult =1f;
    private float bulletSpeed;

    public void ShootBullet(float power)
    {
        bulletSpeed = power;
        // Launch forward
        this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Transform>().right * bulletSpeed * bulletSpeedMult, ForceMode2D.Impulse);
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		
	}
}
