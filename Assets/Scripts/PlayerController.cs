using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    public HeadCollider HeadCollider;
    public float moveSpeed = 10f;
    public float jumpForce = 15f;
    public float rotationSpeed = 100f;
    public Rigidbody[] bodyParts;
    public Transform cameraTransform;
    public GameObject armatureRoot;
    public bool isGrounded;

    private void Start()
    {
        bodyParts = armatureRoot.GetComponentsInChildren<Rigidbody>(true);
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera
        Vector3 movementDirection = cameraTransform.forward * moveVertical + cameraTransform.right * moveHorizontal;
        movementDirection.y = 0f; // Ignore vertical movement

        // Apply force to each body part
        foreach (Rigidbody rb in bodyParts)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            foreach (Rigidbody rb in bodyParts)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void Rotate()
    {
        // Get input for rotation
        float rotateHorizontal = Input.GetAxis("Horizontal");

        // Apply torque to the main body parts (e.g., torso or hips)
        foreach (Rigidbody rb in bodyParts)
        {
            Vector3 torque = Vector3.up * rotateHorizontal * rotationSpeed;
            rb.AddTorque(torque);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("GAMEOBJECT LAYER: " + collision.gameObject.layer);
        if (collision.gameObject.layer == 7)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isGrounded = false;
        }
    }
}
