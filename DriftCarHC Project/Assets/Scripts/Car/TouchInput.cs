using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button leftButton;
    public Button rightButton;
    public Button brakeButton;

    public static bool steeringLeft = false;
    public static bool steeringRight = false;
    public static bool braking = false;
    public static bool touched = false;
    public static InputMode inputMode;
    public static Vector2 centeredScreenPosition = Vector2.zero;

    public enum InputMode
    {
        None,
        Touch
    }

    private void Update()
    {
        if (steeringLeft || steeringRight)
        {
            touched = true;
            inputMode = InputMode.Touch;
        }
        else
        {
            touched = false;
            inputMode = InputMode.None;
        }

        if (!braking)
        {
            CarController car = FindObjectOfType<CarController>();
            if (car != null)
            {
                car.SetBraking(false);
            }
        }
    }

    // Checks which button was pressed and sets the corresponding boolean to true
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == brakeButton.gameObject)
        {
            SetBraking(true);
        }

        if (eventData.pointerPress == leftButton.gameObject)
        {
            SetLeftSteering(true);
        }

        if (eventData.pointerPress == rightButton.gameObject)
        {
            SetRightSteering(true);
        }
    }

    // Checks which button was released and sets the corresponding boolean to false
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == brakeButton.gameObject)
        {
            SetBraking(false);
        }
    }

    // Sets the braking boolean to the given value
    public void SetBraking(bool braking)
    {
        TouchInput.braking = braking;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetBraking(braking);
        }
    }

    // Sets the steeringLeft boolean to the given value
    public void SetLeftSteering(bool steeringLeft)
    {
        TouchInput.steeringLeft = steeringLeft;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetSteering(steeringLeft ? -1 : 0);
        }
    }
    
    // Sets the steeringRight boolean to the given value
    public void SetRightSteering(bool steeringRight)
    {
        TouchInput.steeringRight = steeringRight;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetSteering(steeringRight ? 1 : 0);
        }
    }

}
