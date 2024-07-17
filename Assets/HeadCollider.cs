using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    // Start is called before the first frame update

    public RagdollOnOff ragdollOnOff;
    public bool isRagDolled;
    public bool hasRagdolled;
    public bool isUnRagdolledLocal;
    void Start()
    {
        hasRagdolled = false;
        isUnRagdolledLocal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnRagdolledLocal)
        {
            StartCoroutine(OnRagdollOff());
            isUnRagdolledLocal = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Player" && !hasRagdolled)
        {
            Debug.Log("COLLISION ON");
            ragdollOnOff.RagdollModeOn();
            hasRagdolled = true;
            isUnRagdolledLocal = true;
        }
    }

    IEnumerator OnRagdollOff()
    {
        yield return new WaitForSeconds(4);
        ragdollOnOff.RagdollModeOff();
        ragdollOnOff.TransformToPreviousPosition();
        isUnRagdolledLocal = false;
    }

    
}
