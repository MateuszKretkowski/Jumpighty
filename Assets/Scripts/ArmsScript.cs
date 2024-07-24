using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsScript : MonoBehaviour
{

    public Transform LookAtTarget;
    public bool isRight;

    public GameObject HeadColliderObject;
    HeadCollider headCollider;

    void Start()
    {
        headCollider = HeadColliderObject.GetComponent<HeadCollider>();
    }

    void Update()
    {
        if (!headCollider.isUnRagdolledLocal)
        {
        if (isRight)
        {
            Quaternion parentRotation = transform.parent.rotation;
            Quaternion lookDownRotation = Quaternion.Euler(-90, parentRotation.eulerAngles.y, parentRotation.eulerAngles.z);
            transform.rotation = lookDownRotation;
        }
        else
        {
            Quaternion parentRotation = transform.parent.rotation;
            Quaternion lookDownRotation = Quaternion.Euler(-90, parentRotation.eulerAngles.y, parentRotation.eulerAngles.z + 180);
            transform.rotation = lookDownRotation;
        }
        }
    }

}
