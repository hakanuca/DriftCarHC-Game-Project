using UnityEngine;

[CreateAssetMenu(fileName = "NewCar", menuName = "Shop/New Car")]
public class CarData : ScriptableObject
{
    public string carName;
    public GameObject carModelPrefab; 
    public float carSpeed;
    public float maxSpeed;
    public float steeringAngle;
    public float dragAmount; 
    public float tractionForce; 
    public float driftFactor; 
    public float steeringSpeed;
    public bool isBought;
    public float angleThreshold; 
    public int price; 
}