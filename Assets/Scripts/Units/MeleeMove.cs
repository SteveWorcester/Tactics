using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMove : UnitMove
{
    // Edit these defaults
    public int MoveDistance = 5;
    public float MoveSpeed = 2.0f; // how fast the unit traverses the map. This has nothing to do with turn order speed.
    public float JumpHeight = 2.0f;

    // TODOs
    // Turn counter per move action

    void Start()
    {
        _unitMoveSpeed = MoveSpeed;
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        SetAdjacencyList(JumpHeight);
        if (!currentlyMoving)
        {
            SetSelectableTiles(JumpHeight, MoveDistance);
            CheckMouseToMove();
        }
        else
        {
            // move();
        }
    }
}
