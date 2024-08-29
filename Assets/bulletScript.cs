using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public RagdollOnOff ragdollScript;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) 
        {
            ragdollScript.RagdollModeOn();
        }
    }
}
