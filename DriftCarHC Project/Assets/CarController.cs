using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float carSpeed = 10f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float steeringAngle = 30f;
    [SerializeField] float dragAmount = 0.95f;
    [SerializeField] float tractionForce = 1f;
    [SerializeField] float driftFactor = 0.9f;
    Vector3 _moveVec;
    Vector3 _rotateVec;
    public Transform leftWheel, rightWheel;

    [SerializeField] SteeringWheelController steeringWheel; // Reference to Steering Wheel Controller

    void Start()
    {
    }

    void Update()
    {
        // Control acceleration and speed
        _moveVec += transform.forward * carSpeed * Time.deltaTime;
        _moveVec = Vector3.ClampMagnitude(_moveVec, maxSpeed);
        
        // Calculate drift and steering based on steering wheel input
        float steeringInput = steeringWheel.SteeringInput; // Get input from steering wheel
        float driftSteering = steeringInput * steeringAngle * Time.deltaTime * _moveVec.magnitude;

        // Adjust car's rotation during drift
        Vector3 driftVec = Quaternion.AngleAxis(driftSteering * driftFactor, Vector3.up) * _moveVec;
        transform.position += driftVec * Time.deltaTime;

        // Apply drag
        _moveVec *= dragAmount;

        // Limit speed for better control during drift
        _moveVec = Vector3.Lerp(_moveVec.normalized, transform.forward, tractionForce * Time.deltaTime) * _moveVec.magnitude;

        // Clamp steering angle for wheels
        _rotateVec.y = Mathf.Clamp(steeringInput * steeringAngle, -steeringAngle, steeringAngle);

        // Apply the rotation to the car and the wheels
        transform.Rotate(Vector3.up * driftSteering);
        leftWheel.localRotation = Quaternion.Euler(_rotateVec);
        rightWheel.localRotation = Quaternion.Euler(_rotateVec);
    }
}