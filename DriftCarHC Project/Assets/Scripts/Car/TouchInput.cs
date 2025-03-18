using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Left, Right, Brake }
    public ButtonType buttonType;
    public static float steeringValue = 0f; // -1 (left) to 1 (right)
    public static bool braking = false;

    private BlinkingEffect blinkingEffect;

    private void Start()
    {
        blinkingEffect = FindObjectOfType<BlinkingEffect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Left)
        {
            TouchInput.steeringValue = -1f;
        }
        else if (buttonType == ButtonType.Right)
        {
            TouchInput.steeringValue = 1f;
        }
        else if (buttonType == ButtonType.Brake)
        {
            TouchInput.braking = true;
            blinkingEffect?.StartBlinking();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Brake)
        {
            TouchInput.braking = false;
            blinkingEffect?.StopBlinking();
        }
        else
        {
            TouchInput.steeringValue = 0f;
        }
    }
}