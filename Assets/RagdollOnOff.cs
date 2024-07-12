using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject armatureRoot;
    public Rigidbody rb;
    void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Ragdoll mode: ON");
        RagdollModeOn();
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Ragdoll mode: ON");
        RagdollModeOff();
    }

    Collider[] ragDollColliders;
    Rigidbody[] ragDollRigidbodies;
    void GetRagdollBits()
    {
        ragDollColliders = armatureRoot.GetComponentsInChildren<Collider>();
        ragDollRigidbodies = armatureRoot.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollModeOn()
    {
        mainCollider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in ragDollRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    void RagdollModeOff()
    {
        mainCollider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in ragDollRigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

}
