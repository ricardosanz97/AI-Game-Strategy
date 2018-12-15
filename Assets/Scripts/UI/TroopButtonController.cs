using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TroopButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public TROOP troopType;
    
    [Inject]
    private SpawnablesManager _spawnablesManager;

    private LevelController _levelController;

    [Inject]
    private SoundManager soundManager;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>().GetComponent<LevelController>();
    }

    public void SelectTroop()
    {
        soundManager.PlaySingle(soundManager.buttonPressedSound);
        _levelController.ClosePopups();
        _spawnablesManager.SetCurrentTroop(troopType);
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseExit()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.transform.Translate(Vector3.up * 5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.Translate(Vector3.down * 5f);
    }
}
