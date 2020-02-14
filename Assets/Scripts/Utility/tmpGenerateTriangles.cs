using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tmpGenerateTriangles: MonoBehaviour
{
    public GameObject trianglePrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            GameObject newTriangle = GameObject.Instantiate(trianglePrefab, position, Quaternion.Euler(0,0, Random.value * 360));
        }
    }

}
