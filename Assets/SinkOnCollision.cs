using System.Collections;
using UnityEngine;

public class SinkOnCollision : MonoBehaviour
{
    public float sinkDepth = 0.2f; // How much the leg should sink
    public float sinkSpeed = 2.0f; // Speed of sinking
    public bool isHands;

    private Vector3 originalPosition;
    private bool isSinking = false;

    void Start()
    {
    
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7 && !isSinking)
        {
            isSinking = true;
            originalPosition = transform.localPosition;
            StartCoroutine(SinkLeg());
        }
    }
    private IEnumerator SinkLeg()
    {
        Vector3 targetPosition;
        if (isHands)
        {
            targetPosition = originalPosition - new Vector3(0, sinkDepth, 0);
        }
        else
        {
            targetPosition = originalPosition + new Vector3(0, sinkDepth, 0);
        }
        bool isRaising = false;
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f && !isRaising)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * sinkSpeed);
            yield return null;
        }
        isRaising = true;
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f && isRaising)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * sinkSpeed);
            yield return null;
        }
        isSinking = false;
    }
}
