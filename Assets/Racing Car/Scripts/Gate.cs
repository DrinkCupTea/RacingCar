using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // Start is called before the first frame update
    private bool addedScore = false;
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (addedScore)
        {
            return;
        }
        addedScore = true;
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.UpdateScore(5);
            Destroy(gameObject);
        }
    }
}
