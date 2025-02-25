using UnityEngine;

public class CarShopTester : MonoBehaviour
{
    public Shop shop; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextCar();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousCar();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            BuySelectedCar();
        }
    }

    private void SelectNextCar()
    {
        if (shop.cars.Length == 0)
        {
            return;
        }

        int nextIndex = (shop.selectedCarIndex + 1) % shop.cars.Length;
        shop.SelectCar(nextIndex);
    }

    private void SelectPreviousCar()
    {
        if (shop.cars.Length == 0)
        {
            return;
        }

        int prevIndex = (shop.selectedCarIndex - 1 + shop.cars.Length) % shop.cars.Length;
        shop.SelectCar(prevIndex);
    }

    private void BuySelectedCar()
    {
        shop.BuyCar(shop.selectedCarIndex);
    }
}