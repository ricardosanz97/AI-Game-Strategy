using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupController : MonoBehaviour{
    public void ClosePopup()
    {

    }

    public void OpenPopup()
    {
        this.transform.DOScale(0f, 0f);
        Sequence s = DOTween.Sequence();
        s.Append(this.gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad));
    }
}
