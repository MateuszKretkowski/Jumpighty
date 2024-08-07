using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    // Start is called before the first frame update

    public RagdollOnOff ragdollOnOff;
    public bool isRagDolled;
    public bool hasRagdolled;
    public bool isUnRagdolledLocal;

    public float delayTime = 4f;
    public float delay;

    public float delayRagdoll;
    public float delayRagdollTime = 3f;
    public bool canRunTime;

    bool once;

    public Rigidbody playerRb;
    public PlayerControllerPogo playerControllerPogo;
    void Start()
    {
        hasRagdolled = false;
        isUnRagdolledLocal = false;
        once = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        if (delay < 0)
        {
            delay = 0;
        }
        if (isUnRagdolledLocal)
        {
            if (!once)
            {
                StartCoroutine(OnRagdollOff());
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerRb.velocity.y < 0.5f)
        {
            canRunTime = true;
        }
        else
        {
            canRunTime = false;
        }
        if (canRunTime && delayRagdoll > 0)
        {
            delayRagdoll -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer != 6 && delay == 0)
        {
            ragdollOnOff.RagdollModeOn();
            Debug.Log(collision.gameObject.layer);
            hasRagdolled = true;
            isRagDolled = true;
            if (delayRagdoll <= 0)
            {
                isUnRagdolledLocal = true;
            }
        }
    }

    IEnumerator OnRagdollOff()
    {
        once = true;
        yield return new WaitForSeconds(4);
        isUnRagdolledLocal = false;
        ragdollOnOff.RagdollModeOff();
        ragdollOnOff.TransformToPreviousPosition();
        isRagDolled = false;
        isUnRagdolledLocal = false;
        delay = delayTime;
        once = false;
    }


}
