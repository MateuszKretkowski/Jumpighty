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
            // Pobierz wszystkie dzieci obiektu houseProps
            foreach (Transform child in houseProps.transform)
            {
                // SprawdŸ, czy obiekt ma komponent MeshRenderer (aby upewniæ siê, ¿e to jest obiekt 3D)
                if (child.GetComponent<MeshRenderer>() != null)
                {
                    // SprawdŸ, czy obiekt nie ma jeszcze komponentu MeshCollider
                    if (child.GetComponent<MeshCollider>() == null)
                    {
                        // Dodaj komponent MeshCollider do obiektu
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
