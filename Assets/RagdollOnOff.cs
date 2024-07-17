using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        // headColliderScript = headCollider.GetComponent<HeadCollider>();
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

    BoxCollider[] ragDollColliders;
    Rigidbody[] ragDollRigidbodies;
    void GetRagdollBits()
    {
        ragDollColliders = armatureRoot.GetComponentsInChildren<BoxCollider>(true);
        ragDollColliders = System.Array.FindAll(ragDollColliders, collider => collider.gameObject.name != "HeadCollider");

        ragDollRigidbodies = armatureRoot.GetComponentsInChildren<Rigidbody>(true);
        ragDollRigidbodies = System.Array.FindAll(ragDollRigidbodies, rb => rb.gameObject.name != "HeadCollider");

        Debug.Log("ragDollColliders: " + ragDollColliders);
        Debug.Log("ragDollRigidbodies: " + ragDollRigidbodies);
    }

    public void RagdollModeOn()
    {
        mainCollider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = true;
            Debug.Log("Collider: " + col);
        }
        foreach (Rigidbody rb in ragDollRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            Debug.Log("RigidBody: " + rb);
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

    public GameObject Armature;
    public Transform pointToRagdoll;
    public Transform lookAtPosition;
    public float timeToRagdoll;
    public void TransformToPreviousPosition()
    {
        mainCollider.enabled = false;
        // Armature.transform.position = pointToRagdoll.position;
        Armature.transform.position = Vector3.Lerp(transform.position, pointToRagdoll.position, timeToRagdoll * Time.deltaTime);
        Armature.transform.LookAt(lookAtPosition);
        mainCollider.enabled = true;
    }

}
