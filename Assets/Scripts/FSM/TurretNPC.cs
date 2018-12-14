using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPathfinding;

[RequireComponent(typeof(AreaAttack))]
public class TurretNPC : AbstractNPCBrain
{
    public TROTATION CurrentRotation;
    public enum TROTATION
    {
        //sentido horario
        Front,
        Right,
        Back,
        Left
    }
    public List<CellBehaviour> CellsUnderMyAttack = new List<CellBehaviour>();
    public List<Node> nodeAffectedList = new List<Node>();
    public override void Start()
    {
        CurrentRotation = TROTATION.Front;

        FSMSystem.AddState(this, new State(STATE.Idle, this,
            () =>
            {
                 GetInitialDamage();
                 SetAffectedCells();
            },
            () =>
            {

            }));

        SetStates();
        SetTransitions();
        currentState = states.Find((x) => x.stateName == STATE.Idle);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);
        currentState.OnEnter();

        base.Start();
    }

    public override void SetStates()
    {
        FSMSystem.AddState(this, new State(STATE.Remain, this));
        SetAttackState();
        //SetIdleState();
    }

    public void SetAttackState()
    {
        FSMSystem.AddState(this, new State(STATE.Attack, this));
    }

    public override void SetTransitions()
    {
    }

    public void OnMouseDown()
    {
        Debug.Log("click in enemy");
        if (_levelController.GetAnyPopupEnabled() || this.owner == Owner.AI || this.GetComponent<AbstractNPCBrain>().executed)
        {
            return;
        }
        popupOptionsEnabled = true;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleOptionsPopup"));
        go.GetComponent<SimpleOptionsPopupController>().SetPopup(
        this.transform.localPosition,
        this.npc.ToString(),
        "RIGHT",
        "LEFT",
        () => {
            //rotate 90
            RotateRight();
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;

        },
        () => {
            //rotate -90
            RotateLeft();
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;
        },
        () =>
        {
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;
        });
    }

    private void RotateRight()
    {
        this.transform.Rotate(Vector3.up * 90f);
        switch (CurrentRotation)
        {
            case TROTATION.Front:
                CurrentRotation = TROTATION.Right;
                break;
            case TROTATION.Right:
                CurrentRotation = TROTATION.Back;
                break;
            case TROTATION.Back:
                CurrentRotation = TROTATION.Left;
                break;
            case TROTATION.Left:
                CurrentRotation = TROTATION.Front;
                break;
        }
        UpdateAffectedCells();
    }

    private void RotateLeft()
    {
        this.transform.Rotate(Vector3.up * -90f);
        switch (CurrentRotation)
        {
            case TROTATION.Front:
                CurrentRotation = TROTATION.Left;
                break;
            case TROTATION.Right:
                CurrentRotation = TROTATION.Front;
                break;
            case TROTATION.Back:
                CurrentRotation = TROTATION.Right;
                break;
            case TROTATION.Left:
                CurrentRotation = TROTATION.Back;
                break;
        }
        UpdateAffectedCells();
    }

    public void UpdateAffectedCells()
    {
        List<CellBehaviour> affectedCells = CellsUnderMyAttack;
        foreach (CellBehaviour cell in affectedCells)
        {
            cell.explosionBelongsTo.Remove(this);
            if (cell.explosionBelongsTo.Count <= 0)
            {
                //cell.PNode.ResetColor();
                cell.gameObject.transform.Find("ProjectilePlacement").gameObject.SetActive(false);
            }
        }
        CellsUnderMyAttack.Clear();

        SetAffectedCells();

    }

    public void SetAffectedCells()
    {
        int offset = GetComponent<AreaAttack>().offset + 2;
        CustomPathfinding.Node node = null;
        switch (CurrentRotation)
        {
            case TROTATION.Front:
                node = this.cell.PNode.FindNodeFromThis(offset, 0);
                break;
            case TROTATION.Right:
                node = this.cell.PNode.FindNodeFromThis(0, -offset);
                break;
            case TROTATION.Back:
                node = this.cell.PNode.FindNodeFromThis(-offset, 0);
                break;
            case TROTATION.Left:
                node = this.cell.PNode.FindNodeFromThis(0, offset);
                break;
        }

        if (node != null)
        {

            nodeAffectedList = _pathfindingManager.RequestNodesAtRadius(GetComponent<AreaAttack>().areaSize, node.WorldPosition);

            for (int i = 0; i < nodeAffectedList.Count; i++)
            {
                //nodeAffectedList[i].ColorAsPossibleTurretExplosion();
                nodeAffectedList[i].cell.gameObject.transform.Find("ProjectilePlacement").gameObject.SetActive(true);
                this.CellsUnderMyAttack.Add(nodeAffectedList[i].GetOurCell());
                nodeAffectedList[i].cell.explosionBelongsTo.Add(this);
            }

            //node.ColorAsPossibleTurretExplosion();
            node.cell.gameObject.transform.Find("ProjectilePlacement").gameObject.SetActive(true);
            this.CellsUnderMyAttack.Add(node.GetOurCell());
            node.cell.explosionBelongsTo.Add(this);

            Debug.Log("el nodo en " + node.GridX + ", " + node.GridZ + " se pinta?");
        }



        for (int i = 0; i < CellsUnderMyAttack.Count; i++)
        {
            if (CellsUnderMyAttack[i].troopIn != null && CellsUnderMyAttack[i].troopIn.owner != this.owner)
            {
                CellsUnderMyAttack[i].troopIn.GetComponent<Health>().ReceiveDamage(this.GetComponent<AreaAttack>().damage);
            }
        }
    }


    public override void DoAttackAnimation()
    {
        FindObjectOfType<SoundManager>().PlaySingle(FindObjectOfType<SoundManager>().turretSoundShot);
    }

}
