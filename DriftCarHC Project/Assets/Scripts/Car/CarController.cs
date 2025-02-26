using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Control")]
    public float throttle = 1;
    public float screenUse = 0.8f;
    public float brakeForce = 1000f;
    private bool isBraking = false;

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

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.isKinematic = false;
    }

    void Update()
    {
        float wheelAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        PointDriveWheelsAt(wheelAngle);
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

        Debug.Log("Velocity: " + _rb.velocity.magnitude);

        if (TouchInput.braking)
        {
            Debug.Log("Braking2: " + TouchInput.braking);
            _rb.drag = 5f; // Apply strong braking effect
            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.02f); // Smoothly decrease velocity
        }
        else
        {
            _rb.drag = drag; // Reset drag when not braking

            if (WheelsGrounded())
            {
                float drivePower = GetDriveForce();
                _rb.AddForce(GetDriveDirection() * drivePower, ForceMode.Force);
                _rb.angularVelocity += -transform.up * GetSteeringAngularAcceleration() * Time.fixedDeltaTime;
            }
        }
    }


    private void PointDriveWheelsAt(float targetAngle)
    {
        foreach (Transform wheel in steeringWheels)
        {
            float currentAngle = wheel.localEulerAngles.y;
            float change = ((((targetAngle - currentAngle) % 360) + 540) % 360) - 180;
            float newAngle = currentAngle + change * Time.deltaTime * maxVisualSteeringSpeed;
            wheel.localEulerAngles = new Vector3(0, newAngle, 0);
        }
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
        return Mathf.Clamp(TouchInput.centeredScreenPosition.x / screenUse, -1, 1);
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