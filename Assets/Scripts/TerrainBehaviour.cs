using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehaviour : MonoBehaviour
{
    int numJoints = 0;
    List<int> colliderList = new List<int>();
    public float breakForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain" && collision.gameObject.GetComponent<Rigidbody2D>() != null && !(colliderList.Contains(collision.gameObject.GetInstanceID())) && numJoints < 3)
        {
            SpringJoint2D springJoint = gameObject.AddComponent<SpringJoint2D>();
            springJoint.connectedBody = collision.rigidbody;
            springJoint.breakForce = breakForce;
            colliderList.Add(collision.gameObject.GetInstanceID());
            numJoints += 1;
        }
    }
}
