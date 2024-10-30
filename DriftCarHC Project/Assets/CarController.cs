using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float carSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float steeringAngle;
    float dragAmount = 0.99f;
    [SerializeField] float TractionForce;
    Vector3 _moveVec;
    Vector3 _rotateVec;
    public Transform leftWheel, rightWheel;
    void Start()
    {
    }

    void Update()
    {
        _moveVec += transform.forward * carSpeed * Time.deltaTime;
        transform.position += _moveVec * Time.deltaTime;

        _rotateVec += new Vector3(0,Input.GetAxis("Horizontal"),0);
       
        transform.Rotate(Vector3.up*Input.GetAxis("Horizontal")*steeringAngle*Time.deltaTime*_moveVec.magnitude);
        
        
        _moveVec *= dragAmount;
        _moveVec = Vector3.ClampMagnitude(_moveVec, maxSpeed);
        _moveVec = Vector3.Lerp(_moveVec.normalized,transform.forward, TractionForce*Time.deltaTime)*_moveVec.magnitude;

        _rotateVec = Vector3.ClampMagnitude(_rotateVec, steeringAngle);

        leftWheel.localRotation = Quaternion.Euler(_rotateVec);
        rightWheel.localRotation = Quaternion.Euler(_rotateVec);

    }
}