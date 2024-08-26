using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCollider : MonoBehaviour
{
    public Rigidbody rb;
    public float reflectMultiplier = 1.0f;
    // public PlayerController playerController;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "default_obstacle":
                // Si�a do ty�u w lokalnym uk�adzie wsp�rz�dnych
                Vector3 localBackForce = transform.TransformDirection(Vector3.back) * 15f;

                // Si�a do g�ry w lokalnym uk�adzie wsp�rz�dnych
                Vector3 localUpForce = transform.TransformDirection(Vector3.up) * 15f;

                // Dodaj si�y w trybie impulsu
                rb.AddForce(localBackForce, ForceMode.Impulse);
                rb.AddForce(localUpForce, ForceMode.Impulse);

                break;
            case "Trampoline":

                break;
        }
    }
}
