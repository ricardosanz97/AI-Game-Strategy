using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodController : MonoBehaviour {

    private Image bloodPlayerIndicator;
    private Text bloodPlayerValueText;

    public int minValue = 0;
    public int maxValue = 100;

    public int PlayerBlood;
    public int AIBlood;

    private void Awake()
    {
        bloodPlayerIndicator = GameObject.FindGameObjectWithTag("BloodIndicator").GetComponent<Image>();
        bloodPlayerValueText = bloodPlayerIndicator.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        PlayerBlood = maxValue;
        AIBlood = maxValue;

        bloodPlayerIndicator.fillAmount = PlayerBlood/maxValue;
        bloodPlayerValueText.text = PlayerBlood.ToString();
    }

    public void DecreasePlayerBloodValue (int value)
    {
        //Debug.Log("Tenemos " + PlayerBlood);
        //Debug.Log("Restamos " + value);
        float value2 = value / (float)maxValue;

        //Debug.Log(PlayerBlood + " / " +  maxValue + " - " + value2 + " > " + minValue + " = ");

        float division = (float)PlayerBlood / (float)maxValue;
        //Debug.Log(division + " - " + value2 + " > " + minValue + " = ");
        if (division - value2 > minValue)
        {
            //Debug.Log("true");
            PlayerBlood -= Mathf.RoundToInt(value2 * maxValue);
        }
        else
        {
            //Debug.Log("false");
            PlayerBlood = minValue;
        }
        //Debug.Log("Nos queda " + PlayerBlood);
        bloodPlayerValueText.text = PlayerBlood.ToString();
    }

    public void IncreasePlayerBloodValue (int value)
    {
        float value2 = value / (float)maxValue;
        float division = (float)PlayerBlood / (float)maxValue;
        if (division + value2 < maxValue)
        {
            PlayerBlood += Mathf.RoundToInt(value2 * maxValue);
        }
        else
        {
            PlayerBlood = maxValue;
        }
        bloodPlayerValueText.text = PlayerBlood.ToString();
    }

    public void SetPlayerBlood(int value)
    {
        PlayerBlood = value;
        bloodPlayerValueText.text = PlayerBlood.ToString();
    }

    public void SetAIBlood(int value)
    {
        AIBlood = value;
    }

    public int GetCurrentPlayerBlood()
    {
        return PlayerBlood;
    }

    public void DecreaseAIBloodValue(int value)
    {
        float value2 = value / (float)maxValue;

        float division = (float)AIBlood / (float)maxValue;
        if (division - value2 > minValue)
        {
            AIBlood -= Mathf.RoundToInt(value2 * maxValue);
        }
        else
        {
            AIBlood = minValue;
        }
    }

    public void IncreaseAIBloodValue(int value)
    {
        float value2 = value / (float)maxValue;

        float division = (float)AIBlood / (float)maxValue;
        if (division + value2 < maxValue)
        {
            AIBlood += Mathf.RoundToInt(value2 * maxValue);
        }
        else
        {
            AIBlood = maxValue;
        }
    }

    public int GetCurrentAIBlood()
    {
        return AIBlood;
    }
}
