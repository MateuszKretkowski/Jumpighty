using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCollider : MonoBehaviour
{

    public PlayerController playerController;
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
            case "Trampoline":
                playerController.reflectForce(other, 15f);
                break;
        }
    }
}
