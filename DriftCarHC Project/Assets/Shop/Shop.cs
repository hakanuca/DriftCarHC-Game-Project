using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop/Car")]
public class Shop : ScriptableObject
{
    public Car[] cars;
    public CarController[] carControllers;
    public int CarsCount => cars.Length;
    public CarController GetCar(int index) => carControllers[index];
    public void BuyCar(int index) => carControllers[index].isBought = true;
}
