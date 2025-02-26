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

    private void Start()
    {
        if (leftButton != null)
        {
            leftButton.onClick.AddListener(() => SetSteering(true, false));
        }

        if (rightButton != null)
        {
            rightButton.onClick.AddListener(() => SetSteering(false, true));
        }
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

    private void SetSteering(bool left, bool right)
    {
        steeringLeft = left;
        steeringRight = right;
        centeredScreenPosition = left ? new Vector2(-1, 0) : right ? new Vector2(1, 0) : Vector2.zero;
    }

    // Handle brake button press (start braking)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == brakeButton.gameObject)
        {
            SetBraking(true);
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
        Debug.Log("Braking: " + braking);
        TouchInput.braking = braking;
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            Debug.Log("Car found");
            car.SetBraking(braking);
        }
    }

}
