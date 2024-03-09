using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private AudioSource gateSound;
    private bool addedScore = false;

    void OnTriggerEnter(Collider other)
    {
        if (addedScore)
        {
            return;
        }
        addedScore = true;
        gateSound.Play();
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.UpdateScore(5);
        }
    }
}
