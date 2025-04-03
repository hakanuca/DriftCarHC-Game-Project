using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public CarController carController; // Reference to the CarController script
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI TextMeshPro object
    public float scoreMultiplier = 1f; // Reduced multiplier for the score calculation

    private float driftStartTime;
    private bool isDrifting;
    private float totalScore;

    void Update()
    {
        if (carController.IsDrifting())
        {
            if (!isDrifting)
            {
                // Start drifting
                isDrifting = true;
                driftStartTime = Time.time;
            }
            else
            {
                // Calculate score based on drift duration and angle
                float driftDuration = Time.time - driftStartTime;
                float driftAngle = carController.GetDriftAngle();
                float score = CalculateScore(driftDuration, driftAngle);
                totalScore += score;
                scoreText.text = "Score: " + Mathf.FloorToInt(totalScore).ToString();
            }
        }
        else
        {
            if (isDrifting)
            {
                // Stop drifting
                isDrifting = false;
            }
        }
    }

    float CalculateScore(float duration, float angle)
    {
        // Reduce the score increment by dividing by a factor
        return (duration * Mathf.Abs(angle) * scoreMultiplier) / 10f;
    }

    public float GetTotalScore()
    {
        return totalScore;
    }
}