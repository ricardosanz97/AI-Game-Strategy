using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public override void Start()
    {
        CurrentRotation = TROTATION.Front;

        FSMSystem.AddState(this, new State(STATE.Idle, this,
            () =>
            {
                int offset = GetComponent<AreaAttack>().offset  + 2;
                CustomPathfinding.Node node = null;
                switch (CurrentRotation)
                {
                    case TROTATION.Front:
                        node = this.cell.PNode.FindNodeFromThis(offset,0);
                        Debug.Log("centro de la explosión en " + node.GridX + ", " + node.GridZ);
                        break;
                    case TROTATION.Right:
                        node = this.cell.PNode.FindNodeFromThis(0,-offset);
                        Debug.Log("centro de la explosión en " + node.GridX + ", " + node.GridZ);
                        break;
                    case TROTATION.Back:
                        node = this.cell.PNode.FindNodeFromThis(-offset,0);
                        Debug.Log("centro de la explosión en " + node.GridX + ", " + node.GridZ);
                        break;
                    case TROTATION.Left:
                        node = this.cell.PNode.FindNodeFromThis(0,offset);
                        Debug.Log("centro de la explosión en " + node.GridX + ", " + node.GridZ);
                        break;
                }

                if (node != null)
                {
                    Transform t = GameObject.Find("Node Container").transform; 
                    for (int i = 0; i<t.childCount; i++)
                    {
                        if (t.GetChild(i).GetComponent<CustomPathfinding.Node>().GridX == node.GridX && t.GetChild(i).GetComponent<CustomPathfinding.Node>().GridZ == node.GridZ)
                        {
                            t.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.cyan;
                        }
                    }
                    this.cell.PNode.FindNodeFromThis(0, 0).ColorAsPossibleTurretExplosion();
                    //TODO: esto no se pq no va.
                    //CustomPathfinding.Node[] listNodes = _pathfindingManager.RequestNodesAtRadius(GetComponent<AreaAttack>().areaSize, node.WorldPosition);
                    /*
                    foreach (CustomPathfinding.Node n in listNodes)
                    {
                        n.ColorAsPossibleTurretExplosion();
                    }
                    */

                    Debug.Log("el nodo en " + node.GridX + ", " + node.GridZ + " se pinta?");
                    //this.cell.PNode.ColorAsPossibleTurretExplosion();
                }       
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
    }
}
