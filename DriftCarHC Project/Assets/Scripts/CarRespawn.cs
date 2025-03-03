using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class CarRespawn : MonoBehaviour
{
    // public Transform respawnPoint;
    private Rigidbody rb;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public GameObject[] collectableObjects;
    private List<GameObject> collectedObjects = new List<GameObject>();
    private CarController carController;
    void Start()
    {

        gameOverScreen.SetActive(false);
        carController = GetComponent<CarController>();
        // ResetCollectables();
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Crashable"))
        {

            ShowGameOverScreen();
            StopCar();
        }
    }

    public void RespawnCar()
    {

        //  transform.position = respawnPoint.position;
        //  transform.rotation = respawnPoint.rotation;
        score = 0;
        scoreText.text = "Score: " + score;
        EnableCar();
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

        AudioManager.instance.RestartMusic();

    }
    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        gameOverScreen.SetActive(false);

    }

    void StopCar()
    {

        if (carController != null)
        {
            carController.enabled = false;
        }
    }

    void EnableCar()
    {

        if (carController != null)
        {
            carController.enabled = true;
        }
    }
}