using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    [SerializeField] bool isSticked;

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

    public ParticleSystem landPs;
    public ParticleSystem landingPs;

    public ParticleSystem jumpPs;

    public Transform stickEndPoint;
    public Transform centerPoint;

    void Start()
    {
        hasRotated = true;
        force = minForce;
    }

    void Update()
    {
        if (headCollider.isUnRagdolledLocal)
        {
            canJump = false;
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

                ParticleSystem jumpParticles = Instantiate(jumpPs, stickEndPoint.position, Quaternion.identity);

                ParticleSystemRenderer renderer = jumpParticles.GetComponent<ParticleSystemRenderer>();
                renderer.material.color = currentColor;

                jumpParticles.Play();
                Destroy(jumpParticles.gameObject, jumpParticles.main.duration + jumpParticles.main.startLifetime.constantMax);

                force = minForce;
                isPreparing = false;
                canJump = false;
                isSticked = false;
            }
        }

        if (rotationTime >= rotationTimeMax)
        {
            hasRotated = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (!headCollider.isRagDolled)
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

            // THAT IS A HARD MODE'S ROTATION MODEL; IT IS REALLY HARD.

            // float torqueZ = -horizontalInput * rotationSpeed;
            // float torqueX = verticalInput * rotationSpeed;

            // rb.AddTorque(transform.right * torqueX + transform.forward * torqueZ);
        }
    }
    float horizontalInput;
    float verticalInput;
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
    }
    Color currentColor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("Object has Entered Ground");
            Debug.Log("triggerenter");

            pogoAniamtor.ResetTrigger("pogo_jumpTrigger");
            pogoAniamtor.SetTrigger("pogo_landTrigger");

            animator.ResetTrigger("fallTrigger");
            animator.SetTrigger("landTrigger");

            ParticleSystem landParticles = Instantiate(landPs, stickEndPoint.position, Quaternion.identity);

            Renderer objectRenderer = other.GetComponent<MeshRenderer>();
            currentColor = objectRenderer.materials[0].color;

            ParticleSystemRenderer renderer = landParticles.GetComponent<ParticleSystemRenderer>();
            renderer.material.color = currentColor;

            Debug.Log("Color of the object: " + currentColor);

            landParticles.Play();

            Destroy(landParticles.gameObject, landParticles.main.duration + landParticles.main.startLifetime.constantMax);

            isGrounded = true;
            canJump = true;
            if (other.gameObject.tag == "non_slippery")
            {
                isSticked = true;
            }
            hasRotated = false;
            rotationTime = 0f;
            StartCoroutine(JumpCaller());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            canJump = false;
            isGrounded = false;
            float rotationSpeed = 1f / 1f;
            gameObject.transform.SetParent(null);
            // transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, transform.rotation.y, 0, 0), 0 * Time.deltaTime);
            Debug.Log("Object has Left Ground");
        }
        if (other.gameObject.tag == "non_slippery")
        {
            isSticked = true;
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

            canJump = false;
            isSticked = false;
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

    }
}