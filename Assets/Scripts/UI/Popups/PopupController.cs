using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupController : MonoBehaviour{
    public void ClosePopup()
    {
        Sequence s = DOTween.Sequence();
        s.Append(this.gameObject.transform.DOScale(0f, 0.5f).SetEase(Ease.InOutCubic));
        s.OnComplete(() =>
        {
            Destroy(this.gameObject);
        });
    }

    public void OpenPopup()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.transform.DOScale(0f, 0f);
        Sequence s = DOTween.Sequence();
        s.Append(this.gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad));
    }
}
