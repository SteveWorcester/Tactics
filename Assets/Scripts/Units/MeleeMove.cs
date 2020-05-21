using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMove : UnitMove
{
    // Edit these defaults

    public float MoveSpeed = 10.0f; // how fast the unit traverses the map. This has nothing to do with turn order speed.

    void Start()
    {
        _unitMoveSpeed = MoveSpeed;
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = GetComponent<Collider>().bounds.extents.y;
        Init();
    }
}
