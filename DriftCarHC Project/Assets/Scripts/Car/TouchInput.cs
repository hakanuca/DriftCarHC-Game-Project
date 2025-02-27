using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Left, Right, Brake }
    public ButtonType buttonType;
    public static float steeringValue = 0f; // -1 (left) to 1 (right)
    public static bool braking = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Left)
        {
            TouchInput.steeringValue = -1f; // Move left
        }
        else if (buttonType == ButtonType.Right)
        {
            TouchInput.steeringValue = 1f; // Move right
        }
        /*else if (buttonType == ButtonType.Brake)
        {
            TouchInput.braking = true; 
        }*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TouchInput.steeringValue = 0f; // Stop movement when released
    }
}