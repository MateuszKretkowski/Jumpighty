using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public GameObject gameManager;
    GameManager gameManagerScript;
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManagerScript.checkpointName = gameObject.name;
            gameManagerScript.isCheckPointed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameManagerScript.isCheckPointed = false;
    }
}
