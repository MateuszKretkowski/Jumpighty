using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.transform.position = new Vector3 (0, 1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
