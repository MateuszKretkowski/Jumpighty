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

    public float delayTime = 4f;
    public float delay;

    public Rigidbody playerRb;
    public PlayerControllerPogo playerControllerPogo;
    void Start()
    {
        hasRagdolled = false;
        isUnRagdolledLocal = false;
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
            StartCoroutine(OnRagdollOff());
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer != 6 && delay == 0)
        {
            ragdollOnOff.RagdollModeOn();
            Debug.Log(collision.gameObject.layer);
            hasRagdolled = true;
            isUnRagdolledLocal = true;
        }
    }

    IEnumerator OnRagdollOff()
    {
        yield return new WaitForSeconds(4);
        isUnRagdolledLocal = false;
        ragdollOnOff.RagdollModeOff();
        playerControllerPogo.activateJump();
        ragdollOnOff.TransformToPreviousPosition();
        isUnRagdolledLocal = false;
        delay = delayTime;
    }


}
