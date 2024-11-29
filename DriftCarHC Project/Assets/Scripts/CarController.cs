using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    [SerializeField] float carSpeed = 10f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float steeringAngle = 30f;
    [SerializeField] float dragAmount = 0.95f;
    [SerializeField] float tractionForce = 1f;
    [SerializeField] float driftFactor = 0.9f;
    [SerializeField] float steeringSpeed = 2f; // Speed at which steering angle increases
    private float currentSteeringInput = 0f;

    Vector3 _moveVec;
    Vector3 _rotateVec;

    public Transform leftWheel, rightWheel;
    [SerializeField] Button leftButton, rightButton;

    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    void Start()
    {
        // Set up button events using UI Event Triggers
        EventTrigger leftTrigger = leftButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger rightTrigger = rightButton.gameObject.AddComponent<EventTrigger>();

        // Add events for left button
        EventTrigger.Entry leftPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        leftPointerDown.callback.AddListener((data) => { isLeftButtonPressed = true; isRightButtonPressed = false; });
        leftTrigger.triggers.Add(leftPointerDown);

        EventTrigger.Entry leftPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        leftPointerUp.callback.AddListener((data) => { isLeftButtonPressed = false; });
        leftTrigger.triggers.Add(leftPointerUp);

        // Add events for right button
        EventTrigger.Entry rightPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        rightPointerDown.callback.AddListener((data) => { isRightButtonPressed = true; isLeftButtonPressed = false; });
        rightTrigger.triggers.Add(rightPointerDown);

        EventTrigger.Entry rightPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        rightPointerUp.callback.AddListener((data) => { isRightButtonPressed = false; });
        rightTrigger.triggers.Add(rightPointerUp);
    }

    void Update()
    {
        ApplyTouchSteering();
        ApplyMovement();
    }

    void ApplyTouchSteering()
    {
        // Smoothly increment/decrement steering based on button press
        if (isLeftButtonPressed)
        {
            currentSteeringInput = Mathf.Clamp(currentSteeringInput - steeringSpeed * Time.deltaTime, -1f, 0f);
        }
        else if (isRightButtonPressed)
        {
            currentSteeringInput = Mathf.Clamp(currentSteeringInput + steeringSpeed * Time.deltaTime, 0f, 1f);
        }
        else
        {
            // Reset steering input when no button is pressed
            currentSteeringInput = Mathf.MoveTowards(currentSteeringInput, 0f, steeringSpeed * Time.deltaTime);
        }

        // Apply parabolic scaling for a more gradual steering control
        float parabolicInput = currentSteeringInput * Mathf.Abs(currentSteeringInput);

        // Calculate steering angle and drift effect
        float driftSteering = parabolicInput * steeringAngle * _moveVec.magnitude;
        Vector3 driftVec = Quaternion.AngleAxis(driftSteering * driftFactor * Time.deltaTime, Vector3.up) * _moveVec;

        transform.position += driftVec * Time.deltaTime;

        // Apply drag to the car's movement
        _moveVec *= Mathf.Pow(dragAmount, Time.deltaTime); // Adjust drag to be time-dependent
        _moveVec = Vector3.Lerp(_moveVec.normalized, transform.forward, tractionForce * Time.deltaTime) * _moveVec.magnitude;

        // Update wheels rotation based on the calculated steering
        _rotateVec.y = Mathf.Clamp(parabolicInput * steeringAngle, -steeringAngle, steeringAngle);
        transform.Rotate(Vector3.up * driftSteering * Time.deltaTime); // Adjust rotation speed with deltaTime
        leftWheel.localRotation = Quaternion.Euler(_rotateVec);
        rightWheel.localRotation = Quaternion.Euler(_rotateVec);
    }

    void ApplyMovement()
    {
        // Apply forward movement to the car
        _moveVec += transform.forward * carSpeed * Time.deltaTime;
        _moveVec = Vector3.ClampMagnitude(_moveVec, maxSpeed);
    }
}
