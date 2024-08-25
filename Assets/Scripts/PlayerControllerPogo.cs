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

    float rotationSpeedess = 1f / 1f;

    Quaternion maxRotation = Quaternion.Euler(90, 0f, 90);
    Quaternion minRotation = Quaternion.Euler(-90, 0f, -90);

    void Start()
    {
        hasRotated = true;
        rb = GetComponent<Rigidbody>();
        force = minForce;
    }

    Quaternion rotations;
    void Update()
    {
        if (!canJump && !headCollider.isRagDolled)
        {
            if (Input.GetKeyDown(KeyCode.W) && transform.rotation.x < maxRotation.x)
            {
                Debug.Log("SADasdasdasasdasdfwrg");
                rotations = Quaternion.Euler(transform.rotation.eulerAngles.x + 20f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            if (Input.GetKeyDown(KeyCode.A) && transform.rotation.z < maxRotation.z)
            {
                Debug.Log("SADasdasdasasdasdfwrg");
                rotations = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 20);
            }
            if (Input.GetKeyDown(KeyCode.S) && transform.rotation.x > minRotation.x)
            {
                Debug.Log("SADasdasdasasdasdfwrg");
                rotations = Quaternion.Euler(transform.rotation.eulerAngles.x - 20f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            if (Input.GetKeyDown(KeyCode.D) && transform.rotation.z > minRotation.z)
            {
                Debug.Log("SADasdasdasasdasdfwrg");
                rotations = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 20);
            }

            // Zachowaj bie��cy k�t Y
            float currentY = transform.rotation.eulerAngles.y;

            // Utw�rz now� rotacj� tylko na podstawie osi X i Z
            Quaternion targetRotations = Quaternion.Euler(rotations.eulerAngles.x, currentY, rotations.eulerAngles.z);

            // Interpoluj rotacj� tylko na osiach X i Z
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotations, 15f * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);


            // THAT IS A HARD MODE'S ROTATION MODEL; IT IS REALLY HARD.

            // float torqueZ = -horizontalInput * rotationSpeed;
            // float torqueX = verticalInput * rotationSpeed;

            // rb.AddTorque(transform.right * torqueX + transform.forward * torqueZ);
        }

        if (isSticked && canJump)
        {
            rb.velocity = Vector3.zero;
        }

        if (headCollider.isUnRagdolledLocal)
        {
            canJump = false;
        }

        if (Input.GetKey(KeyCode.Space) && canJump) isPreparing = true;

        if (isPreparing && canJump)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                transform.rotation = Quaternion.EulerRotation(0f, 0f, 0f);
                pogoAniamtor.ResetTrigger("pogo_landTrigger");
                pogoAniamtor.SetTrigger("pogo_jumpTrigger");

                animator.ResetTrigger("landTrigger");
                animator.ResetTrigger("landDeepTrigger");
                animator.SetTrigger("jumpTrigger");

                Vector3 localUp = transform.TransformDirection(Vector3.up);
                rb.AddForce(localUp * force, ForceMode.Impulse);

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
            if (!Input.GetKey(KeyCode.Space))
            {
                pogoAniamtor.ResetTrigger("pogo_landTrigger");
                pogoAniamtor.SetTrigger("pogo_jumpTrigger");

                animator.ResetTrigger("landTrigger");
                animator.ResetTrigger("landDeepTrigger");
                animator.SetTrigger("jumpTrigger");

                Vector3 localUp = transform.TransformDirection(Vector3.up);
                rb.AddForce(localUp * force, ForceMode.Impulse);

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

        if (rb.velocity.y < 1)
        {
            rb.freezeRotation = true;
            rb.AddForce(Vector3.down * downwardsForce);
        }
        else
        {
            rb.freezeRotation = false;
        }

        if (rotationTime >= rotationTimeMax)
        {
            hasRotated = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

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
                // landingPart = Instantiate(landingPs, transform.position, Quaternion.identity);
                // landingPart.transform.SetParent(transform);
                // landingPart.Play();
                isInstantiated = true;
            }
        }
        else
        {
            // Destroy(landingPart.gameObject);
            // landingPs.Stop();
            isInstantiated = false;
        }
    }
    Color currentColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
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
    Quaternion targetRotation;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            canJump = false;
            isGrounded = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, transform.rotation.y, 0, 0), 0 * Time.deltaTime);

            Transform targetTransform = rotationObject.transform;
            Vector3 direction = targetTransform.position - transform.position;
            targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 2.0f;
            rotations = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            Debug.Log(targetRotation);
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

            Vector3 localUp = transform.TransformDirection(Vector3.up);
            rb.AddForce(localUp * minForce, ForceMode.Impulse);


            force = minForce;
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

        PerformAction();
    }

    private void PerformAction()
    {
        Vector3 localUp = transform.TransformDirection(Vector3.up);
        rb.AddForce(localUp * minForce, ForceMode.Impulse);
    }
}