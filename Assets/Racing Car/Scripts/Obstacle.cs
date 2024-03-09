using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            gameManager.GameOver();
        }
    }

}
