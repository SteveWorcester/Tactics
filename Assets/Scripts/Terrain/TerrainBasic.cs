using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBasic : TerrainGeneric
{
    public float AddMoveDifficulty = 0.0f; //NYI
    public bool BlocksLineOfSight = false; //NYI
    public bool Pathable = true;
    public Color UnitLocationColor = Color.yellow;
    public Color TargetLocationColor = Color.blue;
    public Color MoveSelectableTileColor = Color.cyan;
    public Color IsUnpathableColor = Color.white;
    public Color NotSpecialColor = Color.clear;
    public Color AttackableColor = Color.red;

    void Start()
    {
        IsPathable = Pathable;
        isUnitLocationColor = UnitLocationColor;
        isTargetLocationColor = TargetLocationColor;
        isMoveSelectableColor = MoveSelectableTileColor;
        isPathableColor = IsUnpathableColor;
        isNotSpecialColor = NotSpecialColor;
        isAttackableColor = AttackableColor;
        addTileMoveDifficulty = AddMoveDifficulty;
    }
}
