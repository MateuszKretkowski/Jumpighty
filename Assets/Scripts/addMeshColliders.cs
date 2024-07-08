using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addMeshColliders : MonoBehaviour
{
    public GameObject houseProps;

    void Start()
    {
        if (houseProps != null)
        {
            foreach (Transform child in houseProps.transform)
            {
                if (child.GetComponent<MeshRenderer>() != null)
                {
                    if (child.GetComponent<MeshCollider>() == null)
                    {
                        child.gameObject.AddComponent<MeshCollider>();
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("houseProps nie jest ustawiony w inspektorze.");
        }
    }
}
