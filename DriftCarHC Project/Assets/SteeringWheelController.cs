using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringWheelController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float maxSteeringAngle = 200f; // Maximum rotation angle of the steering wheel
    private float wheelAngle = 0f;
    private float wheelPrevAngle = 0f;
    private RectTransform wheelRectTransform;

    public float SteeringInput { get; private set; } // Output value to control the car

    void Start()
    {
        wheelRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Capture initial touch angle
        Vector2 pos = eventData.position - (Vector2)wheelRectTransform.position;
        wheelPrevAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calculate angle based on drag position
        Vector2 pos = eventData.position - (Vector2)wheelRectTransform.position;
        float currentAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(wheelPrevAngle, currentAngle);
        wheelPrevAngle = currentAngle;

        // Rotate within limits
        wheelAngle = Mathf.Clamp(wheelAngle + deltaAngle, -maxSteeringAngle, maxSteeringAngle);
        wheelRectTransform.localEulerAngles = new Vector3(0, 0, -wheelAngle); // Rotate steering wheel graphic

        // Normalize the input between -1 and 1
        SteeringInput = wheelAngle / maxSteeringAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset wheel angle when touch is released
        wheelAngle = 0f;
        wheelRectTransform.localEulerAngles = Vector3.zero;
        SteeringInput = 0f;
    }
}