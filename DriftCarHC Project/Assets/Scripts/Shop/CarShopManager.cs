using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarShopManager : MonoBehaviour
{
    public List<GameObject> carPrefabs; // Assign in Inspector
    public Transform spawnPoint;        // Where cars appear
    public Button leftButton;
    public Button rightButton;

    private int currentIndex = 0;
    private GameObject currentCar;

    void Start()
    {
        // Hook up button clicks
        leftButton.onClick.AddListener(ShowPreviousCar);
        rightButton.onClick.AddListener(ShowNextCar);

        ShowCar(currentIndex);
    }

    void ShowCar(int index)
    {
        if (currentCar != null)
            Destroy(currentCar);

        // Spawn car with rotation adjustment
        Quaternion rotation = Quaternion.Euler(0, 180, 0); // Y ekseninde 180 derece döndür
        currentCar = Instantiate(carPrefabs[index], spawnPoint.position, rotation);
    }


    void ShowPreviousCar()
    {
        currentIndex = (currentIndex - 1 + carPrefabs.Count) % carPrefabs.Count;
        ShowCar(currentIndex);
    }

    void ShowNextCar()
    {
        currentIndex = (currentIndex + 1) % carPrefabs.Count;
        ShowCar(currentIndex);
    }
}
