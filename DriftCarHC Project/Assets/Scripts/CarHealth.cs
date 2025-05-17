using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CarHealth : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;
    private bool isInvincible = false;
    public float invincibilityTime = 5f;

    public GameObject gameOverScreen;
    public TextMeshProUGUI livesText;

    private void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
        gameOverScreen.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Crashable") && !isInvincible)
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        if (currentLives <= 0)
            return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(StartInvincibility());
        }
    }

    IEnumerator StartInvincibility()
    {
        isInvincible = true;

        float elapsed = 0f;
        float blinkRate = 0.2f;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        while (elapsed < invincibilityTime)
        {
            foreach (Renderer r in renderers)
                r.enabled = false;

            yield return new WaitForSeconds(blinkRate);

            foreach (Renderer r in renderers)
                r.enabled = true;

            yield return new WaitForSeconds(blinkRate);
            elapsed += blinkRate * 2;
        }

        isInvincible = false;
    }

    void UpdateLivesUI()
    {
        livesText.text = "Lives: " + currentLives;
    }

    void Die()
    {

        gameOverScreen.SetActive(true);


        livesText.gameObject.SetActive(false);

        /*
                CarController controller = GetComponentInChildren<CarController>();
                if (controller != null) controller.enabled = false;

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
        */
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}