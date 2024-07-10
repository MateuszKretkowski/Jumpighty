using System.Collections;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;
    private PlayerController playerController;
    public float normalizer = 10000;
    public float force = 5f;
    public float returnTime = 0.5f;
    public float pushForce = 500f;

    private Vector3 initialPosition;
    private bool isGrounded = false;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Do nothing, just waiting for the key to be held down
        }
        if (Input.GetKey(KeyCode.Space) && playerController.force < 100)
        {
            Vector3 newLocalPosition = transform.localPosition;
            newLocalPosition.z += playerController.force / normalizer;
            transform.localPosition = newLocalPosition;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(ReturnHandToInitialPosition());
        }
    }
    private IEnumerator ReturnHandToInitialPosition()
    {
        float elapsedTime = 0;
        Vector3 currentPos = transform.localPosition;

        while (elapsedTime < returnTime)
        {
            transform.localPosition = Vector3.Lerp(currentPos, initialPosition, (elapsedTime / returnTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;
    }
}
