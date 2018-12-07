using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SimpleInfoPopupController : PopupController
{
    public Text titleText;
    public Text infoText;

    public void SetPopup(string titleText, string infoText)
    {
        this.transform.SetParent(GameObject.Find("Canvas").transform);
        this.GetComponent<RectTransform>().localPosition = Vector3.zero;
        OpenPopup();
        this.titleText.text = titleText;
        this.infoText.text = infoText;
        StartCoroutine(PrepareClosePopup());
    }

    IEnumerator PrepareClosePopup()
    {
        yield return new WaitForSeconds(2f);
        ClosePopup();
    }
}
