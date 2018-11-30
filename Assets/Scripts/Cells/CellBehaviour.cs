using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellBehaviour : MonoBehaviour {
    
    

    private void OnMouseOver()
    {
        this.transform.DOLocalMoveY(0.5f, 0f);
    }

    private void OnMouseExit()
    {
        this.transform.DOLocalMoveY(0f, 0f);
    }

    private void OnMouseDown()
    {
        GameObject.FindObjectOfType<TroopController>().GetComponent<TroopController>().SpawnTroop(this.gameObject);
    }

}
