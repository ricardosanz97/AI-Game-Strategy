using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodIndicatorController : MonoBehaviour {

    private Image bloodIndicator;
    private Text bloodValueText;
    private int minValue = 0;
    private int maxValue = 10;

    private void Awake()
    {
        bloodIndicator = GameObject.FindGameObjectWithTag("BloodIndicator").GetComponent<Image>();
        bloodValueText = bloodIndicator.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        bloodIndicator.fillAmount = 1f;
        bloodValueText.text = ((int)(bloodIndicator.fillAmount * maxValue)).ToString();
    }

    public void DecreaseBloodValue (int value)
    {
        float value2 = value / 10f;

        if (bloodIndicator.fillAmount - value2 > minValue)
        {
            bloodIndicator.fillAmount -= value2;
        }
        else
        {
            bloodIndicator.fillAmount = minValue;
        }
        bloodValueText.text = ((int)Mathf.Round(bloodIndicator.fillAmount * maxValue)).ToString();
    }

    public void IncreaseBloodValue (int value)
    {
        float value2 = value / 10f;

        if (bloodIndicator.fillAmount + value2 < maxValue)
        {
            bloodIndicator.fillAmount += value2;
        }
        else
        {
            bloodIndicator.fillAmount = 1f;
        }
        bloodValueText.text = ((int)Mathf.Round(bloodIndicator.fillAmount * maxValue)).ToString();
    }

    public int GetCurrentBlood()
    {
        return (int)Mathf.Round(bloodIndicator.fillAmount * maxValue);
    }
}
