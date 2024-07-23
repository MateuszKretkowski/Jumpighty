using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pointToJump;

    public Rigidbody rb;
    public BoxCollider groundCheck;
    public bool isPreparing;
    public float landingForce = 5f;
    [SerializeField] private bool canJump;

    [SerializeField] public float force;
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 60f;
    // [SerializeField] private float delayMax = 0.5f;
    // [SerializeField] private float delay;

    HeadCollider headCollider;
    public GameObject headColliderObject;

    FollowTarget followTarget;

    void Start()
    {
        // delay = 0f;
        isPreparing = false;
        canJump = true;
        groundCheck.isTrigger = true;
        force = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        headCollider = headColliderObject.GetComponent<HeadCollider>();
        followTarget = gameObject.GetComponent<FollowTarget>();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isPreparing = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canJump = true;
            rb.AddForce(Vector3.down * landingForce, ForceMode.Impulse);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

//    private void OnTriggerEnter(Collider other)
//    {
//       if (other.gameObject.layer == 0)
//        {
//            Debug.Log("Collision!");
//            rb.AddForce(Vector3.back * force, ForceMode.Impulse);
//        }
//    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        // if (delay > 0f)
        // {
            //    delay -= Time.deltaTime;
        // }
        transform.LookAt(pointToJump);
        if (canJump)
        {
            if (force > minForce && !isPreparing)
            {
                Vector3 forward = transform.forward;
                Vector3 up = transform.up;
                rb.AddForce(up * force, ForceMode.Impulse);
                rb.AddForce(forward * force, ForceMode.Impulse);
                // delay = delayMax;
                force = minForce;
            }


            if (Input.GetKey(KeyCode.Space))
            {
                isPreparing = true;
                if (force < maxForce)
                {
                    force += 1f;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isPreparing = false;
            }
        }
            
    }
}
