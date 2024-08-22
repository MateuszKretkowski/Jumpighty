using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ShadowDrop : MonoBehaviour
{
    public GameObject sphere;
    public float normalizer;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f))
        {
            Debug.DrawRay(transform.localPosition, Vector3.down * hit.distance, Color.yellow);
            sphere.transform.position = new Vector3(transform.position.x, -hit.distance + 1f, transform.position.z);
            sphere.transform.localScale = new Vector3(hit.distance, 0, hit.distance);
        }

    }
}
