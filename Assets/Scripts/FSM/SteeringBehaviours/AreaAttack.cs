using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : Action {

    public int areaSize = 1;
    public int offset = 2;
    public CellBehaviour AreaCenterCell;
    public bool AreaCenterSet = false;

    public override void Act()
    {
        if (!AreaCenterSet)
        {
            return;
        }

    }
}
