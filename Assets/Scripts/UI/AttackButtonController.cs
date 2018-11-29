using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour {

    public bool buttonsEnabled = false;
    private bool canPressButton = true;

    public void HandleButtons()
    {
        if (!canPressButton)
        {
            return;
        }
        canPressButton = false;
        if (!buttonsEnabled)
        {
            ShowButtons();
        }
        else
        {
            HideButtons();
        }
    }

    private void ShowButtons()
    {
        Sequence s = DOTween.Sequence();
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            Debug.Log(this.transform.GetChild(i).name);
            s.AppendCallback(() =>
            {
                EnableButton(child);
                child.transform.DOShakePosition(0.3f, 10f);
            });
            s.AppendInterval(0.1f);
        }
        s.OnComplete(() =>
        {
            buttonsEnabled = true;
            canPressButton = true;
        });
    }

    private void HideButtons()
    {
        Sequence s = DOTween.Sequence();
        int childCount = this.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            Debug.Log(this.transform.GetChild(i).name);
            s.AppendCallback(() =>
            {
                DisableButton(child);
                child.transform.DOShakePosition(0.3f, 10f);
            });
            s.AppendInterval(0.1f);
        }
        s.OnComplete(() =>
        {
            buttonsEnabled = false;
            canPressButton = true;
        });
    }

    private void EnableButton(GameObject buttonObject)
    {
        buttonObject.GetComponent<Image>().enabled = true;
        buttonObject.GetComponent<Button>().enabled = true;
    }

    private void DisableButton(GameObject buttonObject)
    {
        buttonObject.GetComponent<Image>().enabled = false;
        buttonObject.GetComponent<Button>().enabled = false;
    }
}
