using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string checkpointName;
    public bool isCheckPointed;

    public PlayerController playerController;
    public HeadCollider headCollider;
    public GameObject pogo;
    void Start()
    {
        
    }

    void Update()
    {
        if (headCollider.isUnRagdolledLocal)
        {
            pogo.SetActive(false);
        }
        else
        {
            pogo.SetActive(true);
        }
    }
}
