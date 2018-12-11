using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SimpleOptionsPopupController : PopupController {
    public Text textPopup;
    public Button button1;
    public Button button2;
    public Button closeButton;

    public void SetPopup(Vector3 pos, string titleText, string button1Text, string button2Text, UnityAction button1Action, UnityAction button2Action, UnityAction closeButtonAction)
    {
        this.transform.SetParent(GameObject.FindGameObjectWithTag("PopupsContainer").transform);
        this.GetComponent<RectTransform>().localPosition = pos;
        OpenPopup();
        this.textPopup.text = titleText;
        this.button1.GetComponentInChildren<Text>().text = button1Text;
        this.button2.GetComponentInChildren<Text>().text = button2Text;
        button1.onClick.AddListener(button1Action);
        button2.onClick.AddListener(button2Action);
        closeButton.onClick.AddListener(closeButtonAction);
    }
}
