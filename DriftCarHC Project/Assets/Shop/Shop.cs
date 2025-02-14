using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop/Car")]
public class Shop : ScriptableObject
{
    public Car[] cars;
    public CarController[] carControllers;
    public GameObject defaultCarModel;
    public CarController defaultCarController;

    public int CarsCount => cars.Length;

    public CarController GetCar(int index) => carControllers[index];

    public void BuyCar(int index)
    {
        carControllers[index].isBought = true;
        UpdateDefaultCar(index);
    }

    private void UpdateDefaultCar(int index)
    {
        // Update the default car's 3D model
        defaultCarModel = carControllers[index].gameObject;

        // Update the default car's controller stats
        defaultCarController.InitializeFromCar(cars[index]);
    }
}