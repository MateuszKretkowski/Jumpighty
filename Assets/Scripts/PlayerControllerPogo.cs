using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerPogo : MonoBehaviour
{
    private Rigidbody rb;
    public Animator animator;
    public Animator pogoAnimator;
    public ParticleSystem landPs;
    public ParticleSystem jumpPs;
    public Transform stickEndPoint;
    public HeadCollider headCollider;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isPreparing;
    [SerializeField] private bool canJump;
    [SerializeField] private bool isSticked;

    [SerializeField] private float rotationTimeMax;
    [SerializeField] private float rotationForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float downwardsForce;
    [SerializeField] private float force;
    [SerializeField] private float maxForce;
    [SerializeField] private float minForce;

    private float rotationTime;
    private bool hasRotated;
    private Color currentColor;
    private bool isInstantiated;
    private float horizontalInput;
    private float verticalInput;
    private ParticleSystem landingPart;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = minForce;
        hasRotated = true;
    }

    private void Update()
    {
        if (isSticked && canJump) rb.velocity = Vector3.zero;
        if (headCollider.isUnRagdolledLocal) canJump = false;

        HandleJumpInput();
        ApplyDownwardForce();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        if (isGrounded) StartCoroutine(WaitAndPerformAction(0.1f));

        if (isPreparing && canJump)
        {
            force = Mathf.Min(force + 1, maxForce);
            UpdateAnimatorForPreparation();
        }

        HandleFallAndLandingEffects();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            rb.velocity = Vector3.zero;
            TriggerLandingEffects(other);
            isGrounded = true;
            canJump = true;
            hasRotated = false;
            rotationTime = 0f;
            if (other.CompareTag("non_slippery")) isSticked = true;

            StartCoroutine(JumpCaller());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7) isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            canJump = false;
            isGrounded = false;
        }

        if (other.CompareTag("non_slippery")) isSticked = false;
    }

    private IEnumerator JumpCaller()
    {
        yield return new WaitForSeconds(0.2f);
        if (!isPreparing && canJump)
        {
            PerformJump();
        }
    }

    private IEnumerator WaitAndPerformAction(float waitTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            if (!isGrounded) yield break;
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

    private void HandleJumpInput()
    {
        if (Input.GetKey(KeyCode.Space) && canJump) isPreparing = true;

        if (isPreparing && canJump && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ResetRotationAndTriggers();
            PerformJump();
        }
    }

    private void ApplyDownwardForce()
    {
        if (rb.velocity.y < 1)
        {
            rb.freezeRotation = true;
            rb.AddForce(Vector3.down * downwardsForce);
        }
        else
        {
            rb.freezeRotation = false;
        }
    }

    private void HandleRotation()
    {
        if (rotationTime >= rotationTimeMax) hasRotated = true;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (!canJump && !headCollider.isRagDolled)
        {
            RotatePlayer();
        }
    }

    private void RotatePlayer()
    {
        if (horizontalInput != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(0f, 0f, -horizontalInput * rotationSpeed);
            transform.rotation *= deltaRotation;
        }

        if (verticalInput != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(verticalInput * rotationSpeed, 0f, 0f);
            transform.rotation *= deltaRotation;
        }
    }

    private void PerformJump()
    {
        ResetRotationAndTriggers();

        Vector3 localUp = transform.TransformDirection(Vector3.up);
        rb.AddForce(localUp * force, ForceMode.Impulse);

        PlayParticles(jumpPs);

        force = minForce;
        isPreparing = false;
        canJump = false;
        isSticked = false;
    }

    private void ResetRotationAndTriggers()
    {
        transform.rotation = Quaternion.identity;
        pogoAnimator.ResetTrigger("pogo_landTrigger");
        pogoAnimator.SetTrigger("pogo_jumpTrigger");
        animator.ResetTrigger("landTrigger");
        animator.ResetTrigger("landDeepTrigger");
        animator.SetTrigger("jumpTrigger");
    }

    private void UpdateAnimatorForPreparation()
    {
        animator.ResetTrigger("landTrigger");
        animator.SetTrigger("landDeepTrigger");

        pogoAnimator.ResetTrigger("pogo_jumpTrigger");
        pogoAnimator.SetTrigger("pogo_landTrigger");
    }

    private void HandleFallAndLandingEffects()
    {
        if (rb.velocity.y < -1) animator.SetTrigger("fallTrigger");
        if (rb.velocity.y < -10f && !isInstantiated)
        {
            isInstantiated = true;
        }
        else
        {
            isInstantiated = false;
        }
    }

    private void TriggerLandingEffects(Collider other)
    {
        pogoAnimator.ResetTrigger("pogo_jumpTrigger");
        pogoAnimator.SetTrigger("pogo_landTrigger");
        animator.ResetTrigger("fallTrigger");
        animator.SetTrigger("landTrigger");

        Renderer objectRenderer = other.GetComponent<MeshRenderer>();
        currentColor = objectRenderer.materials[0].color;

        PlayParticles(landPs);
    }

    private void PlayParticles(ParticleSystem particleSystem)
    {
        ParticleSystem particles = Instantiate(particleSystem, stickEndPoint.position, Quaternion.identity);
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = currentColor;

        particles.Play();
        Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
    }
}
