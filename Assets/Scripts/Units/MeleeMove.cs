using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMove : UnitMove
{
    // Edit these defaults
    public int Move = 5;
    public float MoveSpeed = 2.0f;
    public float JumpHeight = 2.0f;
    
    void Start()
    {
              
    }

    void Update()
    {
        base.SetAdjacencyList(JumpHeight);
        base.SetSelectableTiles(JumpHeight, Move);
    }
}
