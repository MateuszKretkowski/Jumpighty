using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public Animator animator;

    public bool isPreparing;
    public bool canJump;

    [SerializeField] float force;
    [SerializeField] float maxForce;
    [SerializeField] float minForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = minForce;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canJump) isPreparing = true;

        if (isPreparing && canJump)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                animator.ResetTrigger("landTrigger");
                animator.SetTrigger("jumpTrigger");
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                force = minForce;
                isPreparing = false;
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPreparing && canJump)
        {
            if (force < maxForce) force += 1;
        }
        if (rb.velocity.y < 0)
        {
            animator.SetTrigger("landTrigger");
            animator.ResetTrigger("jumpTrigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "default_obstacle")
        {
            Debug.Log("triggerenter");
            canJump = true;
            StartCoroutine(JumpCaller());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "default_obstacle")
        {
            Debug.Log("triggerex");
            canJump = false;
        }
    }



    private IEnumerator JumpCaller()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isPreparing && canJump)
        {
            animator.ResetTrigger("landTrigger");
            animator.SetTrigger("jumpTrigger");
            rb.AddForce(Vector3.up * minForce, ForceMode.Impulse);
            force = minForce;
            canJump = false;
        }
    }
}
