using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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

    [Space(20f)] 
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private Outline itemOutline;
    
    public void SetItemPosition(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition += position;
    }
    
    public void SetCarImage(GameObject gameObject)
    {
        carFigure.gameObject.SetActive(true);
    }
    
    public void SetCarName(string name)
    {
        carName.text = name;
    }
    
    public void SetCarPrice(int price)
    {
        carPrice.text = price.ToString();
    }
    
    public void SetCarSpeedFill(float fillAmount)
    {
        carSpeedFill.fillAmount = fillAmount;
    }
    
    public void SetCarPowerFill(float fillAmount)
    {
        carPowerFill.fillAmount = fillAmount;
    }
    
    public void SetCarPriceButton(bool isInteractable)
    {
        carPurchaseButton.interactable = isInteractable;
    }

    public void SetCharacterAsPurchased()
    {
        carPurchaseButton.gameObject.SetActive(false);
        itemButton.interactable = true;
        
        itemImage.color = carSelectedColor;
    }

    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        carPurchaseButton.onClick.RemoveListener(() => action(itemIndex));
        carPurchaseButton.onClick.AddListener(() => action(itemIndex));
    }
    
    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {                   
        itemButton.interactable = true;
        itemButton.onClick.RemoveListener(() => action(itemIndex));
        itemButton.onClick.AddListener(() => action(itemIndex));
    }
    
    public void SelecItem()
    {
        itemOutline.enabled = false;
        itemImage.color = carSelectedColor;
        itemButton.interactable = true;
    }
}
