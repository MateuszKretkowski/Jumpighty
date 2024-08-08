using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class PlayerControllerPogo : MonoBehaviour
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

    [SerializeField] float rotationSpeed;
    [SerializeField] float downwardsForce;

    [SerializeField] float force;
    [SerializeField] float maxForce;
    [SerializeField] float minForce;

    public HeadCollider headCollider;

    [SerializeField] bool isGrounded;

    public GameObject land;
    public GameObject jump;
    public GameObject hit;
    public GameObject landing;
    public GameObject rotating;

    public ParticleSystem landPs;
    public ParticleSystem jumpPs;
    public ParticleSystem hitPs;
    public ParticleSystem landingPs;
    public ParticleSystem rotatingPs;

    void Start()
    {
        landPs = land.GetComponent<ParticleSystem>();
        hasRotated = true;
        rb = GetComponent<Rigidbody>();
        force = minForce;
    }

    void Update()
    {

        if (headCollider.isUnRagdolledLocal)
        {
            canJump = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!canJump && !headCollider.isRagDolled)
        {
            if (horizontalInput != 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(0f, 0f, -horizontalInput * rotationSpeed);
                transform.rotation = transform.rotation * deltaRotation;
            }

            if (verticalInput != 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(verticalInput * rotationSpeed, 0f, 0f);
                transform.rotation = transform.rotation * deltaRotation;
            }
        }

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

                Vector3 localUp = transform.TransformDirection(Vector3.up);
                rb.AddForce(localUp * force, ForceMode.Impulse);

                force = minForce;
                isPreparing = false;
                canJump = false;
            }
        }

        if (rb.velocity.y < 1)
        {
            rb.AddForce(Vector3.down * downwardsForce);
        }

        if (rotationTime >= rotationTimeMax)
        {
            hasRotated = true;
        } 
    }
    bool isInstantiated;
    ParticleSystem landingPart;
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            WaitAndPerformAction(0.1f);
        }
        if (!canJump && !hasRotated && !headCollider.isRagDolled)
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
        if (rb.velocity.y < -10f)
        {
            if (!isInstantiated)
            {
                landingPart = Instantiate(landingPs, transform.position, Quaternion.identity);
                landingPart.transform.SetParent(transform);
                landingPart.Play();
                isInstantiated = true;
            }
        }
        else
        {
            Destroy(landingPart.gameObject);
            // landingPs.Stop();
            isInstantiated = false;
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

            landPs.Play();

            isGrounded = true;
            canJump = true;
            hasRotated = false;
            rotationTime = 0f;
            StartCoroutine(JumpCaller());


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "default_obstacle")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "default_obstacle")
        {
            Debug.Log("triggerex");
            canJump = false;
            isGrounded = false;
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

            Vector3 localUp = transform.TransformDirection(Vector3.up);
            rb.AddForce(localUp * minForce, ForceMode.Impulse);
            
            force = minForce;
            canJump = false;
        }
    }
    private IEnumerator WaitAndPerformAction(float waitTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            if (!isGrounded)
            {
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PerformAction();
    }

    private void PerformAction()
    {
        Vector3 localUp = transform.TransformDirection(Vector3.up);
        rb.AddForce(localUp * minForce, ForceMode.Impulse);
    }
}
