using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float force;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Vector3 localUp = transform.TransformDirection(Vector3.up);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(localUp * force, ForceMode.Impulse);
        }
    }
}
