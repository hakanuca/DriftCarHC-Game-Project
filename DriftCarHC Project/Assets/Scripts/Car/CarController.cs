using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Control")]
    public float throttle = 1;
    public float screenUse = 0.8f;
    public float brakeForce = 1000f;
    private bool isBraking = false;
    private bool isReversing = false; 

    [Header("Body")]
    public Transform centerOfMass;
    public Transform groundTrigger;
    public LayerMask wheelCollidables;
    public float drag = 0.5f;

    [Header("Engine")]
    public float driveForce = 500f;
    public float maxSpeed = 50f;

    [Header("Suspension and Steering")]
    public float activeVisualSteeringAngleEffect = 1;
    public float maxVisualSteeringSpeed = 1;
    public float maxVisualSteeringAngle = 30;
    public float maxAngularAcceleration = 30;
    public List<Transform> steeringWheels;
    public List<Transform> driveWheels;

    private Rigidbody _rb;
    public float driftAngleThreshold = 10.0f;
    private float  reverseForce = 1000f; // don't change the variable!

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.isKinematic = false;
        
        foreach (Transform wheel in steeringWheels)
        {
            Debug.Log($"[Check] Steering Wheel Assigned: {wheel.name}");
        }
    }

    private void Update()
    {
        float steeringInput = GetSteering();
        float targetAngle = steeringInput * maxVisualSteeringAngle;

        Debug.Log($"[Update] Steering Input: {steeringInput}, Target Angle: {targetAngle}");

        // Fix: Ensure we pass the correct steering input
        PointDriveWheelsAt(targetAngle);
        
        foreach (Transform wheel in steeringWheels)
        {
            Debug.Log($"[Wheel Rotation Check] {wheel.name} Rotation: {wheel.localEulerAngles}");
        }
    }


    public void SetBraking(bool braking)
    {
        isBraking = braking;
        if (isBraking)
        {
            _rb.drag = 5f; // Increase drag to slow down the car
        }
        else
        {
            _rb.drag = drag; // Reset drag to normal value
        }
    }

    private void FixedUpdate()
    {
        _rb.centerOfMass = centerOfMass.localPosition;

        // Determine if the car is reversing based on velocity and direction
        isReversing = Vector3.Dot(_rb.velocity, transform.forward) < -0.1f;

        if (TouchInput.braking)
        {
            _rb.drag = 2f; // Reduce drag slightly for smooth deceleration
            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.02f); // Smooth velocity decrease

            // If the car is slow enough OR already in reverse, allow reversing
            if (_rb.velocity.magnitude < 1.0f || isReversing)
            {
                Vector3 reverseDir = -GetDriveDirection();
                float reverseBoost = 2.0f;
                _rb.AddForce(reverseDir * reverseForce * reverseBoost * Time.fixedDeltaTime, ForceMode.Force);
            }
        }
        else
        {
            _rb.drag = drag; // Reset drag when not braking

            // Allow normal driving only if not reversing
            if (WheelsGrounded() && !isReversing)
            {
                float drivePower = GetDriveForce();
                if (_rb.velocity.magnitude < maxSpeed) 
                {
                    _rb.AddForce(GetDriveDirection() * drivePower, ForceMode.Force);
                }

                // Ensure steering actually affects the car!
                float steering = GetSteering();
                float turnStrength = 1.5f; // Adjust this if turning is too weak
                _rb.angularVelocity += -transform.up * (steering * turnStrength) * Time.fixedDeltaTime;
            }
        }
    }
    
    private void PointDriveWheelsAt(float targetAngle)
    {
        foreach (Transform wheel in steeringWheels)
        {
            Vector3 wheelEuler = wheel.localEulerAngles;
            float currentAngle = wheelEuler.y;

            // Normalize to -180 to +180
            if (currentAngle > 180) currentAngle -= 360;

            // Move towards the target smoothly
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxVisualSteeringSpeed * Time.deltaTime * 500);

            Debug.Log($"[PointDriveWheelsAt] Wheel: {wheel.name}, Current Angle: {currentAngle}, Target Angle: {targetAngle}, New Angle: {newAngle}");

            // Force the rotation update
            wheel.localEulerAngles = new Vector3(0, newAngle, 0);
        }
    }
    
    public void SetSteering(float steeringInput)
    {
        float targetAngle = steeringInput * maxVisualSteeringAngle;
        Debug.Log($"[SetSteering] Steering Input: {steeringInput}, Target Angle: {targetAngle}");

        // Fix: Call GetSteering() directly to avoid unexpected behavior
        PointDriveWheelsAt(GetSteering() * maxVisualSteeringAngle);
    }
    
    public bool WheelsGrounded()
    {
        Collider[] colliders = Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables);
        return colliders.Length > 0;
    }

    float GetSteeringAngularAcceleration()
    {
        return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
    }

    float GetSteering()
    {
        float steering = 0;

        // Directly check if left or right is pressed
        if (TouchInput.steeringLeft)
        {
            steering = -1; // Turn left
        }
        else if (TouchInput.steeringRight)
        {
            steering = 1; // Turn right
        }
        else
        {
            // Smooth return to center when no input
            steering = Mathf.Lerp(steering, 0, Time.deltaTime * 5f);
        }

        Debug.Log($"[GetSteering] Steering: {steering}");
        return steering;
    }

    Vector3 GetDriveDirection()
    {
        return transform.forward.normalized;
    }

    float GetDriveForce()
    {
        return driveForce * throttle;
    }
}
