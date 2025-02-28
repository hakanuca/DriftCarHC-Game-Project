using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CarRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public GameObject[] collectableObjects;
    private List<GameObject> collectedObjects = new List<GameObject>();
    void Start()
    {

        gameOverScreen.SetActive(false);
        ResetCollectables();
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Crashable"))
        {

            ShowGameOverScreen();
        }
    }

    public void RespawnCar()
    {

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        score = 0;
        scoreText.text = "Score: " + score;
        ResetCollectables();
    }
    void ResetCollectables()
    {

        foreach (GameObject obj in collectableObjects)
        {
            obj.SetActive(true);
        }
    }
    void ShowGameOverScreen()
    {

        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {

        gameOverScreen.SetActive(false);
        RespawnCar();
    }


}