using UnityEngine;
using System.Collections;

public class FadeCanvas : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 5f; // Time in seconds

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component is missing!");
            return;
        }
        
        StartCoroutine(FadeOutCanvas());
    }

    IEnumerator FadeOutCanvas()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before fading

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f; // Ensure it's fully transparent
        canvasGroup.interactable = true; // Do not change this part, otherwise you cannot control the car
        canvasGroup.blocksRaycasts = true; // Do not change this part, otherwise you cannot control the car
    }
}
