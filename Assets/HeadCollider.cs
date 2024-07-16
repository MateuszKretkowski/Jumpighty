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
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnRagdolledLocal)
        {
            StartCoroutine(OnRagdollOff());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && !hasRagdolled)
        {
            Debug.Log("COLLISION ON");
            isRagDolled = true;
            hasRagdolled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("COLLISION OFF");
        isUnRagdolledLocal = true;
    }

    IEnumerator OnRagdollOff()
    {
        yield return new WaitForSeconds(4);
        ragdollOnOff.RagdollModeOff();
        isUnRagdolledLocal = false;
    }

    
}
