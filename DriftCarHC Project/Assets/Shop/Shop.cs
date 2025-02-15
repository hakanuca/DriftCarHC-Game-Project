using UnityEngine;

public class Shop : MonoBehaviour
{
    public CarData[] cars; 
    public CarManager carManager; 
    public int selectedCarIndex = 0; 

    private int[] boughtCars; 

    void Start()
    {
        if (cars.Length == 0)
        {
            return;
        }

        boughtCars = new int[cars.Length]; 
        LoadDefaultCar();
    }

    public bool IsCarBought(int index)
    {
        return boughtCars[index] == 1;
    }

    public void BuyCar(int index)
    {
        if (!IsCarBought(index))
        {
            boughtCars[index] = 1; 
            SelectCar(index);
        }
    }

    public void SelectCar(int index)
    {
        if (index < 0 || index >= cars.Length)
        {
            return;
        }

        selectedCarIndex = index;

        if (carManager == null)
        {
            return;
        }

        carManager.LoadCar(cars[selectedCarIndex]); // Yeni arabayı yükle
    }


    public void LoadDefaultCar()
    {
        if (cars.Length > 0)
        {
            selectedCarIndex = 0;
            carManager.LoadCar(cars[selectedCarIndex]);
        }
    }
}