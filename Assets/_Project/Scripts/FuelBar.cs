using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public Slider fuelBar;  // Reference to the UI slider
    public float maxFuel = 100;
    public float currentFuel;

    // Update is called once per frame
    void Update()
    {
        fuelBar.value = currentFuel;
    }
    void Start()
    {
        currentFuel = maxFuel;
        fuelBar.maxValue = maxFuel;
        fuelBar.value = currentFuel;
    }
}