using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region Variables

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
    private Rigidbody _rb;
    public float driftAngleThreshold = 10.0f;
    [SerializeField] private float wheelRotationScalingFactor = 2.0f; 


    [Header("Drift Effects")]
    public List<TrailRenderer> tireTrails; // Tire trails for drifting effect
    public List<ParticleSystem> tireSmoke; // Tire smoke for drifting effect

    [Header("Braking")]
    [SerializeField] private float reverseMultiplier = 0.5f; 
    [SerializeField] private float brakingForceMultiplier = 10f; // Tweak this value
    private bool isReversing = false;

    #endregion

    #region UnityFunctions

    void Start()
    {
        // Set the frame rate to match the phone's refresh rate
        Application.targetFrameRate = 120;
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Point wheels
        float wheelAngle = Vector3.Angle(_rb.velocity, GetDriveDirection()) * Vector3.Cross(_rb.velocity, GetDriveDirection()).y * wheelRotationScalingFactor;       
        wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        PointDriveWheelsAt(wheelAngle);
        
        // Enable/Disable trails based on drift
        HandleDriftEffects();
        HandleDriftSmoke();
    }
    
    void FixedUpdate()
    {
        // Body
        _rb.centerOfMass = centerOfMass.localPosition;  // Doing each frame allows it to be changed in inspector
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);
        
        HandleBraking(); // Call the braking system
        
        if (TouchInput.braking || isReversing) return; // Prevent forward movement when braking or reversing

        // If rear wheels on ground
        if (WheelsGrounded())
        {
            // Engine
            _rb.AddForce(GetDriveDirection() * GetDriveForce());
            
            // Steering
            _rb.angularVelocity += -transform.up * GetSteeringAngularAcceleration() * Time.fixedDeltaTime;
        }
    }

#endregion

    #region CarControlFunctions

    private void HandleBraking()
    {
        float speed = _rb.velocity.magnitude;

        if (TouchInput.braking)
        {
            if (speed > 0.5f && !isReversing)
            {
                // Apply braking force while moving forward
                Vector3 brakeForce = -_rb.velocity.normalized * brakingForceMultiplier * _rb.mass;
                _rb.AddForce(brakeForce);
            }
            else
            {
                // If already slow, go into reverse mode immediately
                isReversing = true;

                // Apply reverse force
                Vector3 reverseForce = -GetDriveDirection() * GetDriveForce() * reverseMultiplier * _rb.mass;
                _rb.AddForce(reverseForce);
            }
        }
        else
        {
            // Reset reverse mode when brake is released
            isReversing = false;
        }
    }
    
    private float GetRawDriftAngle() 
    {
        if (!WheelsGrounded()) return 0;
        return Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
    }

    public float GetDriftAngle() 
    {
        return GetRawDriftAngle();
    } 

    public bool IsDrifting() 
    { 
        return Mathf.Abs(GetDriftAngle()) > driftAngleThreshold;
    }
    
    private void PointDriveWheelsAt(float targetAngle)
    {
        if (IsDrifting())
        {
            targetAngle = -targetAngle; 
        }

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
        return Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables).Length > 0;
    }

    float GetSteeringAngularAcceleration()
    {
        return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
    }

    float GetSteering()
    {
        return TouchInput.steeringValue;
    }
    
    Vector3 GetDriveDirection()
    {
        return _rb.transform.forward.normalized;
    }

    float GetDriveForce()
    {
        return driveForce * throttle;
    }

    float GetDragForce()
    {
        return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
    }

#endregion

    #region VFXFunctions

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

    #endregion
}
