using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodIndicatorController : MonoBehaviour {

    private Slider bloodIndicator;
    private Text bloodValueText;
    private int minValue = 0;
    private int maxValue = 10;

    private void Awake()
    {
        bloodIndicator = GameObject.FindGameObjectWithTag("BloodIndicator").GetComponent<Slider>();
        bloodValueText = bloodIndicator.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        bloodIndicator.value = maxValue;
        bloodValueText.text = bloodIndicator.value.ToString();
    }

    public void DecreaseBloodValue (int value)
    {
        if ((int)bloodIndicator.value - value > minValue)
        {
            bloodIndicator.value -= value;
            bloodValueText.text = bloodIndicator.value.ToString();
        }
        else
        {
            bloodIndicator.value = minValue;
            bloodValueText.text = bloodIndicator.value.ToString();
        }
    }

    public void IncreaseBloodValue (int value)
    {
        if ((int)bloodIndicator.value + value < maxValue)
        {
            bloodIndicator.value += value;
            bloodValueText.text = bloodIndicator.value.ToString();
        }
        else
        {
            bloodIndicator.value = maxValue;
            bloodValueText.text = bloodIndicator.value.ToString();
        }
    }

    public int GetCurrentBlood()
    {
        return (int)bloodIndicator.value;
    }

}
