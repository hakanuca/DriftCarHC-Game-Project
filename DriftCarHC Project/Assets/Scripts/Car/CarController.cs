using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    #region Variables

    [SerializeField] private float carSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float steeringAngle;
    [SerializeField] private float dragAmount;
    [SerializeField] private float tractionForce;
    [SerializeField] private float driftFactor;
    [SerializeField] private float steeringSpeed;
    [SerializeField] private float angleThreshold = 90f; 

    #endregion
    
    public bool isBought;
    private float currentSteeringInput = 0f;

    private Vector3 _moveVec;
    private Vector3 _rotateVec;

    public Transform leftWheel, rightWheel;
    [SerializeField] private Button leftButton, rightButton;

    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    private void Start()
    {
        // Set up button events using UI Event Triggers
        EventTrigger leftTrigger = leftButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger rightTrigger = rightButton.gameObject.AddComponent<EventTrigger>();

        // Left button events
        EventTrigger.Entry leftPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        leftPointerDown.callback.AddListener((data) => { isLeftButtonPressed = true; isRightButtonPressed = false; });
        leftTrigger.triggers.Add(leftPointerDown);

        EventTrigger.Entry leftPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        leftPointerUp.callback.AddListener((data) => { isLeftButtonPressed = false; });
        leftTrigger.triggers.Add(leftPointerUp);

        // Right button events
        EventTrigger.Entry rightPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        rightPointerDown.callback.AddListener((data) => { isRightButtonPressed = true; isLeftButtonPressed = false; });
        rightTrigger.triggers.Add(rightPointerDown);

        EventTrigger.Entry rightPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        rightPointerUp.callback.AddListener((data) => { isRightButtonPressed = false; });
        rightTrigger.triggers.Add(rightPointerUp);
    }

    private void Update()
    {
        ApplyTouchSteering();
        ApplyMovement();
    }

    private void ApplyTouchSteering()
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

        // Apply parabolic scaling for smoother steering
        float parabolicInput = currentSteeringInput * Mathf.Abs(currentSteeringInput);

        // Calculate the current rotation angle
        float currentRotationAngle = transform.eulerAngles.y;
        if (currentRotationAngle > 180f) currentRotationAngle -= 360f; // Normalize angle to range [-180, 180]

        // Apply a scaling factor to make it harder to turn more at higher angles
        float angleFactor = Mathf.Clamp01(1f - Mathf.Abs(currentRotationAngle) / angleThreshold);
        parabolicInput *= angleFactor;

        // Dynamic drift factor adjustment based on speed
        float speedFactor = Mathf.Clamp01(_moveVec.magnitude / maxSpeed); // Normalize speed effect
        float driftSteering = parabolicInput * steeringAngle * _moveVec.magnitude * (driftFactor + speedFactor);

        // Lateral drift force
        Vector3 lateralDrift = transform.right * parabolicInput * _moveVec.magnitude * 0.15f;

        // Apply drift steering and movement
        Vector3 driftVec = Quaternion.AngleAxis(driftSteering * Time.deltaTime, Vector3.up) * _moveVec;
        driftVec += lateralDrift; // Apply additional sideways drift

        transform.position += driftVec * Time.deltaTime;

        // Apply time-dependent drag to maintain drift effect
        _moveVec *= Mathf.Pow(dragAmount, Time.deltaTime);
        _moveVec = Vector3.Lerp(_moveVec.normalized, transform.forward, tractionForce * _moveVec.magnitude * Time.deltaTime) * _moveVec.magnitude;

        // Update car rotation and wheel movement
        _rotateVec.y = Mathf.Clamp(parabolicInput * steeringAngle, -steeringAngle, steeringAngle);
        transform.Rotate(Vector3.up * driftSteering * Time.deltaTime);
        leftWheel.localRotation = Quaternion.Euler(_rotateVec);
        rightWheel.localRotation = Quaternion.Euler(_rotateVec);
    }

    private void ApplyMovement()
    {
        // Apply forward movement to the car
        _moveVec += transform.forward * carSpeed * Time.deltaTime;
        _moveVec = Vector3.ClampMagnitude(_moveVec, maxSpeed);
    }

    public void InitializeFromCar(Car car)
    {
        carSpeed = car.carSpeed;
        maxSpeed = car.maxSpeed;
        steeringAngle = car.steeringAngle;
        dragAmount = car.dragAmount;
        tractionForce = car.tractionForce;
        driftFactor = car.driftFactor;
        steeringSpeed = car.steeringSpeed;
        isBought = car.isBought;
    }
}