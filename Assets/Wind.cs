using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float initialForce;
    public float continiousForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            rb.velocity += forward * initialForce;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            rb.velocity += forward * continiousForce;
        }
    }
}
