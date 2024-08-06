using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RagdollOnOff : MonoBehaviour
{
    // public BoxCollider mainCollider;
    public GameObject armatureRoot;
    public Rigidbody rb;

    public GameObject headCollider;
    HeadCollider headColliderScript;

    bool isFirstTime;

    void Start()
    {
        isFirstTime = true;
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

    Transform[] bones;
    Vector3[] initialPositions;
    Quaternion[] initialRotations;
    BoxCollider[] ragDollColliders;
    Rigidbody[] ragDollRigidbodies;
    public Animator animator;
    void GetRagdollBits()
    {
        bones = armatureRoot.GetComponentsInChildren<Transform>(true);
        initialPositions = new Vector3[bones.Length];
        initialRotations = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            initialPositions[i] = bones[i].localPosition;
            initialRotations[i] = bones[i].localRotation;
        }

        ragDollColliders = armatureRoot.GetComponentsInChildren<BoxCollider>(true);
        ragDollColliders = System.Array.FindAll(ragDollColliders, collider => collider.gameObject.name != "HeadCollider" && collider.gameObject.tag != "arms");

        ragDollRigidbodies = armatureRoot.GetComponentsInChildren<Rigidbody>(true);
        ragDollRigidbodies = System.Array.FindAll(ragDollRigidbodies, rb => rb.gameObject.name != "HeadCollider");

        Debug.Log("ragDollColliders: " + ragDollColliders);
        Debug.Log("ragDollRigidbodies: " + ragDollRigidbodies);
    }

    public void resetRagdoll()
    {
        foreach (var rb in ragDollRigidbodies)
        {
            // rb.isKinematic = true;
        }

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localPosition = initialPositions[i];
            bones[i].localRotation = initialRotations[i];
        }

        foreach (var rb in ragDollRigidbodies)
        {
            // rb.isKinematic = false;
        }
    }

    // public GameObject[] arms;
    public void RagdollModeOn()
    {
        //mainCollider.enabled = false;
        // rb.isKinematic = true;
        // rb.useGravity = false;
        animator.enabled = false;
        rb.AddForce(Vector3.back * 10f, ForceMode.Impulse);
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in ragDollRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        // foreach (GameObject go in arms)
        // {
            // go.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10f, ForceMode.Impulse);
        // }
    }

    public void RagdollModeOff()
    {
        animator.enabled = true;
        TransformToPreviousPosition();

        if (!isFirstTime)
        {
            resetRagdoll();
        }
        isFirstTime = false;
        //mainCollider.enabled = true;
        // rb.isKinematic = false;
        // rb.useGravity = true;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in ragDollRigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            Debug.Log("RB: " + rb.gameObject.name);
        }
    }

    public GameObject Armature;
    public Transform pointToRagdoll;
    public Transform lookAtPosition;
    public float timeToRagdoll;
    public float forceRagdoll;
    public void TransformToPreviousPosition()
    {
        transform.rotation = Quaternion.EulerRotation(0f, 0f, 0f);
        //rb.AddForce(Vector3.up * forceRagdoll, ForceMode.Impulse);
    }

}
