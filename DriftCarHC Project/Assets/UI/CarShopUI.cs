using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShopUI : MonoBehaviour
{
    [SerializeField] private Color carNotSelectedColor;
    [SerializeField] private Color carSelectedColor;
    
    [SerializeField] private GameObject carFigure;
    [SerializeField] private TMP_Text carName;
    [SerializeField] private TMP_Text carPrice;
    [SerializeField] private Image carSpeedFill;
    [SerializeField] private Image carPowerFill;
    [SerializeField] private Button carPurchaseButton;

    [SerializeField] private Shop shop; 
    
    private int selectedCarIndex = 0;

    private void Start()
    {
        LoadCarData(selectedCarIndex);
    }

    public void LoadCarData(int carIndex)
    {
        selectedCarIndex = carIndex;
        CarData car = shop.cars[carIndex];

        SetCarName(car.carName);
        SetCarPrice(car.price);
        SetCarSpeedFill(car.carSpeed / car.maxSpeed);
        SetCarPowerFill(car.tractionForce);

        if (shop.IsCarBought(carIndex))
        {
            SetCharacterAsPurchased();
        }
        else
        {
            SetCarPriceButton(true);
        }
    }

    public void BuyCar()
    {
        if (!shop.IsCarBought(selectedCarIndex))
        {
            shop.BuyCar(selectedCarIndex);
            SetCharacterAsPurchased();
        }
    }

    public void SelectCar()
    {
        if (shop.IsCarBought(selectedCarIndex))
        {
            shop.SelectCar(selectedCarIndex);
            LoadCarData(selectedCarIndex);
        }
    }

    public void SetCarName(string name) => carName.text = name;
    public void SetCarPrice(int price) => carPrice.text = price.ToString();
    public void SetCarSpeedFill(float fillAmount) => carSpeedFill.fillAmount = fillAmount;
    public void SetCarPowerFill(float fillAmount) => carPowerFill.fillAmount = fillAmount;
    public void SetCarPriceButton(bool isInteractable) => carPurchaseButton.interactable = isInteractable;

    public void SetCharacterAsPurchased()
    {
        carPurchaseButton.gameObject.SetActive(false);
    }
}
