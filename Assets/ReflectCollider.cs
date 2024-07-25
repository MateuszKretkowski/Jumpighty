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
        playerController.reflectForce(other);
    }
}
