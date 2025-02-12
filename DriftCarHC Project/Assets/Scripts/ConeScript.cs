using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour

{
    public int scoreValue = 10;

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {

            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(scoreValue);
                Destroy(gameObject);
            }

        }

    }
}