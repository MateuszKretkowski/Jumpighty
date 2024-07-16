using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject armatureRoot;
    public Rigidbody rb;

    public GameObject headCollider;
    HeadCollider headColliderScript;
    void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }

    void Update()
    {
        headColliderScript = headCollider.GetComponent<HeadCollider>();
        if (!headColliderScript.isRagDolled)
        {
            pointToRagdoll = transform;
        }
        if (headColliderScript.isRagDolled)
        {
            Debug.Log(headColliderScript.isRagDolled);
            Debug.Log("Ragdoll mode: ON");
            RagdollModeOn();
        }
    }

    // private void OnTriggerEnter(Collider collision)
    // {
        // Debug.Log("Ragdoll mode: ON");
        // RagdollModeOn();
    // }

    // private void OnCollisionExit(Collision collision)
    // {
        // Debug.Log("Ragdoll mode: ON");
        // RagdollModeOff();
    // }

    Collider[] ragDollColliders;
    Rigidbody[] ragDollRigidbodies;
    void GetRagdollBits()
    {
        ragDollColliders = armatureRoot.GetComponentsInChildren<Collider>();
        ragDollRigidbodies = armatureRoot.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollModeOn()
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

    public void RagdollModeOff()
    {
        TransformToPreviousPosition();
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

    public Transform pointToRagdoll;
    public float timeToRagdoll;
    public void TransformToPreviousPosition()
    {
        mainCollider.enabled = false;
        transform.position = Vector3.Lerp(transform.position, pointToRagdoll.position, timeToRagdoll * Time.deltaTime);
        mainCollider.enabled = true;
    }

}
