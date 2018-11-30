using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager
{
    public int TestInt { get; private set; }

    public GameManager(int testInt)
    {
        TestInt = testInt;
    }
}
