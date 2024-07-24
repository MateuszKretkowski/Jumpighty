using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject headColliderObject;
    HeadCollider headCollider;
    BoxCollider boxCollider;
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        headCollider = headColliderObject.GetComponent<HeadCollider>();
        if (headCollider.isUnRagdolledLocal)
        {
            //boxCollider.enabled = false;
        }
        else
        {
            //boxCollider.enabled = false;
        }
    }
}
