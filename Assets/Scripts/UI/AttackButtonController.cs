using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour {

    public bool buttonsEnabled = false;
    [SerializeField] private bool canPressButton = true;
    private LevelController _levelController;
    private TurnHandler _turnHandler;
    [Inject]
    private SoundManager soundManagerRef;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _turnHandler = FindObjectOfType<TurnHandler>();
    }

    public void HandleButtons()
    {
        if (!canPressButton || !_levelController.CheckIfCanSpawn() || _turnHandler.currentTurn != PlayerType.Player)
        {
            if (buttonsEnabled)
            {
                HideButtons();
            }
            
            return;
        }
        canPressButton = false;
        if (!buttonsEnabled)
        {
            ShowButtons();
        }
        else
        {
            soundManagerRef.PlaySingle(soundManagerRef.cardsSound);
            HideButtons();
        }
    }

    public void ShowButtons()
    {
        soundManagerRef.PlaySingle(soundManagerRef.cardsSound);
        _levelController.ClosePopups();
        Sequence s = DOTween.Sequence();
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
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

    public void HideButtons()
    {
        _levelController.ClosePopups();
        Sequence s = DOTween.Sequence();
        int childCount = this.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
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
        buttonObject.GetComponentInChildren<Text>().enabled = true;
        buttonObject.GetComponent<Button>().enabled = true;
    }

    private void DisableButton(GameObject buttonObject)
    {
        buttonObject.GetComponent<Image>().enabled = false;
        buttonObject.GetComponentInChildren<Text>().enabled = false;
        buttonObject.GetComponent<Button>().enabled = false;
    }
}
