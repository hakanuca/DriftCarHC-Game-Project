using UnityEngine;
using UnityEngine.UI;

public class BlinkingEffect : MonoBehaviour
{
    public Image targetImage;
    public float blinkInterval = 0.5f;
    private bool isBlinking = false;

    private void Start()
    {
        targetImage.enabled = false; // Ensure the image is off by default
    }

    public void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(Blink());
        }
    }

    public void StopBlinking()
    {
        if (isBlinking)
        {
            isBlinking = false;
            targetImage.enabled = false; // Ensure the image is off when stopping
            StopCoroutine(Blink());
        }
    }

    private System.Collections.IEnumerator Blink()
    {
        while (isBlinking)
        {
            targetImage.enabled = !targetImage.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}