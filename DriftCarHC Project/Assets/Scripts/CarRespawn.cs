using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
        // ResetCollectables();
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
        //ResetCollectables();
    }
    /*
    void ResetCollectables()
    {

        foreach (GameObject obj in collectableObjects)
        {
            obj.SetActive(true);
        }
    }
    */
    void ShowGameOverScreen()
    {

        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverScreen.SetActive(false);
        RespawnCar();
    }
    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        gameOverScreen.SetActive(false);

    }


}