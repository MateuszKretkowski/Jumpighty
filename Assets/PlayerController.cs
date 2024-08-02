using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public Animator animator;
    public Animator pogoAniamtor;

    public bool isPreparing;
    public bool canJump;

    public Transform rotationObject;
    public float rotationTimeMax;
    public float rotationTime;
    public bool hasRotated;

    [SerializeField] float rotationForce;

    [SerializeField] float downwardsForce;

    [SerializeField] float force;
    [SerializeField] float maxForce;
    [SerializeField] float minForce;

    void Start()
    {
        hasRotated = true;
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
                pogoAniamtor.ResetTrigger("pogo_landTrigger");
                pogoAniamtor.SetTrigger("pogo_jumpTrigger");

                animator.ResetTrigger("landTrigger");
                animator.ResetTrigger("landDeepTrigger");
                animator.SetTrigger("jumpTrigger");
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                force = minForce;
                isPreparing = false;
                canJump = false;
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * downwardsForce);
        }

        if (rotationTime >= rotationTimeMax)
        {
            hasRotated = true;
        } 
    }

    private void FixedUpdate()
    {
        if (!canJump && !hasRotated)
        {
            Transform targetTransform = rotationObject.transform;
            Vector3 direction = targetTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 2.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (rotationTime < rotationTimeMax) rotationTime += 1f;
        }

        if (isPreparing && canJump)
        {

            if (force < maxForce) force += 1;
            animator.ResetTrigger("landTrigger");
            animator.SetTrigger("landDeepTrigger");

            pogoAniamtor.ResetTrigger("pogo_jumpTrigger");
            pogoAniamtor.SetTrigger("pogo_landTrigger");
        }
        if (rb.velocity.y < -1)
        {
            animator.SetTrigger("fallTrigger");
            animator.ResetTrigger("jumpTrigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "default_obstacle")
        {
            Debug.Log("triggerenter");

            pogoAniamtor.ResetTrigger("pogo_jumpTrigger");
            pogoAniamtor.SetTrigger("pogo_landTrigger");

            animator.ResetTrigger("fallTrigger");
            animator.SetTrigger("landTrigger");


            canJump = true;
            hasRotated = false;
            rotationTime = 0f;
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
        yield return new WaitForSeconds(0.2f);
        if (!isPreparing && canJump)
        {
            animator.ResetTrigger("landTrigger");
            animator.ResetTrigger("landDeepTrigger");
            animator.SetTrigger("jumpTrigger");
            pogoAniamtor.ResetTrigger("pogo_landTrigger");
            pogoAniamtor.SetTrigger("pogo_jumpTrigger");
            rb.AddForce(Vector3.up * minForce, ForceMode.Impulse);
            force = minForce;
            canJump = false;
        }
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * rotationForce * Time.deltaTime;
    }
}
