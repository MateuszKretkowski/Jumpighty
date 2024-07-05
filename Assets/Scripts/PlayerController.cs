using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody rb;
    public BoxCollider groundCheck;
    public bool isPreparing;
    [SerializeField] private bool canJump;

    [SerializeField] private float force;
    [SerializeField] private float maxForce = 100f;

    void Start()
    {
        isPreparing = false;
        canJump = true;
        groundCheck.isTrigger = true;
        force = 0f;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (canJump)
        {
            if (force != 0f && !isPreparing)
            {
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
                force = 0f;
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
