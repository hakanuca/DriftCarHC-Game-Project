using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CarRespawn : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private CarController carController;

    [Header("Life System")]
    public int lives = 3;
    public Image[] lifeIcons;

    private bool isInvincible = false;
    public float invincibilityDuration = 5f;

    void Start()
    {
        gameOverScreen.SetActive(false);
        carController = GetComponent<CarController>();
        UpdateLifeUI();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Çarpışma oldu: " + other.name);
        if (other.CompareTag("Crashable") && !isInvincible)
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        lives--;
        UpdateLifeUI();

        if (lives <= 0)
        {
            ShowGameOverScreen();
            StopCar();
        }
        else
        {
            StartCoroutine(StartInvincibility());
        }
    }

    IEnumerator StartInvincibility()
    {
        isInvincible = true;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float elapsed = 0f;
        float blinkInterval = 0.2f;

        while (elapsed < invincibilityDuration)
        {

            foreach (Renderer r in renderers)
                r.enabled = false;

            yield return new WaitForSeconds(blinkInterval);


            foreach (Renderer r in renderers)
                r.enabled = true;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;
        }

        isInvincible = false;
    }

    public void RespawnCar()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        lives = 3;
        UpdateLifeUI();
        EnableCar();
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < lives;
        }
    }

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