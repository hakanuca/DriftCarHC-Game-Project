using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Transform spawnPoint;
    private GameObject currentCar;

    public void LoadCar(CarData car)
    {
        if (car == null)
        {
            return;
        }

        if (currentCar != null)
        {
            Destroy(currentCar);
        }

        currentCar = Instantiate(car.carModelPrefab, spawnPoint.position, Quaternion.identity);
    }

}