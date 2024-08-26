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
                // Si³a do ty³u w lokalnym uk³adzie wspó³rzêdnych
                Vector3 localBackForce = transform.TransformDirection(Vector3.back) * 15f;

                // Si³a do góry w lokalnym uk³adzie wspó³rzêdnych
                Vector3 localUpForce = transform.TransformDirection(Vector3.up) * 15f;

                // Dodaj si³y w trybie impulsu
                rb.AddForce(localBackForce, ForceMode.Impulse);
                rb.AddForce(localUpForce, ForceMode.Impulse);

                break;
            case "Trampoline":

                break;
        }
    }
}
