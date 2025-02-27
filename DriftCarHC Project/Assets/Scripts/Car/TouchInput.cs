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

    // Handle brake button press (start braking)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == brakeButton.gameObject)
        {
            SetBraking(true);
        }

        if (eventData.pointerPress == leftButton.gameObject)
        {
            Debug.Log("Left button pressed");
            SetLeftSteering(true);
        }

        if (eventData.pointerPress == rightButton.gameObject)
        {
            Debug.Log("Right button pressed");
            SetRightSteering(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == brakeButton.gameObject)
        {
            SetBraking(false);
        }
    }

    public void SetBraking(bool braking)
    {
        TouchInput.braking = braking;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetBraking(braking);
        }
    }

    public void SetLeftSteering(bool steeringLeft)
    {
        Debug.Log("SetLeftSteering: " + steeringLeft);
        TouchInput.steeringLeft = steeringLeft;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetSteering(steeringLeft ? -1 : 0);
        }
    }
    
    public void SetRightSteering(bool steeringRight)
    {
        Debug.Log("SetRightSteering: " + steeringRight);
        TouchInput.steeringRight = steeringRight;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.SetSteering(steeringRight ? 1 : 0);
        }
    }

}
