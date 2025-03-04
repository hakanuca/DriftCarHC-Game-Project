using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Control")]
    public float throttle = 1;
    public float screenUse = 0.8f;  // How much of the screen to use for turning? max turn is when touch at screenUse of screen width
    [Header("Body")]
    public Transform centerOfMass;
    public Transform groundTrigger;
    public LayerMask wheelCollidables;
    public float drag = 1f;
    [Header("Engine")]
    public float driveForce = 1;
    [Header("Suspension and Steering")]
    public float activeVisualSteeringAngleEffect = 1;
    public float maxVisualSteeringSpeed = 1;
    public float maxVisualSteeringAngle = 30;
    public float maxAngularAcceleration = 30;    // degrees per second
    public List<Transform> steeringWheels;
    public List<Transform> driveWheels;
    private Rigidbody _rb;
    public float driftAngleThreshold = 10.0f;

    [Header("Drift Effects")]
    public List<TrailRenderer> tireTrails; // Tire trails for drifting effect
    public List<ParticleSystem> tireSmoke; // Tire smoke for drifting effect

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Point wheels
        float wheelAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        PointDriveWheelsAt(wheelAngle);
        
        // Enable/Disable trails based on drift
        HandleDriftEffects();
        HandleDriftSmoke();
    }

    private float GetRawDriftAngle() {
        if (! WheelsGrounded()) return 0;
        return Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
    }

    public float GetDriftAngle() {
        return GetRawDriftAngle();
    } 

    public bool IsFrontWheelDrift() {
        return Mathf.Abs(GetDriftAngle()) > maxVisualSteeringAngle;
    }

    // Checks the car is drifting or not
    public bool IsDrifting() { 
        return Mathf.Abs(GetDriftAngle()) > driftAngleThreshold;
    }

    void FixedUpdate()
    {
        // Body
        _rb.centerOfMass = centerOfMass.localPosition;  // Doing each frame allows it to be changed in inspector
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);

        // If rear wheels on ground
        if (WheelsGrounded())
        {
            // Engine
            _rb.AddForce(GetDriveDirection() * GetDriveForce());

            // Steering
            _rb.angularVelocity += -transform.up * GetSteeringAngularAcceleration() * Time.fixedDeltaTime;
        }
    }
    
    void PointDriveWheelsAt(float targetAngle)
    {
        foreach (Transform wheel in steeringWheels)
        {
            float currentAngle = wheel.localEulerAngles.y;
            float change = ((((targetAngle - currentAngle) % 360) + 540) % 360) - 180;
            float newAngle = currentAngle + change * Time.deltaTime * maxVisualSteeringSpeed;
            wheel.localEulerAngles = new Vector3(0, newAngle, 0);
        }
    }

    /// Are the drive wheels grounded
    /// Can the car accelerate?
    public bool WheelsGrounded()
    {
        return Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables).Length > 0;
    }

    /// How fast do we spin car?
    float GetSteeringAngularAcceleration()
    {
        return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
    }

    /// How much should we be turning?
    /// Between -1 and 1
    float GetSteering()
    {
        return TouchInput.steeringValue;
    }
    
    /// What way car pointing
    /// Is normalized
    Vector3 GetDriveDirection()
    {
        return _rb.transform.forward.normalized;
    }

    /// How many beans will the car push itself with
    /// in newtown
    float GetDriveForce()
    {
        return driveForce * throttle;
    }

    /// Magnitude of drag
    /// velocity squared times drag coefficient
    /// Uses overall velocity, doesn't care about what direction car pointing
    float GetDragForce()
    {
        return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
    }
    
    private void HandleDriftEffects()
    {
        bool drifting = IsDrifting();
        foreach (TrailRenderer trail in tireTrails)
        {
            trail.emitting = drifting;
        }
    }
    
    private void HandleDriftSmoke()
    {
        bool drifting = IsDrifting();
        foreach (ParticleSystem smoke in tireSmoke)
        {
            if (drifting)
            {
                if (!smoke.isPlaying)
                    smoke.Play();
            }
            else
            {
                if (smoke.isPlaying)
                    smoke.Stop();
            }
        }
    }
}
