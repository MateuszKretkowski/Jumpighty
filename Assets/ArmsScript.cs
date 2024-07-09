using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsScript : MonoBehaviour
{

    public Transform LookAtTarget;

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(LookAtTarget); 
    }

}
